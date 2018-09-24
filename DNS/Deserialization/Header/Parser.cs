using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;
using DNS.Messages;
using cunt = DNS.Messages.Components.Header;

namespace DNS.Deserialization.Header
{
    public class Parser
    {
        public static cunt Parse(byte[] onlyHeader)
        {
            var what = ReplyCode.Parser.Parse(onlyHeader);
            HeaderBuilder header = new HeaderBuilder(what);
            header.id = (ushort)((onlyHeader[0] << 8) | onlyHeader[1]);
            //HeaderBuilder header = new HeaderBuilder()
            
            return new cunt(header);
        }
    }
}
