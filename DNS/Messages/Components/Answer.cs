using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Records;

namespace DNS.Messages.Components
{
    public class Answer : BaseRecord, ISerializable
    {
        protected Answer(Types recordType) : base(recordType) { }
        protected Answer() : base(Types.A) { }
        public Answer(byte[] FQDN, IRecord record) : base(record.Type()) { a = record; this.FQDN = FQDN; }
        public Answer(string FQDN, IRecord record) : base(record.Type()) { a = record; this.FQDN = Encoding.ASCII.GetBytes(FQDN); }
        public IRecord @a { get; }
        public byte[] FQDN { get; }
        public string StringFQDN { get => Encoding.ASCII.GetString(FQDN); }


        public byte[] Serialize()
        {
            return (Combine(FQDN, a.Serialize()));
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
    }
}
