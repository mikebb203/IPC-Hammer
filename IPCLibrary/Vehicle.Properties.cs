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
    /// From the application's perspective, this class is the API to the vehicle.
    /// </summary>
    /// <remarks>
    /// Methods in this class are high-level operations like "get the VIN," or "read the contents of the EEPROM."
    /// </remarks>
    public partial class Vehicle : IDisposable
    {
        /// <summary>
        /// Query the PCM's VIN.
        /// </summary>
        public async Task<Response<string>> QueryVin()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            this.device.ClearMessageQueue();

            if (!await this.device.SendMessage(this.protocol.CreateVinRequest1()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 1 failed.");
            }

            Message response1 = await this.device.ReceiveMessage();
            if (response1 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 1.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateVinRequest2()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 2 failed.");
            }

            Message response2 = await this.device.ReceiveMessage();
            if (response2 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 2.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateVinRequest3()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 3 failed.");
            }

            Message response3 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 3.");
            }

            return this.protocol.ParseVinResponses(response1.GetBytes(), response2.GetBytes(), response3.GetBytes());
        }

        /// <summary>
        /// Query the PCM's Serial Number.
        /// </summary>
        public async Task<Response<string>> QuerySerial()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            this.device.ClearMessageQueue();

            if (!await this.device.SendMessage(this.protocol.CreateSerialRequest1()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 1 failed.");
            }

            Message response1 = await this.device.ReceiveMessage();
            if (response1 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 1.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateSerialRequest2()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 2 failed.");
            }

            Message response2 = await this.device.ReceiveMessage();
            if (response2 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 2.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateSerialRequest3()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 3 failed.");
            }

            Message response3 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 3.");
            }

            return this.protocol.ParseSerialResponses(response1, response2, response3);
        }

        /// <summary>
        /// Query the PCM's Broad Cast Code.
        /// </summary>
        public async Task<Response<string>> QueryBCC()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(
                this.protocol.CreateBCCRequest,
                this.protocol.ParseBCCresponse, 
                CancellationToken.None);

            return await query.Execute();
        }

        /// <summary>
        /// Query the PCM's Manufacturer Enable Counter (MEC)
        /// </summary>
        public async Task<Response<string>> QueryMEC()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(
                this.protocol.CreateMECRequest,
                this.protocol.ParseMECresponse,
                CancellationToken.None);

            return await query.Execute();
        }
        /// <summary>
        /// Query the IPC Stepper Calibration.
        /// </summary>

        public async Task<Response<string>> QueryStepper()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            this.device.ClearMessageQueue();

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest1()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 1 failed.");
            }

            Message response1 = await this.device.ReceiveMessage();
            if (response1 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 1.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest2()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 2 failed.");
            }

            Message response2 = await this.device.ReceiveMessage();
            if (response2 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 2.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest3()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 3 failed.");
            }

            Message response3 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 3.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest4()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 4 failed.");
            }

            Message response4 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 4.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest5()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 5 failed.");
            }

            Message response5 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 5.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest6()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 6 failed.");
            }

            Message response6 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 6.");
            }

            if (!await this.device.SendMessage(this.protocol.CreateStepperRequest7()))
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. Request for block 7 failed.");
            }

            Message response7 = await this.device.ReceiveMessage();
            if (response3 == null)
            {
                return Response.Create(ResponseStatus.Timeout, "Unknown. No response to request for block 7.");
            }


            return this.protocol.ParseStepperResponses(response1.GetBytes(), response2.GetBytes(), response3.GetBytes(), response4.GetBytes(), response5.GetBytes(), response6.GetBytes(), response7.GetBytes());
        }
        /// <summary>
        /// Query the IPC's Options
        /// </summary>
        public async Task<Response<string>> QueryIPCoptions()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(
                this.protocol.CreateOptionsRequest,
                this.protocol.ParseOptionsresponse,
                CancellationToken.None);

            return await query.Execute();
        }

        /// <summary>
        /// Query the IPC's Options
        /// </summary>
        public async Task<Response<string>> QueryIPCoptions99()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(
                this.protocol.CreateOptionsRequest99,
                this.protocol.ParseOptionsresponse99,
                CancellationToken.None);

            return await query.Execute();
        }

        /// <summary>
        /// Update the PCM's VIN
        /// </summary>
        /// <remarks>
        /// Requires that the PCM is already unlocked
        /// </remarks>
        public async Task<Response<bool>> UpdateVin(string vin)
        {
            this.device.ClearMessageQueue();

            if (vin.Length != 17) // should never happen, but....
            {
                this.logger.AddUserMessage("VIN " + vin + " is not 17 characters long!");
                return Response.Create(ResponseStatus.Error, false);
            }

            this.logger.AddUserMessage("Changing VIN to " + vin);

            byte[] bvin = Encoding.ASCII.GetBytes(vin);
            byte[] vin1 = new byte[6] { 0x00, bvin[0], bvin[1], bvin[2], bvin[3], bvin[4] };
            byte[] vin2 = new byte[6] { bvin[5], bvin[6], bvin[7], bvin[8], bvin[9], bvin[10] };
            byte[] vin3 = new byte[6] { bvin[11], bvin[12], bvin[13], bvin[14], bvin[15], bvin[16] };

            this.logger.AddUserMessage("Block 1");
            Response<bool> block1 = await WriteBlock(BlockId.Vin1, vin1);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 2");
            Response<bool> block2 = await WriteBlock(BlockId.Vin2, vin2);
            if (block2.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 3");
            Response<bool> block3 = await WriteBlock(BlockId.Vin3, vin3);
            if (block3.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);

            return Response.Create(ResponseStatus.Success, true);
        }

        /// <summary>
        /// Update the IPC Stepper Cals
        /// </summary>
        /// <remarks>
        /// Requires that the IPC is already unlocked
        /// </remarks>
        public async Task<Response<bool>> UpdateStepperCals(string steppercal)
        {
            this.device.ClearMessageQueue();

            if (steppercal.Length != 32) // should never happen, but....
            {
                this.logger.AddUserMessage("Stepper Calibtration " + steppercal + " is not 16 Bytes long!");
                return Response.Create(ResponseStatus.Error, false);
            }

            this.logger.AddUserMessage("Changing Stepper Calibration to " + steppercal);

            ///byte[] bvin = Encoding.Unicode.GetBytes(steppercal);
            var speedometer = steppercal.Substring(0, 6);
            var speedint =  UInt32.Parse(speedometer, System.Globalization.NumberStyles.HexNumber) ;
            byte[] b = BitConverter.GetBytes(speedint);
            var tachometer = steppercal.Substring(6, 6);
            var tachint = UInt32.Parse(tachometer, System.Globalization.NumberStyles.HexNumber);
            byte[] c = BitConverter.GetBytes(tachint);
            var oilstep = steppercal.Substring(24, 4);
            var oilint = UInt16.Parse(oilstep, System.Globalization.NumberStyles.HexNumber);
            byte[] d = BitConverter.GetBytes(oilint);
            var fuelstep = steppercal.Substring(12, 4);
            var fuelint = UInt16.Parse(fuelstep, System.Globalization.NumberStyles.HexNumber);
            byte[] e = BitConverter.GetBytes(fuelint);
            var coolantstep = steppercal.Substring(16, 4);
            var coolantint = UInt16.Parse(coolantstep, System.Globalization.NumberStyles.HexNumber);
            byte[] f = BitConverter.GetBytes(coolantint);
            var voltstep = steppercal.Substring(20, 4);
            var voltint = UInt16.Parse(voltstep, System.Globalization.NumberStyles.HexNumber);
            byte[] g = BitConverter.GetBytes(voltint);
            var transstep = steppercal.Substring(28, 4);
            var transint = UInt16.Parse(transstep, System.Globalization.NumberStyles.HexNumber);
            byte[] h = BitConverter.GetBytes(transint);


            byte[] speed = new byte[3] { b[2], b[1], b[0] };
            byte[] tach = new byte[3] { c[2], c[1], c[0] };
            byte[] oil = new byte[2] { d[1], d[0] };
            byte[] fuel = new byte[2] { e[1], e[0] };
            byte[] coolant = new byte[2] { f[1], f[0] };
            byte[] volt = new byte[2] { g[1], g[0] };
            byte[] trans = new byte[2] { h[1], h[0] };
            this.logger.AddUserMessage("Block 1");
            Response<bool> block1 = await WriteBlock(BlockIdIPC.SpeedoCal, speed);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 2");
            Response<bool> block2 = await WriteBlock(BlockIdIPC.TachCal, tach);
            if (block2.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 3");
            Response<bool> block3 = await WriteBlock(BlockIdIPC.OilCal, oil);
            if (block3.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 4");
            Response<bool> block4 = await WriteBlock(BlockIdIPC.FuelCal, fuel);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 5");
            Response<bool> block5 = await WriteBlock(BlockIdIPC.CoolantTempCal, coolant);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 6");
            Response<bool> block6 = await WriteBlock(BlockIdIPC.VoltCal, volt);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            this.logger.AddUserMessage("Block 7");
            Response<bool> block7 = await WriteBlock(BlockIdIPC.TransTempCal, trans);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
            

            return Response.Create(ResponseStatus.Success, true);
        }

        /// <summary>
        /// Update the IPC options
        /// </summary>
        /// <remarks>
        /// Requires that the IPC is already unlocked
        /// </remarks>
        public async Task<Response<bool>> UpdateOptions(string options)
        {
            this.device.ClearMessageQueue();

            if (options.Length != 4) // should never happen, but....
            {
                this.logger.AddUserMessage("Options " + options + " is not 2 Bytes long!");
                return Response.Create(ResponseStatus.Error, false);
            }

            this.logger.AddUserMessage("Changing Options to " + options);

            ///byte[] boptions = Encoding.ASCII.GetBytes(options);
            var optionint = UInt32.Parse(options, System.Globalization.NumberStyles.HexNumber);
            byte[] b = BitConverter.GetBytes(optionint);

            byte[] boptions = new byte[2] { b[1], b[0] };

            this.logger.AddUserMessage("Options");
            Response<bool> block1 = await WriteBlock(BlockIdIPC.Options, boptions);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);


            return Response.Create(ResponseStatus.Success, true);
        }

        /// <summary>
        /// Update the IPC options 99-02
        /// </summary>
        /// <remarks>
        /// Requires that the IPC is already unlocked
        /// </remarks>
        public async Task<Response<bool>> UpdateOptions99(string options)
        {
            this.device.ClearMessageQueue();

            if (options.Length != 4) // should never happen, but....
            {
                this.logger.AddUserMessage("Options " + options + " is not 2 Bytes long!");
                return Response.Create(ResponseStatus.Error, false);
            }

            this.logger.AddUserMessage("Changing Options to " + options);

            ///byte[] boptions = Encoding.ASCII.GetBytes(options);
            var optionint = UInt32.Parse(options, System.Globalization.NumberStyles.HexNumber);
            byte[] b = BitConverter.GetBytes(optionint);

            byte[] boptions = new byte[2] { b[1], b[0] };

            this.logger.AddUserMessage("Options");
            Response<bool> block1 = await WriteBlock(BlockIdIPC.Options99, boptions);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);


            return Response.Create(ResponseStatus.Success, true);
        }

        /// <summary>
        /// Update the PCM's VIN
        /// </summary>
        /// <remarks>
        /// Requires that the PCM is already unlocked
        /// </remarks>
        public async Task<Response<bool>> UpdateMileage(string mileage)
        {
            this.device.ClearMessageQueue();

            if (mileage.Length >= 8) // should never happen, but....
            {
                this.logger.AddUserMessage("Mileage" + mileage + " is not less than 8 characters long!");
                return Response.Create(ResponseStatus.Error, false);
            }

            this.logger.AddUserMessage("Changing Mileage to " + mileage);

            uint intmileage = uint.Parse(mileage, System.Globalization.NumberStyles.Integer);
            //byte[] bmileage = Encoding.ASCII.GetBytes(mileage);
            byte[] bmileage = BitConverter.GetBytes(intmileage);
            byte[] miles = new byte[5] { 0x45, bmileage[3], bmileage[2], bmileage[1], bmileage[0] };

            
            Response<bool> block1 = await WriteBlock(0x99, miles);
            if (block1.Status != ResponseStatus.Success) return Response.Create(ResponseStatus.Error, false);
           
            return Response.Create(ResponseStatus.Success, true);

        }

        /// <summary>
        /// Query the PCM's operating system ID.
        /// </summary>
        /// <returns></returns>
        public async Task<Response<UInt32>> QueryOperatingSystemId(CancellationToken cancellationToken)
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);
            return await this.QueryUnsignedValue(this.protocol.CreateOperatingSystemIdReadRequest, cancellationToken);
        }

        /// <summary>
        /// Query the PCM's Hardware ID.
        /// </summary>
        /// <remarks>
        /// Note that this is a software variable and my not match the hardware at all of the software runs.
        /// </remarks>
        public async Task<Response<UInt32>> QueryHardwareId()
        {
            return await this.QueryUnsignedValue(this.protocol.CreateHardwareIdReadRequest, CancellationToken.None);
        }

        /// <summary>
        /// Query the PCM's calibration ID.
        /// </summary>
        public async Task<Response<UInt32>> QueryCalibrationId()
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(
                this.protocol.CreateCalibrationIdReadRequest,
                this.protocol.ParseUInt32FromBlockReadResponse,
                CancellationToken.None);
            return await query.Execute();
        }

        /// <summary>
        /// Helper function for queries that return unsigned 32-bit integers.
        /// </summary>
        private async Task<Response<UInt32>> QueryUnsignedValue(Func<Message> generator, CancellationToken cancellationToken)
        {
            await this.device.SetTimeout(TimeoutScenario.ReadProperty);

            var query = this.CreateQuery(generator, this.protocol.ParseUInt32FromBlockReadResponse, cancellationToken);
            return await query.Execute();
        }
      
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SweepGauges()
        {
            ///return await this.QueryUnsignedValue(this.protocol.CreateSweepGaugesrequest, CancellationToken.None);
            Message message = this.protocol.CreateSweepGaugesrequest();
            await this.device.SendMessage(message);          
        }

        public async Task LEDson()
        {
            ///return await this.QueryUnsignedValue(this.protocol.CreateLEDsonrequest, CancellationToken.None);
            Message message = this.protocol.CreateLEDsonrequest();
            await this.device.SendMessage(message);
        }

        public async Task Displayon()
        {
            ///return await this.QueryUnsignedValue(this.protocol.CreateDisplaysonrequest, CancellationToken.None);
            Message message = this.protocol.CreateDisplaysonrequest();
            await this.device.SendMessage(message);
        }

        public async Task Lights99()
        {
            ///return await this.QueryUnsignedValue(this.protocol.CreateDisplaysonrequest, CancellationToken.None);
            Message message = this.protocol.CreateLights99request();
            await this.device.SendMessage(message);
            Message message1 = this.protocol.CreateLights992request();
            await this.device.SendMessage(message1);
        }

        public async Task PRNDL99()
        {
            Message message = this.protocol.CreatePRNDL99request();
            await this.device.SendMessage(message);
        }

        public async Task SweepGauges9900()
        {
            Message message = this.protocol.CreateSweepGauges9900();
            await this.device.SendMessage(message);
        }

        public async Task SweepGauges9950()
        {
            Message message = this.protocol.CreateSweepGauges9950();
            await this.device.SendMessage(message);
        }

        public async Task SweepGauges99100()
        {
            Message message = this.protocol.CreateSweepGauges99100();
            await this.device.SendMessage(message);
        }

    }
}
