using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PcmHacking
{
    /// <summary>
    /// How much of the IPC to erase and rewrite.
    /// </summary>
    public enum WriteTypeIpc  
    {
        None = 0,
        Compare,
        TestWrite,
        Calibration,
        Parameters,
        OsPlusCalibrationPlusBoot,
        Full,
        Ipc,
        Ipc1,
    }

    public class CKernelWriterIpc
    {
        private readonly Vehicle vehicle;
        private readonly Protocol protocol;
        private readonly WriteTypeIpc writeTypeIpc;
        private readonly ILogger logger;

        public CKernelWriterIpc(Vehicle vehicle, Protocol protocol, WriteTypeIpc writeTypeIpc, ILogger logger)
        {
            this.vehicle = vehicle;
            this.protocol = protocol;
            this.writeTypeIpc = writeTypeIpc;
            this.logger = logger;
        }

        /// <summary>
        /// Write changes to the IPC's flash memory, or just test writing (Without 
        /// making changes) to evaluate the connection quality.
        /// </summary>
        public async Task<bool> Write(
            byte[] image,
            UInt32 kernelVersion,
            FileValidator validator,
            bool needToCheckOperatingSystem,
            CancellationToken cancellationToken)
        {
            bool success = false;

            try
            {
                // Start with known state.
                await this.vehicle.ForceSendToolPresentNotification();
                this.vehicle.ClearDeviceMessageQueue();

                // TODO: install newer version if available.
                if (kernelVersion == 0)
                {
                    // Switch to 4x, if possible. But continue either way.
                    if (Configuration.Enable4xReadWrite)
                    {
                        // if the vehicle bus switches but the device does not, the bus will need to time out to revert back to 1x, and the next steps will fail.
                        if (!await this.vehicle.VehicleSetVPW4x(VpwSpeed.FourX))
                        {
                            this.logger.AddUserMessage("Stopping here because we were unable to switch to 4X.");
                            return false;
                        }
                    }
                    else
                    {
                        this.logger.AddUserMessage("4X communications disabled by configuration.");
                    }

                   
                   
                }
                ///var sweepResponse = await this.vehicle.startProgram();

                // TODO: instead of this hard-coded address, get the base address from the PcmInfo object.
                int address1 = 0x020000;
                int claimedSize1 = 0x000002;
                if (!await this.vehicle.PCMStartProgram(address1, claimedSize1, cancellationToken))
                {
                    logger.AddUserMessage("Start Program to IPC denied");

                    return false;
                }

                logger.AddUserMessage("Start Program to IPC allowed.");

                /// var modeResponse = await this.vehicle.requestmode34();
                // TODO: instead of this hard-coded address, get the base address from the PcmInfo object.
                int address = 0x00E000;
                int claimedSize = 0x0400;
                if (!await this.vehicle.PCMExecute1( address, claimedSize, cancellationToken))
                {
                    logger.AddUserMessage("Upload to IPC denied");

                    return false;
                }

                logger.AddUserMessage("Upload to IPC allowed.");

                success = await this.Write(cancellationToken, image);

                // We only do cleanup after a successful write.
                // If the kernel remains running, the user can try to flash again without rebooting and reloading.
                // TODO: kernel version should be stored at a fixed location in the bin file.
                // TODO: app should check kernel version (not just "is present") and reload only if version is lower than version in kernel file.
                if (success)
                {
                    await this.vehicle.Cleanup();
                }

                return success;
            }
            catch (Exception exception)
            {
                if (!success)
                {
                    switch (this.writeTypeIpc)
                    {
                        case WriteTypeIpc.None:
                        case WriteTypeIpc.Compare:
                        case WriteTypeIpc.TestWrite:
                            await this.vehicle.Cleanup();
                            this.logger.AddUserMessage("Something has gone wrong. Please report this error.");
                            this.logger.AddUserMessage("Errors during comparisons or test writes indicate a");
                            this.logger.AddUserMessage("problem with the PCM, interface, cable, or app. Don't");
                            this.logger.AddUserMessage("try to do any actual writing until you are certain that");
                            this.logger.AddUserMessage("the underlying problem has been completely corrected.");
                            break;

                        default:
                            this.logger.AddUserMessage("Something went wrong. " + exception.Message);
                            this.logger.AddUserMessage("Do not power off the PCM! Do not exit this program!");
                            this.logger.AddUserMessage("Try flashing again. If errors continue, seek help online.");
                            break;
                    }

                    ///this.logger.AddUserMessage("https://pcmhacking.net/forums/viewtopic.php?f=42&t=6080");
                    this.logger.AddUserMessage(string.Empty);
                    this.logger.AddUserMessage(exception.ToString());
                }

                return success;
            }
        }

        /// <summary>
        /// Write the calibration blocks.
        /// </summary>
        private async Task<bool> Write(CancellationToken cancellationToken, byte[] image)
        {
            await this.vehicle.SendToolPresentNotification();

            BlockType relevantBlocks;
            switch (this.writeTypeIpc)
            {
                case WriteTypeIpc.Ipc:
                    relevantBlocks = BlockType.IPC;
                    break;
                case WriteTypeIpc.Ipc1:
                    relevantBlocks = BlockType.IPC1;
                    break;
                default:
                    throw new InvalidDataException("Unsuppported operation type: " + this.writeTypeIpc.ToString());
            }


            
            
            // Which flash chip?
            await this.vehicle.SendToolPresentNotification();
            UInt32 chipId;
            if (image.Length == 112 * 1024)
            {
                chipId = 0x6D696B65;

            }
            else
            {
                chipId = 0x4D696B65;
            }
            FlashChip flashChip = FlashChip.Create(chipId, this.logger);
            logger.AddUserMessage("Flash chip: " + flashChip.ToString());

            if (flashChip.Size != image.Length)
            {
                this.logger.AddUserMessage(string.Format("File size {0:n0} does not match Flash Chip size {1:n0}!", image.Length, flashChip.Size));
                await this.vehicle.Cleanup();
                return false;
            }


            // Erase and rewrite the required memory ranges.
            DateTime startTime = DateTime.Now;
            UInt32 totalSize = this.GetTotalSize(flashChip, relevantBlocks);
            UInt32 bytesRemaining = totalSize;
            foreach (MemoryRange range in flashChip.MemoryRanges)
            {
                // We'll send a tool-present message during the erase request.
                if (!this.ShouldProcess(range, relevantBlocks))
                {
                    continue;
                }

                this.logger.AddUserMessage(
                    string.Format(
                        "Processing range {0:X6}-{1:X6}",
                        range.Address,
                        range.Address + (range.Size - 1)));

               
                if (this.writeTypeIpc == WriteTypeIpc.TestWrite)
                {
                    this.logger.AddUserMessage("Pretending to write...");
                }
                else
                {
                    this.logger.AddUserMessage("Writing...");
                }

                this.logger.AddUserMessage("Address\t% Done\tTime Remaining");




                Response<bool> writeResponse = await WriteMemoryRange(
                    range,
                    image,
                    this.writeTypeIpc == WriteTypeIpc.TestWrite,
                    startTime,
                    totalSize,
                    bytesRemaining,
                    cancellationToken);

               

                if (writeResponse.Value)
                {
                    bytesRemaining -= range.Size;
                }
            }
    


            return true;
        }

        private UInt32 GetTotalSize(FlashChip chip, BlockType relevantBlocks)
        {
            UInt32 result = 0;
            foreach (MemoryRange range in chip.MemoryRanges)
            {
                if (this.ShouldProcess(range, relevantBlocks))
                {
                    result += range.Size;
                }
            }

            return result;
        }


        private bool ShouldProcess(MemoryRange range, BlockType relevantBlocks)
        {
            
            return true;
        }
        /// <summary>
        /// Copy a single memory range to the PCM.
        /// </summary>
        private async Task<Response<bool>> WriteMemoryRange(
                MemoryRange range,
                byte[] image,
                bool justTestWrite,
                DateTime startTime,
                UInt32 totalSize,
                UInt32 bytesRemaining,
                CancellationToken cancellationToken)
            {
                int retryCount = 0;
                int devicePayloadSize = vehicle.DeviceMaxFlashWriteSendSize - 12; // Headers use 10 bytes, sum uses 2 bytes.
                for (int index = 0; index < range.Size; index += devicePayloadSize)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return Response.Create(ResponseStatus.Cancelled, false, retryCount);
                    }

                    await this.vehicle.SendToolPresentNotification();

                ///int startAddress = (int)(0x000000);
                int startAddress = (int)(range.Address + index);
                int ramAddress = (int)(0x00E000);
                    UInt32 thisPayloadSize = (UInt32)Math.Min(devicePayloadSize, (int)range.Size - index);

                    logger.AddDebugMessage(
                        string.Format(
                            "Sending payload with offset 0x{0:X4}, start address 0x{1:X6}, length 0x{2:X4}.",
                            index,
                            startAddress,
                            thisPayloadSize));

                    Message payloadMessage = protocol.CreateBlockMessage(
                        image,
                        startAddress,
                        (int)thisPayloadSize,
                        ramAddress,
                        justTestWrite ? BlockCopyType.TestWrite : BlockCopyType.Copy);

                    string timeRemaining;

                    TimeSpan elapsed = DateTime.Now - startTime;
                    UInt32 totalWritten = totalSize - bytesRemaining;

                    // Wait 10 seconds before showing estimates.
                    if (elapsed.TotalSeconds < 10)
                    {
                        timeRemaining = "Measuring write speed...";
                    }
                    else
                    {
                        UInt32 bytesPerSecond = (UInt32)(totalWritten / elapsed.TotalSeconds);

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
                            totalWritten * 100 / totalSize,
                            timeRemaining));

                    await this.vehicle.SetDeviceTimeout(TimeoutScenario.WriteMemoryBlock);

                    // WritePayload contains a retry loop, so if it fails, we don't need to retry at this layer.
                    Response<bool> response = await this.vehicle.WritePayload(payloadMessage, cancellationToken);
                    if (response.Status != ResponseStatus.Success)
                    {
                        return Response.Create(ResponseStatus.Error, false, response.RetryCount);
                    }

                    bytesRemaining -= thisPayloadSize;
                    retryCount += response.RetryCount;
                }

                return Response.Create(ResponseStatus.Success, true, retryCount);
        }
        

    }
}
