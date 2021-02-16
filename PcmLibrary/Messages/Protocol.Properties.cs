using System;
using System.Collections.Generic;
using System.Text;

namespace PcmHacking
{
    public partial class Protocol
    {
        /// <summary>
        /// Create a request to read the given block of PCM memory.
        /// </summary>
        public Message CreateReadRequest(byte Block)
        {
            byte[] Bytes = new byte[] { Priority.Physical0, DeviceId.Pcm, DeviceId.Tool, Mode.ReadBlock, Block };
            return new Message(Bytes);
        }

        /// <summary>
        /// Create a request to read the PCM's operating system ID.
        /// </summary>
        /// <returns></returns>
        public Message CreateOperatingSystemIdReadRequest()
        {
            return CreateReadRequest(BlockId.OperatingSystemID);
        }

        /// <summary>
        /// Create a request to read the PCM's Calibration ID.
        /// </summary>
        /// <returns></returns>
        public Message CreateCalibrationIdReadRequest()
        {
            return CreateReadRequest(BlockId.CalibrationID);
        }

        /// <summary>
        /// Create a request to read the PCM's Hardware ID.
        /// </summary>
        /// <returns></returns>
        public Message CreateHardwareIdReadRequest()
        {
            return CreateReadRequest(BlockId.HardwareID);
        }

        /// <summary>
        /// Parse a 32-bit value from the first four bytes of a message payload.
        /// </summary>
        public Response<UInt32> ParseUInt32(Message message, byte responseMode)
        {
            byte[] bytes = message.GetBytes();
            int result = 0;
            ResponseStatus status;

            byte[] expected = new byte[] { Priority.Physical0, DeviceId.Tool, DeviceId.Pcm, responseMode };
            if (!TryVerifyInitialBytes(bytes, expected, out status))
            {
                return Response.Create(ResponseStatus.Error, (UInt32)result);
            }
            if (bytes.Length < 9)
            {
                return Response.Create(ResponseStatus.Truncated, (UInt32)result);
            }

            result = bytes[5] << 24;
            result += bytes[6] << 16;
            result += bytes[7] << 8;
            result += bytes[8];

            return Response.Create(ResponseStatus.Success, (UInt32)result);
        }

        /// <summary>
        /// Parse the response to a block-read request.
        /// </summary>
        public Response<UInt32> ParseUInt32FromBlockReadResponse(Message message)
        {
            return ParseUInt32(message, Mode.ReadBlock + Mode.Response);
        }

        #region VIN

        /// <summary>
        /// Create a request to read the first segment of the PCM's VIN.
        /// </summary>
        public Message CreateVinRequest1()
        {
            return CreateReadRequest(BlockId.Vin1);
        }

        /// <summary>
        /// Create a request to read the second segment of the PCM's VIN.
        /// </summary>
        public Message CreateVinRequest2()
        {
            return CreateReadRequest(BlockId.Vin2);
        }

        /// <summary>
        /// Create a request to read the thid segment of the PCM's VIN.
        /// </summary>
        public Message CreateVinRequest3()
        {
            return CreateReadRequest(BlockId.Vin3);
        }

