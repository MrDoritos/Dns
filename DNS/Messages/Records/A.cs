using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;

namespace DNS.Messages.Records
{
    public class A : BaseRecord, IRecord
    {
        public A(byte[] record) :base (Classes.IN, Types.A) { Record = record; }
        public uint TTL { get; set; }
        public byte[] Record { get; }

        public byte[] Serialize()
        {
            byte[] rcd = new byte[14];
            rcd[0] = (byte)((int)(Type) << 8);
            rcd[1] = (byte)((int)(Type));
            rcd[2] = (byte)((int)(Class) << 8);
            rcd[3] = (byte)((int)(Class));
            rcd[4] = (byte)(TTL >> 24);
            rcd[5] = (byte)(TTL >> 16);
            rcd[6] = (byte)(TTL >> 8);
            rcd[7] = (byte)(TTL);
            rcd[8] = 0;
            rcd[9] = 4;
            rcd[10] = Record[0];
            rcd[11] = Record[1];
            rcd[12] = Record[2];
            rcd[13] = Record[3];
            return rcd;
        }

        Types IRecord.Type()
        {
            return Type;
        }

        public override string ToString()
        {
            return $"{Record[0]}.{Record[1]}.{Record[2]}.{Record[3]}";
        }
    }
}
