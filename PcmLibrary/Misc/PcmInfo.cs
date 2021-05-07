using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcmHacking
{
    /// <summary>
    /// This combines various metadata about whatever PCM we've connected to.
    /// </summary>
    /// <remarks>
    /// This was ported from the LS1 flashing utility originally posted at pcmhacking.net.
    /// </remarks>
    public class PcmInfo
    {
        /// <summary>
        /// Operating system ID.
        /// </summary>
        public uint HardwareID { get; private set; }

        /// <summary>
        /// Descriptive text.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Indicates whether this PCM is supported by the app.
        /// </summary>
        public bool IsSupported { get; private set; }

        /// <summary>
        /// Base address to begin writing the kernel to.
        /// </summary>
        public int KernelBaseAddress { get; private set; }

        /// <summary>
        /// Base address to begin reading or writing the ROM contents.
        /// </summary>
        public int ImageBaseAddress { get; private set; }

        /// <summary>
        /// Size of the ROM.
        /// </summary>
        public int ImageSize { get; private set; }

        /// <summary>
        /// Which key algorithm to use to unlock the PCM.
        /// </summary>
        public int KeyAlgorithm { get; private set; }

        /// <summary>
        /// Populate this object based on the given OSID.
        /// </summary>
        public PcmInfo(uint hardwareid)
        {
            this.HardwareID = hardwareid;

            // This will be overwritten for known-to-be-unsupported operating systems.
            this.IsSupported = true;

            switch (hardwareid)
            {
                /*

                case 12201465:
                case 12201463:
                */
                case 15114624:
                case 15124091:
                case 15190861:
                case 15782454:
                case 15135638:
                    this.KeyAlgorithm = 20;
                    this.Description = "21002558 IPC";
                    this.ImageSize = 112 * 1024;
                    break;

                default:
                    this.IsSupported = false; // Not sure what the default should be...
                    this.KeyAlgorithm = 22;
                    this.Description = "Unknown";
                    this.ImageSize = 112 * 1024;
                    break;
            }
        }
    }
}
