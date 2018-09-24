using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages;

namespace DNS.Messages.Components
{
    public class Header : ReplyCode
    {
        private ushort _id;

        public ushort Id => _id;

        public Header() { }
        public Header(HeaderBuilder header) : base(header) { _id = header.id; }
        

        public enum OpCodes : byte
        {
            QUERY = 0,
            IQUERY = 1,
            STATUS = 2,
        }

        public enum ResponseCodes : byte
        {
            NOERROR = 0,
            FORMATERROR = 1,
            SERVERFAILURE = 2,
            NAMEERROR = 3,
            NOTIMPLEMENTED = 4,
            REFUSED = 5,
        }
    }
}
