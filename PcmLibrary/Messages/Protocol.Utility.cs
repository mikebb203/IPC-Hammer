using System;
using System.Collections.Generic;
using System.Text;

namespace PcmHacking
{
    public partial class Protocol
    {
        /// <summary>
        /// Parse a one-byte payload
        /// </summary>
        internal Response<byte> ParseByte(Message responseMessage, byte mode, byte submode)
        {
            ResponseStatus status;
            byte[] expected = { 0x6C, DeviceId.Tool, DeviceId.Pcm, (byte)(mode | 0x40), submode };
            if (!TryVerifyInitialBytes(responseMessage, expected, out status))
            {
                byte[] refused = { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7F, mode, submode };
                if (TryVerifyInitialBytes(responseMessage, refused, out status))
                {
                    return Response.Create(ResponseStatus.Refused, (byte)0);
                }

                return Response.Create(status, (byte)0);
            }

            byte[] responseBytes = responseMessage.GetBytes();
            if (responseBytes.Length < 6)
            {
                return Response.Create(ResponseStatus.Truncated, (byte)0);
            }

            int value = responseBytes[5];

            return Response.Create(ResponseStatus.Success, (byte)value);
        }

        /// <summary>
        /// Turn four bytes of payload into a UInt32.
        /// </summary>
        internal Response<UInt32> ParseUInt32(Message responseMessage, byte mode, byte submode)
        {
            ResponseStatus status;
            byte[] expected = { 0x6C, DeviceId.Tool, DeviceId.Pcm, (byte)(mode | 0x40), submode };
            if (!TryVerifyInitialBytes(responseMessage, expected, out status))
            {
                byte[] refused = { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7F, mode, submode };
                if (TryVerifyInitialBytes(responseMessage, refused, out status))
                {
                    return Response.Create(ResponseStatus.Refused, (UInt32)0);
                }

                return Response.Create(status, (UInt32)0);
            }

            byte[] responseBytes = responseMessage.GetBytes();

            if (responseBytes.Length < 9)
            {
                return Response.Create(ResponseStatus.Truncated, (UInt32)0);
            }

            int value =
                (responseBytes[5] << 24) |
                (responseBytes[6] << 16) |
                (responseBytes[7] << 8) |
                responseBytes[8];

            return Response.Create(ResponseStatus.Success, (UInt32)value);
        }

        /// <summary>
        /// Check for an accept/reject message with the given mode byte.
        /// </summary>
        /// <remarks>
        /// TODO: Make this private, use public methods that are tied to a specific message type.
        /// </remarks>
        public Response<bool> DoSimpleValidation(Message message, byte priority, byte mode, params byte[] data)
        {
            byte[] actual = message.GetBytes();
            ResponseStatus status;

            byte[] success = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, (byte)(mode + 0x40), };
            if (this.TryVerifyInitialBytes(actual, success, out status))
            {
                if (data != null && data.Length > 0)
                {
                    for (int index = 0; index < data.Length; index++)
                    {
                        const int headBytes = 4;
                        int actualLength = actual.Length;
                        int expectedLength = data.Length + headBytes;
                        if (actualLength >= expectedLength)
                        {
                            if (actual[headBytes + index] == data[index])
                            {
                                continue;
                            }
                            else
                            {
                                return Response.Create(ResponseStatus.UnexpectedResponse, false);
                            }
                        }
                        else
                        {
                            return Response.Create(ResponseStatus.Truncated, false);
                        }
                    }
                }

                return Response.Create(ResponseStatus.Success, true);
            }

            byte[] failure = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, 0x7F, mode };
            if (this.TryVerifyInitialBytes(actual, failure, out status))
            {
                return Response.Create(ResponseStatus.Refused, false);
            }

