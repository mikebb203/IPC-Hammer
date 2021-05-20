using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcmHacking
{
    public class BlockId
    {
        public const byte Vin1               = 0x01; // 5 bytes of VIN
        public const byte Vin2               = 0x02; // 6 bytes of VIN
        public const byte Vin3               = 0x03; // 6 bytes of VIN
        public const byte HardwareID         = 0x08; // Hardware ID
        public const byte Serial1            = 0x05; // 4 bytes of Serial
        public const byte Serial2            = 0x06; // 4 bytes of Serial
        public const byte Serial3            = 0x07; // 4 bytes of Serial
        public const byte CalibrationID      = 0x0B; // Calibration ID
        public const byte OperatingSystemID  = 0x0A; // Operating System ID aka OSID
        public const byte EngineCalID        = 0xFF; // Engine Segment Calibration ID
        public const byte EngineDiagCalID    = 0x0C; // Engine Diagnostic Calibration ID
        public const byte TransCalID         = 0x0D; // Transmission Segment Calibration ID
        public const byte TransDiagID        = 0x0E; // Transmission Diagnostic Calibration ID
        public const byte FuelCalID          = 0x0F; // Fuel Segment Calibration ID
        public const byte SystemCalID        = 0x10; // System Segment Calibration ID
        public const byte SpeedCalID         = 0x11; // Speed Calibration ID
        public const byte BCC                = 0x14; // Broad Cast Code
        public const byte OilLifePerc        = 0x6D; // Oil Life Remaining Percent
        public const byte OperatingSystemLvl = 0x93; // Operating System Level
        public const byte EngineCalLvl       = 0x94; // Engine Segment Calibration Level
        public const byte EngineDiagCalLvl   = 0x95; // Engine Diagnostic Calibration Level
        public const byte TransCalLvl        = 0x96; // Transmission Segment Calibration Level
        public const byte TransDiagLvl       = 0x97; // Transmission Diagnostic Calibration Level
        public const byte BootSector         = 0x98; // Fuel Segment Calibration Level
        public const byte SystemCalLvl       = 0x99; // System Segment Calibration Level
        public const byte SpeedCalLvl        = 0x9A; // Speed Calibration Level
        public const byte MEC                = 0xA0; // Manufacturers Enable Counter  
    }

    public class BlockIdIPC
    {
        public const byte SpeedoCal          = 0x90;
        public const byte TachCal            = 0x91;
        public const byte FuelCal            = 0x92;
        public const byte CoolantTempCal     = 0x93;
        public const byte VoltCal            = 0x94;
        public const byte OilCal             = 0x95;
        public const byte TransTempCal       = 0x97;
        public const byte Options            = 0x9A;    
    
    
    }


}
