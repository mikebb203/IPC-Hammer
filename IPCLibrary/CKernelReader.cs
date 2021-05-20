﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PcmHacking
{
    /// <summary>
    /// Reader classes use a kernel to read the entire flash memory.
    /// </summary>
    public class CKernelReader
    {
        private readonly Vehicle vehicle;
        private readonly Protocol protocol;
        private readonly ILogger logger;

        public CKernelReader(Vehicle vehicle, ILogger logger)
        {
            this.vehicle = vehicle;

            // This seems wrong... Some alternatives:
            // a) Have the caller pass in the message factory and message-parser methods
            // b) Have the caller pass in a smaller KernelProtocol class - with subclasses for each kernel - 
            //    This would only make sense if it turns out that this one reader class can handle multiple kernels.
            // c) Just create a smaller KernelProtocol class here, for the kernel that this class is intended for.
            this.protocol = new Protocol();

            this.logger = logger;
        }

        /// <summary>
        /// Read the full contents of the PCM.
        /// Assumes the PCM is unlocked and we're ready to go.
        /// </summary>
        public async Task<Response<Stream>> ReadContents(CancellationToken cancellationToken)
        {
            try
            {
                // Start with known state.
                await this.vehicle.ForceSendToolPresentNotification();
                this.vehicle.ClearDeviceMessageQueue();

                // Switch to 4x, if possible. But continue either way.
                if (Configuration.Enable4xReadWrite)
                {
                    // if the vehicle bus switches but the device does not, the bus will need to time out to revert back to 1x, and the next steps will fail.
                    if (!await this.vehicle.VehicleSetVPW4x(VpwSpeed.FourX))
                    {
                        this.logger.AddUserMessage("Stopping here because we were unable to switch to 4X.");
                        return Response.Create(ResponseStatus.Error, (Stream)null);
                    }
                }
                else
                {
                    this.logger.AddUserMessage("4X communications disabled by configuration.");
                }

                await this.vehicle.SendToolPresentNotification();

                // execute read kernel
                Response<byte[]> response = await vehicle.LoadKernelFromFile("kernel.bin");
                if (response.Status != ResponseStatus.Success)
                {
                    logger.AddUserMessage("Failed to load kernel from file.");
                    return new Response<Stream>(response.Status, null);
                }
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return Response.Create(ResponseStatus.Cancelled, (Stream)null);
                }

                await this.vehicle.SendToolPresentNotification();

                // TODO: instead of this hard-coded 0xFF9150, get the base address from the PcmInfo object.
                // TODO: choose kernel at run time? Because now it's FF8000...
                if (!await this.vehicle.PCMExecute(response.Value, 0xFF8000, cancellationToken))
                {
                    logger.AddUserMessage("Failed to upload kernel to PCM");

                    return new Response<Stream>(
                        cancellationToken.IsCancellationRequested ? ResponseStatus.Cancelled : ResponseStatus.Error, 
                        null);
                }

                logger.AddUserMessage("kernel uploaded to PCM succesfully. Requesting data...");

                // Which flash chip?
                await this.vehicle.SendToolPresentNotification();
                UInt32 chipId = await this.vehicle.QueryFlashChipId(cancellationToken);
                FlashChip flashChip = FlashChip.Create(chipId, this.logger);
                logger.AddUserMessage("Flash chip: " + flashChip.ToString());

                await this.vehicle.SetDeviceTimeout(TimeoutScenario.ReadMemoryBlock);

                this.logger.AddUserMessage("Address\t% Done\tTime Remaining");

                byte[] image = new byte[flashChip.Size];
                int retryCount = 0;
                int startAddress = 0;
                int endAddress = (int)flashChip.Size;
                int bytesRemaining = (int)flashChip.Size;
                int blockSize = this.vehicle.DeviceMaxReceiveSize - 10 - 2; // allow space for the header and block checksum

                DateTime startTime = DateTime.MaxValue;
                while (startAddress < endAddress)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Response.Create(ResponseStatus.Cancelled, (Stream)null);
                    }

                    // The read kernel needs a short message here for reasons unknown. Without it, it will RX 2 messages then drop one.
                    await this.vehicle.ForceSendToolPresentNotification();

                    if (startAddress + blockSize > endAddress)
                    {
                        blockSize = endAddress - startAddress;
                    }

                    if (blockSize < 1)
                    {
                        this.logger.AddUserMessage("Image download complete");
                        break;
                    }

                    if (startTime == DateTime.MaxValue)
                    {
                        startTime = DateTime.Now;
                    }

                    Response<bool> readResponse = await TryReadBlock(
                        image, 
                        blockSize, 
                        startAddress,
                        startTime,
                        cancellationToken);
                    if (readResponse.Status != ResponseStatus.Success)
                    {
                        this.logger.AddUserMessage(
                            string.Format(
                                "Unable to read block from {0} to {1}",
                                startAddress,
                                (startAddress + blockSize) - 1));
                        return new Response<Stream>(ResponseStatus.Error, null);
                    }

                    startAddress += blockSize;
                    retryCount += readResponse.RetryCount;
                }

                logger.AddUserMessage("Read complete.");
                Utility.ReportRetryCount("Read", retryCount, flashChip.Size, this.logger);
                logger.AddUserMessage("Starting verification...");

                CKernelVerifier verifier = new CKernelVerifier(
                    image,
                    flashChip.MemoryRanges,
                    this.vehicle,
                    this.protocol,
                    this.logger);

                if (await verifier.CompareRanges(
                    image,
                    BlockType.All,
                    cancellationToken))
                {
                    logger.AddUserMessage("The contents of the file match the contents of the PCM.");
                }
                else
                {
                    logger.AddUserMessage("##############################################################################");
                    logger.AddUserMessage("There are errors in the data that was read from the PCM. Do not use this file.");
                    logger.AddUserMessage("##############################################################################");
                }

                await this.vehicle.Cleanup(); // Not sure why this does not get called in the finally block on successfull read?

                MemoryStream stream = new MemoryStream(image);
                return new Response<Stream>(ResponseStatus.Success, stream);
            }
            catch(Exception exception)
            {
                this.logger.AddUserMessage("Something went wrong. " + exception.Message);
                this.logger.AddDebugMessage(exception.ToString());
                return new Response<Stream>(ResponseStatus.Error, null);
            }
            finally
            {
                // Sending the exit command at both speeds and revert to 1x.
                await this.vehicle.Cleanup();
            }
        }

        /// <summary>
        /// Try to read a block of PCM memory.
        /// </summary>
        private async Task<Response<bool>> TryReadBlock(
            byte[] image, 
            int length, 
            int startAddress, 
            DateTime startTime,
            CancellationToken cancellationToken)
        {
            this.logger.AddDebugMessage(string.Format("Reading from {0} / 0x{0:X}, length {1} / 0x{1:X}", startAddress, length));

            int retryCount = 0;
            for (; retryCount < Vehicle.MaxSendAttempts; retryCount++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                Response<byte[]> readResponse = await this.vehicle.ReadMemory(
                    () => this.protocol.CreateReadRequest(startAddress, length),
                    (payloadMessage) => this.protocol.ParsePayload(payloadMessage, length, startAddress),
                    cancellationToken);

                if(readResponse.Status != ResponseStatus.Success)
                {
                    this.logger.AddDebugMessage("Unable to read segment: " + readResponse.Status);
                    continue;
                }

                byte[] payload = readResponse.Value;

                if (payload.Length != length)
                {
                    this.logger.AddUserMessage(
                        string.Format(
                            "Expected {0} bytes, received {1} bytes.",
                            length,
                            payload.Length));
                    return Response.Create(ResponseStatus.Truncated, false);
                }

                Buffer.BlockCopy(payload, 0, image, startAddress, payload.Length);

                int percentDone = (startAddress * 100) / image.Length;
                TimeSpan elapsed = DateTime.Now - startTime;
                string timeRemaining;

                // Wait 10 seconds before showing time estimate.
                if (elapsed.TotalSeconds < 10)
                {
                    timeRemaining = "Measuring read speed...";
                }
                else
                {
                    UInt32 bytesPerSecond = (UInt32)(startAddress / elapsed.TotalSeconds);
                    UInt32 bytesRemaining = (UInt32)(image.Length - startAddress);

                    // Don't divide by zero.
                    if (bytesPerSecond > 0)
                    {
                        UInt32 secondsRemaining = (UInt32)(bytesRemaining / bytesPerSecond);
                        timeRemaining = TimeSpan.FromSeconds(secondsRemaining).ToString("mm\\:ss");
                    }
                    else
                    {
                        timeRemaining = "??:??";
                    }
                }

                logger.AddUserMessage(
                    string.Format(
                        "0x{0:X6}\t{1}%\t{2}",
                        startAddress,
                        startAddress * 100 / image.Length,
                        timeRemaining));

                return Response.Create(ResponseStatus.Success, true, retryCount);
            }

            return Response.Create(ResponseStatus.Error, false, retryCount);
        }
    }
}
