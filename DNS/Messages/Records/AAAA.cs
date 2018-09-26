using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;
using DNS.Messages;

namespace DNS.Messages.Records
{
    public class AAAA : BaseOfBaseRecord, IRecord
    {
        public AAAA(byte[] bytes) : base(Types.AAAA) { Bytes = bytes; }
        public AAAA(byte[] bytes, uint TTL) : base(Types.AAAA, TTL) { Bytes = bytes; }
        
        public byte[] Bytes { get; }

        public byte[] Serialize()
        {
            byte[] rcd = new byte[26];
            rcd[0] = (byte)((int)(Type) << 8);
            rcd[1] = (byte)((int)(Type));
            rcd[2] = (byte)((int)(Class) << 8);
            rcd[3] = (byte)((int)(Class));
            rcd[4] = (byte)(TTL >> 24);
            rcd[5] = (byte)(TTL >> 16);
            rcd[6] = (byte)(TTL >> 8);
            rcd[7] = (byte)(TTL);
            rcd[8] = 0;
            rcd[9] = 16;
            for (int i = 0, r = 10; i < 16; i++, r++)
                rcd[r] = Bytes[i];
            return rcd;
        }

        Types IRecord.Type()
        {
            return Type;
        }

        public override string ToString()
        {
            return BitConverter.ToString(Bytes, 0, 16);
        }
    }
}
