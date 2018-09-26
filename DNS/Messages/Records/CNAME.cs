using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;
using DNS.Messages;

namespace DNS.Messages.Records
{
    public class CNAME : BaseOfBaseRecord, IRecord
    {
        //public AAAA(byte[] bytes) : base(Types.AAAA) { Bytes = bytes; }
        //public AAAA(byte[] bytes, uint TTL) : base(Types.AAAA, TTL) { Bytes = bytes; }
        public CNAME(byte[][] vs, uint TTL) : base(Types.CNAME, TTL) { RawBytes = vs; }
        public CNAME(byte[][] vs) : base(Types.CNAME) { RawBytes = vs; }

        public byte[][] RawBytes { get; }

        private byte[] GetNAME()
        {
            byte[][] a = new byte[RawBytes.Length + 1][];
            int length = RawBytes.Length;
            for (int i = 0; i < length; i++)
            {
                byte nlength = (byte)RawBytes[i].Length;
                a[i] = new byte[nlength + 1];
                a[i][0] = nlength;
                for (int n = 0; n < nlength; n++)
                {
                    a[i][n + 1] = RawBytes[i][n];
                }
            }
            a[RawBytes.Length] = new byte[1] { 0 };
            return Combine(a);
        }
        
        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public byte[] Bytes { get; }

        public byte[] Serialize()
        {
            byte[] cname = GetNAME();
            byte[] rcd = new byte[8 + cname.Length];
            rcd[0] = (byte)((int)(Type) << 8);
            rcd[1] = (byte)((int)(Type));
            rcd[2] = (byte)((int)(Class) << 8);
            rcd[3] = (byte)((int)(Class));
            rcd[4] = (byte)(TTL >> 24);
            rcd[5] = (byte)(TTL >> 16);
            rcd[6] = (byte)(TTL >> 8);
            rcd[7] = (byte)(TTL);
            //rcd[8] = (byte)(cname.Length >> 8);
            //rcd[9] = (byte)(cname.Length);
            for (int i = 0, r = 8; i < cname.Length; i++, r++)
                rcd[r] = cname[i];
            return rcd;
        }

        Types IRecord.Type()
        {
            return Type;
        }

        public override string ToString()
        {
            return Encoding.ASCII.GetString(GetNAME());
        }
    }
}
