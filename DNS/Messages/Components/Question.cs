using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Messages.Components
{
    public class Question : BaseRecord, ISerializable
    {
        public string Raw { get => (RawBytes.Select(n => Encoding.ASCII.GetString(n))).Aggregate((a,b) => a += "." + b); }
        public byte[][] RawBytes { get; }
        public byte[] QNAME { get => GetQNAME(); }
        protected Question(byte[][] raw) : base(Types.ANY) { RawBytes = raw; }
        public Question(byte[][] raw, Types type) : base(type) { RawBytes = raw; }
        public Question(byte[][] raw, Types type, Classes @class) : base(@class, type) { RawBytes = raw; }

        private byte[] GetQNAME()
        {
            byte[][] a = new byte[RawBytes.Length + 1][];
            int length = RawBytes.Length;
            for (int i=0;i<length;i++)
            {
                byte nlength = (byte)RawBytes[i].Length;
                a[i] = new byte[nlength + 1];
                a[i][0] = nlength;
                for (int n = 0; n < nlength; n++)
                {
                    a[i][n + 1] = RawBytes[i][n];
                }
            }
            a[RawBytes.Length] = new byte[1] {0 };
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

        public override string ToString()
        {
            return Raw;
        }

        public byte[] Serialize()
        {
            return Combine(GetQNAME(), GetExtra());
        }

        private byte[] GetExtra()
        {
            byte[] vs = new byte[4];
            vs[0] = (byte)((int)Type >> 8);
            vs[1] = (byte)((int)Type);
            vs[2] = (byte)((int)Class >> 8);
            vs[3] = (byte)((int)Class);
            return vs;
        }
    }
}
