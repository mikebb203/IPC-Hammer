﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PcmHacking
{
    public enum BlockCopyType
    {
        // Copy to RAM or Flash
        Copy = 0x00,

        // Execute after copying to RAM
        Execute = 0x80,

        // Test copy to flash, but do not unlock or actually write.
        TestWrite = 0x44,
    };

    public partial class Protocol
    {
        /// <summary>
        /// Create a block message from the supplied arguments.
        /// </summary>
        public Message CreateBlockMessage(byte[] Payload, int Offset, int Length, int Address, BlockCopyType copyType)
        {
            byte[] Buffer = new byte[10 + Length + 2];
            byte[] Header = new byte[10];

            byte Size1 = unchecked((byte)(Length >> 8));
            byte Size2 = unchecked((byte)(Length & 0xFF));
            byte Addr1 = unchecked((byte)(Address >> 16));
            byte Addr2 = unchecked((byte)(Address >> 8));
            byte Addr3 = unchecked((byte)(Address & 0xFF));

            Header[0] = Priority.Block;
            Header[1] = DeviceId.Pcm;
            Header[2] = DeviceId.Tool;
            Header[3] = Mode.PCMUpload;
            Header[4] = (byte)copyType;
            Header[5] = Size1;
            Header[6] = Size2;
            Header[7] = Addr1;
            Header[8] = Addr2;
            Header[9] = Addr3;

            System.Buffer.BlockCopy(Header, 0, Buffer, 0, Header.Length);
            System.Buffer.BlockCopy(Payload, Offset, Buffer, Header.Length, Length);

            return new Message(VpwUtilities.AddBlockChecksum(Buffer));
        }

        /// <summary>
        /// Create a request to uploade size bytes to the given address
        /// </summary>
        /// <remarks>
        /// Note that mode 0x34 is only a request. The actual payload is sent as a mode 0x36.
        /// </remarks>
        public Message CreateUploadRequest(int Address, int Size)
        {
            byte[] requestBytes = { Priority.Physical0, DeviceId.Pcm, DeviceId.Tool, Mode.PCMUploadRequest, SubMode.Null, 0x00, 0x00, 0x00, 0x00, 0x00 };
            requestBytes[5] = unchecked((byte)(Size >> 8));
            requestBytes[6] = unchecked((byte)(Size & 0xFF));
            requestBytes[7] = unchecked((byte)(Address >> 16));
            requestBytes[8] = unchecked((byte)(Address >> 8));
            requestBytes[9] = unchecked((byte)(Address & 0xFF));

            return new Message(requestBytes);
        }

        public Message CreateStartProgramRequest(int Address, int Size)
        {
            byte[] requestBytes = { Priority.Physical0, DeviceId.Pcm, DeviceId.Tool, Mode.Devicecontrol, SubMode.Program, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            requestBytes[5] = unchecked((byte)(Size >> 16));
            requestBytes[6] = unchecked((byte)(Size >> 8));
            requestBytes[7] = unchecked((byte)(Size & 0xFF));
            requestBytes[8] = unchecked((byte)(Address >> 16));
            requestBytes[9] = unchecked((byte)(Address >> 8));
            requestBytes[10] = unchecked((byte)(Address & 0xFF));

            return new Message(requestBytes);
        }

        /// <summary>
        /// Parse the response to a request for permission to upload a RAM kernel (or part of a kernel).
        /// </summary>
        public Response<bool> ParseUploadPermissionResponse(Message message)
        {
            Response<bool> response = this.DoSimpleValidation(message, Priority.Physical0, Mode.PCMUploadRequest);

            if (response.Status == ResponseStatus.Success || response.Status == ResponseStatus.Refused)
            {
                return response;
            }

            // In case the PCM sends back a 7F message with an 8C priority byte...
            return this.DoSimpleValidation(message, Priority.Physical0High, Mode.PCMUploadRequest);
        }

        /// <summary>
        /// Parse the response to a request for permission to upload a RAM kernel (or part of a kernel).
        /// </summary>
        public Response<bool> ParseUploadPermissionResponse1(Message message)
        {
            Response<bool> response = this.DoSimpleValidation(message, Priority.Physical0High, Mode.PCMUploadRequest);

            if (response.Status == ResponseStatus.Success || response.Status == ResponseStatus.Refused)
            {
                return response;
            }

            // In case the PCM sends back a 7F message with an 8C priority byte...
            return this.DoSimpleValidation(message, Priority.Physical0High, Mode.PCMUploadRequest);
        }

        /// <summary>
        /// Parse the response to a request for permission to upload a RAM kernel (or part of a kernel).
        /// </summary>
        public Response<bool> ParseStartProgramResponse(Message message)
        {
            Response<bool> response = this.DoSimpleValidation(message, Priority.Physical0High, Mode.Devicecontrol);

            if (response.Status == ResponseStatus.Success || response.Status == ResponseStatus.Refused)
            {
                return response;
            }

            // In case the PCM sends back a 7F message with an 8C priority byte...
            return this.DoSimpleValidation(message, Priority.Physical0High, Mode.Devicecontrol);
        }

        /// <summary>
        /// Parse the response to an upload-to-RAM request.
        /// </summary>
        public Response<bool> ParseUploadResponse(Message message)
        {
            return this.DoSimpleValidation(message, Priority.Block, Mode.PCMUpload);
        }

        /// <summary>
        /// Parse the response to an upload-to-RAM request.
        /// </summary>
        public Response<bool> ParseUploadResponse1(Message message)
        {
            return this.DoSimpleValidation2(message, Priority.Physical0High, Mode.PCMUpload, 0x00, 0x73, 0x86);
        }

        public Response<bool> ParseUploadResponse2(Message message)
        {
            return this.DoSimpleValidation3(message, Priority.Physical0High, Mode.PCMUpload, 0x00, 0x78);
        }
        /// <summary>
        /// Create a request to read an arbitrary address range.
        /// </summary>
        /// <remarks>
        /// This command is only understood by the reflash kernel.
        /// </remarks>
        /// <param name="startAddress">Address of the first byte to read.</param>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns></returns>
        public Message CreateReadRequest(int startAddress, int length)
        {
            byte[] request = { 0x6D, DeviceId.Pcm, DeviceId.Tool, 0x35, 0x01, (byte)(length >> 8), (byte)(length & 0xFF), (byte)(startAddress >> 16), (byte)((startAddress >> 8) & 0xFF), (byte)(startAddress & 0xFF) };
            byte[] request2 = { 0x6D, DeviceId.Pcm, DeviceId.Tool, 0x37, 0x01, (byte)(length >> 8), (byte)(length & 0xFF), (byte)(startAddress >> 24), (byte)(startAddress >> 16), (byte)((startAddress >> 8) & 0xFF), (byte)(startAddress & 0xFF) };

            if (startAddress > 0xFFFFFF)
            {
                return new Message(request2);
            }
            else
            {
                return new Message(request);
            }
        }

        /// <summary>
        /// Parse the payload of a read request.
        /// </summary>
        /// <remarks>
        /// It is the callers responsability to check the ResponseStatus for errors
        /// </remarks>
        public Response<byte[]> ParsePayload(Message message, int length, int expectedAddress)
        {
            ResponseStatus status;
            byte[] actual = message.GetBytes();
            byte[] expected = new byte[] { 0x6D, 0xF0, 0x10, 0x36 };
            if (!TryVerifyInitialBytes(actual, expected, out status))
            {
                return Response.Create(status, new byte[0]);
            }

            // Ensure that we can read the data length and start address from the message.
            if (actual.Length < 10) 
            {
                return Response.Create(ResponseStatus.Truncated, new byte[0]);
            }

            // Read the data length.
            int dataLength = (actual[5] << 8) + actual[6];

            // Read and validate the data start address.
            int actualAddress = ((actual[7] << 16) + (actual[8] << 8) + actual[9]);
            if (actualAddress != expectedAddress)
            {
                return Response.Create(ResponseStatus.UnexpectedResponse, new byte[0]);
            }

            byte[] result = new byte[dataLength];

            // Normal block
            if (actual[4] == 1)
            {
                // With normal encoding, data length should be actual length minus header size
                if (actual.Length - 12 < dataLength)
                {
                    return Response.Create(ResponseStatus.Truncated, new byte[0]);
                }

                // Verify block checksum
                UInt16 ValidSum = VpwUtilities.CalcBlockChecksum(actual);
                int PayloadSum = (actual[dataLength + 10] << 8) + actual[dataLength + 11];
                Buffer.BlockCopy(actual, 10, result, 0, dataLength);
                if (PayloadSum != ValidSum)
                {
                    return Response.Create(ResponseStatus.Error, result);
                }

                return Response.Create(ResponseStatus.Success, result);
            }
            // RLE block
            else if (actual[4] == 2)
            {
                // This isnt going to work with existing kernels... need to support variable length.
                byte value = actual[10];
                for (int index = 0; index < dataLength; index++)
                {
                    result[index] = value;
                }

                return Response.Create(ResponseStatus.Error, result);
            }
            else
            {
                return Response.Create(ResponseStatus.Error, result);
            }
        }
    }
}
