using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Messages.Components
{
    public class BaseRecord
    {
        public Classes Class { get; }
        public Types Type { get; }

        public BaseRecord(Classes @class, Types type) { Class = @class; Type = type; }
        public BaseRecord(Types type) { Type = type; Class = Classes.IN; }
        
        public enum Classes : ushort
        {
            IN = 0x0001,
        }

        public enum Types : ushort
        {
            A = 0x0001,
            NS = 0x0002,
            CNAME = 0x0005,
            SOA = 0x0006,
            WKS = 0x000B,
            PTR = 0x000C,
            MX = 0x000F,
            SRV = 0x0021,
            AAAA = 0x001C,
            ANY = 0x00FF,
        }
    }
}
