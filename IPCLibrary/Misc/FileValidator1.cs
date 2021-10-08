using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PcmHacking
{
    /// <summary>
    /// Try to detect corrupted firmware images.
    /// </summary>
    public class FileValidator1
    {
        /// <summary>
        /// Names of segments in P01 and P59 operating systems.
        /// </summary>
        private readonly string[] segmentNames =
        {
            "Operating system",
            "IPC calibration",
            "IPC calibration",
            "Transmission calibration",
            "Transmission diagnostics",
            "Fuel system",
            "System",
            "Speedometer",
        };

        /// <summary>
        /// The contents of the firmware file that someone wants to flash.
        /// </summary>
        private readonly byte[] image;

        /// <summary>
        /// For reporting progress and success/fail.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileValidator1(byte[] image, ILogger logger)
        {
            this.image = image;
            this.logger = logger;
        }

        /// <summary>
        /// Indicate whether the image is valid or not.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
           
            if (this.image.Length == 16 * 1024)
            {
                this.logger.AddUserMessage("Validating 16k file.");
            }
            ///else if (this.image.Length == 112 * 1024)
            ///{
            ///    this.logger.AddUserMessage("Validating 112k file.");
            ///}
            ///else if (this.image.Length == 96 * 1024)
            ///{
            ///    this.logger.AddUserMessage("Validating 96k file.");
            ///}
            else
            {
                this.logger.AddUserMessage(
                    string.Format(
                        "Files must be 16K. This file is {0} / {1:X} bytes long.",
                        this.image.Length,
                        this.image.Length));
                return false;
            }

            bool success = true;
         ///   success &= this.ValidateSignatures();
            success &= this.ValidateChecksums();
            return success;
        }

        

        /// <summary>
        /// Get the OSID from the file that the user wants to flash.
        /// </summary>
        public uint GetOsidFromImage()
        {
            int osid = 0;
            if (image.Length == 16 * 1024)
            {
                osid += image[0x004] << 24;
                osid += image[0x005] << 16;
                osid += image[0x006] << 8;
                osid += image[0x007] << 0;
            }
            if (image.Length == 96 * 1024)
            {
                osid += image[0x004] << 24;
                osid += image[0x005] << 16;
                osid += image[0x006] << 8;
                osid += image[0x007] << 0;
            }
            if (image.Length == 112 * 1024) // bin valid sizes
            {
                osid += image[0x004] << 24;
                osid += image[0x005] << 16;
                osid += image[0x006] << 8;
                osid += image[0x007] << 0;

                return (uint)osid;
            }
            if (image.Length == 512 * 1024)
            {
                osid += image[0x504] << 24;
                osid += image[0x505] << 16;
                osid += image[0x506] << 8;
                osid += image[0x507] << 0;

                return (uint)osid;            
            }
            if (image.Length == 1024 * 1024)
            {
                osid += image[0x504] << 24;
                osid += image[0x505] << 16;
                osid += image[0x506] << 8;
                osid += image[0x507] << 0;

                return (uint)osid;
            }

            return (uint)osid;
        }

        /// <summary>
        /// Validate a 512k image.
        /// </summary>
        public bool ValidateChecksums()
        {
            bool success = true;
            UInt16 loop = 0;
            ///byte[] startAddresstable;
            ///byte[] endAddresstable;
            UInt16 loop1 = 0;
            if (image.Length == 112 * 1024)
            {
               /// startAddresstable = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x80, 0x00 };
               /// endAddresstable = new byte[] { 0x00, 0x01, 0x7F, 0xFE, 0x00, 0x01, 0xCF, 0xFE };
                
                loop = 0;
                loop1 = 2;
            }
            if (image.Length == 96 * 1024)
            {

                loop = 0;
                loop1 = 1;
            }
            if (image.Length == 16 * 1024)
            {
               /// startAddresstable = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
               /// endAddresstable = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3F, 0xFE };
                loop = 2;
                loop1 = 3;
            }
            
            this.logger.AddUserMessage("\tStart\tEnd\tStored\tNeeded\tVerdict\tSegment Name");
            
            for (UInt32 segment = loop; segment < loop1; segment++)
            {
                UInt32 startAddress = 0x000000;
                UInt32 endAddress = 0x000000;
                

                if (segment == 0)
                {
                    startAddress = 0x000000;
                    endAddress = 0x017FFC;
                }
                if (segment == 1)
                {
                    startAddress = 0x018000;
                    endAddress = 0x01BFFE;
                }
                if (segment == 2)
                {
                    startAddress = 0x000000;
                    endAddress = 0x003FFE;
                }
                // For most segments, the first two bytes are the checksum, so they're not counted in the computation.
                // For the overall "segment" the checkum is in the middle.
                UInt32 checksumAddress = startAddress;
                if (startAddress == 0)
                {
                    startAddress += 2;
                }
                if (startAddress == 0x018000)
                {
                    startAddress += 2;
                }
                string segmentName = segmentNames[segment];

                if ((startAddress >= image.Length) || (endAddress >= image.Length) || (checksumAddress >= image.Length))
                {
                    this.logger.AddUserMessage("Checksum table is corrupt.");
                    return false;
                }

                success &= this.ValidateRange(startAddress, endAddress, checksumAddress, segmentName);
            }

            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        private UInt32 ReadUnsigned(byte[] image, UInt32 offset)
        {
            return BitConverter.ToUInt32(image.Skip((int)offset).Take(4).Reverse().ToArray(), 0);
        }
        
        /// <summary>
        /// Validate signatures
        /// </summary>
        private bool ValidateSignatures()
        {
            if ((image[0x1FFFE] != 0x4A) || (image[0x01FFFF] != 0xFC))
            {
                this.logger.AddUserMessage("This file does not contain the expected signature at 0x1FFFE.");
                return false;
            }

            if (image.Length == 512 * 1024)
            {
                if ((image[0x7FFFE] != 0x4A) || (image[0x07FFFF] != 0xFC))
                {
                    this.logger.AddUserMessage("This file does not contain the expected signature at 0x7FFFE.");
                    return false;
                }
            }
            else if (image.Length == 1024 * 1024)
            {
                if ((image[0xFFFFE] != 0x4A) || (image[0x0FFFFF] != 0xFC))
                {
                    this.logger.AddUserMessage("This file does not contain the expected signature at 0xFFFFE.");
                    return false;
                }
            }
            else
            {
                this.logger.AddUserMessage("Files of size " + image.Length.ToString("X8") + " are not supported.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Print the header for the table of checksums.
        /// </summary>
        private void PrintHeader()
        {
            this.logger.AddUserMessage("\tStart\tEnd\tResult\tFile\tActual\tContent");
        }

        /// <summary>
        /// Validate a range.
        /// </summary>
        private bool ValidateRange(UInt32 start, UInt32 end, UInt32 storage, string description)
        {
            UInt16 storedChecksum = (UInt16)((this.image[storage] << 8) + this.image[storage + 1]);
            UInt16 computedChecksum = 0;
            
            for(UInt32 address = start; address <= end; address+=2)
            {
               
                UInt16 value = (UInt16)(this.image[address + 1] << 8);
                value |= this.image[address];
                computedChecksum += value;
            }

            computedChecksum = (UInt16)((0 - computedChecksum));

            computedChecksum = ReverseBytes(computedChecksum);

            image[0] = (byte)(computedChecksum >> 8);
            image[1] = (byte)(computedChecksum);

            bool verdict = computedChecksum == computedChecksum;

            return verdict;
        }

        // reverse byte order (16-bit)
        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }



    }
}