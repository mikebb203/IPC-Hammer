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
    public class OsInfo
    {
        /// <summary>
        /// Operating system ID.
        /// </summary>
        public string OsID { get; private set; }

        public bool IsSupported { get; private set; }

        public uint BootSector { get; private set; }

        
        /// Populate this object based on the given OSID.
        /// </summary>
        public OsInfo(int calid)
        {
            //this.OsID = calid;

            // This will be overwritten for known-to-be-unsupported operating systems.
            this.IsSupported = true;

            switch (calid)
            {
                ///2003 cluster
                case 15104985:
                case 15104986:
                case 15104987:
                case 15104988:
                case 15104989:
                case 15104990:
                case 15104991:
                case 15104992:
                case 15104993:
                case 15104994:
                case 15104995:
                case 15104996:
                case 15104997:
                case 15104998:
                case 15104999:
                    this.OsID = "15124091.bin";
                    this.BootSector = 12227606;
                    
                    break;

                ///2004 cluster
                case 15130763:
                case 15130764:
                case 15130765:
                case 15130766:
                case 15130767:
                case 15130768:
                case 15130769:
                case 15130770:
                case 15130771:
                case 15130772:
                case 15130773:
                case 15130774:
                case 15130775:
                case 15130776:
                case 15130777:
                case 15130778:
                case 15130779:
                case 15130780:
                case 15130781:
                    this.OsID = "15114624.bin";
                    this.BootSector = 0;
                    break;

                ///2005 cluster
                case 15224110:
                case 15224111:
                case 15224112:
                case 15224113:
                case 15224114:
                case 15224115:
                case 15224116:
                case 15224117:
                case 15224118:
                case 15224119:
                case 15224120:
                case 15224121:
                case 15224122:
                case 15224123:
                case 15224124:
                case 15224125:
                case 15224126:
                case 15224127:
                case 15224128:
                case 15224129:
                case 15224130:
                case 15224131:
                case 15224132:
                    this.OsID = "15782454.bin";
                    this.BootSector = 12245742;
                    break;

                ///2005 cluster 2003-2004 truck
                case 15787053:
                case 15787054:
                case 15787055:
                case 15787056:
                case 15787057:
                case 15787058:
                case 15787059:
                case 15787060:
                case 15787061:
                case 15787062:
                case 15787063:
                case 15787064:
                case 15787065:
                case 15787066:
                case 15787067:
                case 15787068:
                case 15787069:
                case 15787070:
                case 15787071:
                case 15787072:
                case 15787073:
                    this.OsID = "15190861.bin";
                    this.BootSector = 12245742;
                    break;

                ///2006 cluster
                case 15105974:
                case 15105975:
                case 15105976:
                case 15105978:
                case 15105979:
                case 15105980:
                case 15105981:
                case 15105982:
                case 15105983:
                case 15105986:
                    this.OsID = "15782454.bin";
                    this.BootSector = 28011118;
                    break;

                default:
                    this.IsSupported = false; // Not sure what the default should be...
                    break;
            }
        }
    }
}