            return Response.Create(ResponseStatus.UnexpectedResponse, false);
        }

        /// <summary>
        /// Check for an accept/reject message with the given mode byte.
        /// </summary>
        /// <remarks>
        /// TODO: Make this private, use public methods that are tied to a specific message type.
        /// </remarks>
        public Response<bool> DoSimpleValidation2(Message message, byte priority, byte mode, byte first, byte more, byte done, params byte[] data)
        {
            byte[] actual = message.GetBytes();
            
            ResponseStatus status;

            byte[] success = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, (byte)(mode + 0x40), first, more };
            byte[] success1 = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, (byte)(mode + 0x40), first, done };
            if (this.TryVerifyInitialBytes1(actual, success, success1, out status))
            {
                if (data != null && data.Length > 0)
                {
                    for (int index = 0; index < data.Length; index++)
                    {
                        const int headBytes = 4;
                        int actualLength = actual.Length;
                        int expectedLength = data.Length + headBytes;
                        if (actualLength >= expectedLength)
                        {
                            if (actual[headBytes + index] == data[index])
                            {
                                continue;
                            }
                            else
                            {
                                return Response.Create(ResponseStatus.UnexpectedResponse, false);
                            }
                        }
                        else 
                        {
                            return Response.Create(ResponseStatus.Truncated, false);
                        }
                    }
                }

                return Response.Create(ResponseStatus.Success, true);
            }

            byte[] failure = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, 0x7F, mode };
            if (this.TryVerifyInitialBytes(actual, failure, out status))
            {
                return Response.Create(ResponseStatus.Refused, false);
            }

            return Response.Create(ResponseStatus.UnexpectedResponse, false);
        }

        public Response<bool> DoSimpleValidation3(Message message, byte priority, byte mode, byte first, byte wait, params byte[] data)
        {
            byte[] actual = message.GetBytes();

            ResponseStatus status;

            byte[] success = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, (byte)(mode + 0x40), first, wait };
            if (this.TryVerifyInitialBytes(actual, success, out status))
            {
                if (data != null && data.Length > 0)
                {
                    for (int index = 0; index < data.Length; index++)
                    {
                        const int headBytes = 4;
                        int actualLength = actual.Length;
                        int expectedLength = data.Length + headBytes;
                        if (actualLength >= expectedLength)
                        {
                            if (actual[headBytes + index] == data[index])
                            {
                                continue;
                            }
                            else
                            {
                                return Response.Create(ResponseStatus.UnexpectedResponse, false);
                            }
                        }
                        else
                        {
                            return Response.Create(ResponseStatus.Truncated, false);
                        }
                    }
                }

                return Response.Create(ResponseStatus.Success, true);
            }

            byte[] failure = new byte[] { priority, DeviceId.Tool, DeviceId.Pcm, 0x7F, mode };
            if (this.TryVerifyInitialBytes(actual, failure, out status))
            {
                return Response.Create(ResponseStatus.Refused, false);
            }

            return Response.Create(ResponseStatus.UnexpectedResponse, false);
        }


        /// <summary>
        /// Confirm that the first portion of the 'actual' array of bytes matches the 'expected' array of bytes.
        /// </summary>
        private bool TryVerifyInitialBytes(Message actual, byte[] expected, out ResponseStatus status)
        {
            return TryVerifyInitialBytes(actual.GetBytes(), expected, out status);
        }

        /// <summary>
        /// Confirm that the first portion of the 'actual' array of bytes matches the 'expected' array of bytes.
        /// </summary>
        private bool TryVerifyInitialBytes(byte[] actual, byte[] expected, out ResponseStatus status)
        {
            if (actual.Length < expected.Length)
            {
                // This is how we indicate that the response is too short.
                status = ResponseStatus.Truncated;
                return false;
            }

            for (int index = 0; index < expected.Length; index++)
            {
                if (actual[index] != expected[index])
                {
                    // This is how we indicate that the response contained garbage.
                    status = ResponseStatus.UnexpectedResponse;
                    return false;
                }
            }

            status = ResponseStatus.Success;
            return true;
        }

        private bool TryVerifyInitialBytes1(byte[] actual, byte[] expected, byte[] expected1, out ResponseStatus status)
        {
            if (actual.Length < expected.Length)
            {
                // This is how we indicate that the response is too short.
                status = ResponseStatus.Truncated;
                return false;
            }

            if (actual.Length < expected1.Length)
            {
                status = ResponseStatus.Truncated;
                return false;
            }
           
            
            if (actual[5] == 0x86)
            {
                for (int index = 0; index < expected1.Length; index++)
                {
                    if (actual[index] != expected1[index])
                    {
                        // This is how we indicate that the response contained garbage.
                        status = ResponseStatus.UnexpectedResponse;
                        return false;
                    }
                }
                status = ResponseStatus.Success;
                return true;
            }

            if (actual[5] == 0x73)
            {
                for (int index = 0; index < expected.Length; index++)
                {
                    if (actual[index] != expected[index])
                    {
                        status = ResponseStatus.UnexpectedResponse;
                        return false;

                    }
                }
                status = ResponseStatus.Success;
                return true;

            }
            status = ResponseStatus.UnexpectedResponse;
            return false;
            
        }
    }
}