        /// <summary>
        /// Parse the responses to the three requests for VIN information.
        /// </summary>
        public Response<string> ParseVinResponses(byte[] response1, byte[] response2, byte[] response3)
        {
            string result = "Unknown";
            ResponseStatus status;

            byte[] expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockId.Vin1 };
            if (!TryVerifyInitialBytes(response1, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockId.Vin2 };
            if (!TryVerifyInitialBytes(response2, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockId.Vin3 };
            if (!TryVerifyInitialBytes(response3, expected, out status))
            {
                return Response.Create(status, result);
            }

            byte[] vinBytes = new byte[17];
            Buffer.BlockCopy(response1, 6, vinBytes, 0, 5);
            Buffer.BlockCopy(response2, 5, vinBytes, 5, 6);
            Buffer.BlockCopy(response3, 5, vinBytes, 11, 6);
            string vin = System.Text.Encoding.ASCII.GetString(vinBytes);
            return Response.Create(ResponseStatus.Success, vin);
        }

        #endregion

        #region Serial

        /// <summary>
        /// Create a request to read the first segment of the PCM's Serial Number.
        /// </summary>
        public Message CreateSerialRequest1()
        {
            return CreateReadRequest(BlockId.Serial1);
        }

        /// <summary>
        /// Create a request to read the second segment of the PCM's Serial Number.
        /// </summary>
        public Message CreateSerialRequest2()
        {
            return CreateReadRequest(BlockId.Serial2);
        }

        /// <summary>
        /// Create a request to read the thid segment of the PCM's Serial Number.
        /// </summary>
        public Message CreateSerialRequest3()
        {
            return CreateReadRequest(BlockId.Serial3);
        }

        /// <summary>
        /// Parse the responses to the three requests for Serial Number information.
        /// </summary>
        public Response<string> ParseSerialResponses(Message response1, Message response2, Message response3)
        {
            string result = "Unknown";
            ResponseStatus status;

            byte[] expected = new byte[] { Priority.Physical0, DeviceId.Tool, DeviceId.Pcm, Mode.ReadBlock + Mode.Response, BlockId.Serial1 };
            if (!TryVerifyInitialBytes(response1, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { Priority.Physical0, DeviceId.Tool, DeviceId.Pcm, Mode.ReadBlock + Mode.Response, BlockId.Serial2 };
            if (!TryVerifyInitialBytes(response2, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { Priority.Physical0, DeviceId.Tool, DeviceId.Pcm, Mode.ReadBlock + Mode.Response, BlockId.Serial3 };
            if (!TryVerifyInitialBytes(response3, expected, out status))
            {
                return Response.Create(status, result);
            }

            byte[] serialBytes = new byte[12];
            Buffer.BlockCopy(response1.GetBytes(), 5, serialBytes, 0, 4);
            Buffer.BlockCopy(response2.GetBytes(), 5, serialBytes, 4, 4);
            Buffer.BlockCopy(response3.GetBytes(), 5, serialBytes, 8, 4);

            byte[] printableBytes = Utility.GetPrintable(serialBytes);
            string serial = System.Text.Encoding.ASCII.GetString(printableBytes);

            return Response.Create(ResponseStatus.Success, serial);
        }

        #endregion

        #region BCC

        /// <summary>
        /// Create a request to read the Broad Cast Code (BCC).
        /// </summary>
        public Message CreateBCCRequest()
        {
            return CreateReadRequest(BlockId.BCC);
        }

        public Response<string> ParseBCCresponse(Message responseMessage)
        {
            string result = "Unknown";
            ResponseStatus status;
            byte[] response = responseMessage.GetBytes();

            byte[] expected = new byte[] { Priority.Physical0, DeviceId.Tool, DeviceId.Pcm, Mode.ReadBlock + Mode.Response, BlockId.BCC };
            if (!TryVerifyInitialBytes(response, expected, out status))
            {
                return Response.Create(status, result);
            }

            byte[] BCCBytes = new byte[4];
            Buffer.BlockCopy(response, 5, BCCBytes, 0, 4);

            byte[] printableBytes = Utility.GetPrintable(BCCBytes);
            string BCC = System.Text.Encoding.ASCII.GetString(printableBytes);

            return Response.Create(ResponseStatus.Success, BCC);
        }

        #endregion

        #region MEC

        /// <summary>
        /// Create a request to read the Broad Cast Code (MEC).
        /// </summary>
        public Message CreateMECRequest()
        {
            return CreateReadRequest(BlockId.MEC);
        }

        public Response<string> ParseMECresponse(Message responseMessage)
        {
            string result = "Unknown";
            ResponseStatus status;
            byte[] response = responseMessage.GetBytes();

            byte[] expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockId.MEC };
            if (!TryVerifyInitialBytes(response, expected, out status))
            {
                return Response.Create(status, result);
            }

            string MEC = response[5].ToString();

            return Response.Create(ResponseStatus.Success, MEC);
        }

        #endregion

        #region IPC
        /// <summary>
        /// Create a request Device control.
        /// </summary>
        public Message CreateDevicecontrolrequest(byte Block1, byte Block2, byte Block3, byte Block4, byte Block5, byte Block6, byte Block7)
        {
            byte[] Bytes = new byte[] { Priority.Physical0, DeviceId.Pcm, DeviceId.Tool, Mode.Devicecontrol, Block1, Block2, Block3, Block4, Block5, Block6, Block7 };
            return new Message(Bytes);
        }

        public Message CreateSweepGaugesrequest()
        {
            return CreateDevicecontrolrequest(0x20, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00);
        }


        public Message CreateLEDsonrequest()
        {
            return CreateDevicecontrolrequest(0x11, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00);
        }

        public Message CreateDisplaysonrequest()
        {
            return CreateDevicecontrolrequest(0x21, 0x88, 0x88, 0x00, 0x00, 0x00, 0x00);
        }


        public Message CreatestartingProgramrequest()
        {
            return CreateDevicecontrolrequest(0x18, 0x00, 0x00, 0x02, 0x02, 0x00, 0x00);
        }


        public Message Createmode34request(byte Block1, byte Block2, byte Block3, byte Block4, byte Block5, byte Block6)
        {
            byte[] Bytes = new byte[] { Priority.Physical0, DeviceId.Pcm, DeviceId.Tool, Mode.PCMUploadRequest, Block1, Block2, Block3, Block4, Block5, Block6};
            return new Message(Bytes);
        }
        public Message Createmode34request()
        {
            return Createmode34request(0x00, 0x04, 0x00, 0x00, 0xE0, 0x00);
        }

        /// <summary>
        /// Create a request to read the Speedo Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest1()
        {
            return CreateReadRequest(BlockIdIPC.SpeedoCal);
        }

        /// <summary>
        /// Create a request to read the Tachometer Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest2()
        {
            return CreateReadRequest(BlockIdIPC.TachCal);
        }

        /// <summary>
        /// Create a request to read the Fuel Gauge Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest3()
        {
            return CreateReadRequest(BlockIdIPC.FuelCal);
        }

        /// <summary>
        /// Create a request to read the Coolant Temp Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest4()
        {
            return CreateReadRequest(BlockIdIPC.CoolantTempCal);
        }

        /// <summary>
        /// Create a request to read the Voltage gauge Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest5()
        {
            return CreateReadRequest(BlockIdIPC.VoltCal);
        }

        /// <summary>
        /// Create a request to read the Oil Pressure Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest6()
        {
            return CreateReadRequest(BlockIdIPC.OilCal);
        }

        /// <summary>
        /// Create a request to read the Trans Temp Stepper Calibration.
        /// </summary>
        public Message CreateStepperRequest7()
        {
            return CreateReadRequest(BlockIdIPC.TransTempCal);
        }

        /// <summary>
        /// Parse the responses to the three requests for VIN information.
        /// </summary>
        public Response<string> ParseStepperResponses(byte[] response1, byte[] response2, byte[] response3, byte[] response4, byte[] response5, byte[] response6, byte[] response7)
        {
            string result = "Unknown";
            ResponseStatus status;

            byte[] expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.SpeedoCal };
            if (!TryVerifyInitialBytes(response1, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.TachCal };
            if (!TryVerifyInitialBytes(response2, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.FuelCal };
            if (!TryVerifyInitialBytes(response3, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.CoolantTempCal };
            if (!TryVerifyInitialBytes(response4, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.VoltCal };
            if (!TryVerifyInitialBytes(response5, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.OilCal };
            if (!TryVerifyInitialBytes(response6, expected, out status))
            {
                return Response.Create(status, result);
            }

            expected = new byte[] { 0x6C, DeviceId.Tool, DeviceId.Pcm, 0x7C, BlockIdIPC.TransTempCal };
            if (!TryVerifyInitialBytes(response7, expected, out status))
            {
                return Response.Create(status, result);
            }


            byte[] vinBytes = new byte[16];
            Buffer.BlockCopy(response1, 5, vinBytes, 0, 3);
            Buffer.BlockCopy(response2, 5, vinBytes, 3, 3);
            Buffer.BlockCopy(response3, 5, vinBytes, 6, 2);
            Buffer.BlockCopy(response4, 5, vinBytes, 8, 2);
            Buffer.BlockCopy(response5, 5, vinBytes, 10, 2);
            Buffer.BlockCopy(response6, 5, vinBytes, 12, 2);
            Buffer.BlockCopy(response7, 5, vinBytes, 14, 2);
            string vin = System.Text.Encoding.ASCII.GetString(vinBytes);
            return Response.Create(ResponseStatus.Success, vin);
            
        }
        #endregion

    }




}

