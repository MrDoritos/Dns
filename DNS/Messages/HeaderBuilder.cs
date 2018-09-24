using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpCodes = DNS.Messages.Components.Header.OpCodes;
using ResponseCodes = DNS.Messages.Components.Header.ResponseCodes;
using DNS.Messages.Components;

namespace DNS.Messages
{
    public class HeaderBuilder : ReplyCodeBuilder
    {
        public HeaderBuilder(ReplyCodeBuilder replyCode) : base(replyCode) { }
        public HeaderBuilder(ReplyCode replyCode) : base(replyCode) { }
        public HeaderBuilder() { }
        public ushort id;
    }
}
