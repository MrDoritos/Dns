using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;
using DNS.Messages;
using cunt = DNS.Messages.Components.ReplyCode;
using Opcode = DNS.Messages.DnsMessage.OpCodes;
using ResponseCodes = DNS.Messages.DnsMessage.ResponseCodes;

namespace DNS.Deserialization.Header.ReplyCode
{
    public class Parser
    {
        static public cunt Parse(byte[] onlyReplyCode)
        {
            ReplyCodeBuilder replyCode = new ReplyCodeBuilder();
            replyCode.isQuery = (onlyReplyCode[2] & 1) == 1;
            replyCode.authoritativeAnswer = (onlyReplyCode[2] & 16) == 16;
            replyCode.truncation = (onlyReplyCode[2] & 32) == 32;
            replyCode.recursionDesired = (onlyReplyCode[2] & 64) == 64;
            replyCode.recursionAvailable = (onlyReplyCode[2] & 128) == 128;
            replyCode.opCode = (Opcode)(onlyReplyCode[2] & 14);
            replyCode.responseCode = (ResponseCodes)(onlyReplyCode[3] & 240);
            replyCode.questions = (ushort)((onlyReplyCode[4] << 8) | onlyReplyCode[5]);
            replyCode.answers = (ushort)((onlyReplyCode[6] << 8) | onlyReplyCode[7]);
            replyCode.authorities = (ushort)((onlyReplyCode[8] << 8) | onlyReplyCode[9]);
            replyCode.additional = (ushort)((onlyReplyCode[10] << 8) | onlyReplyCode[11]);
            return new cunt(replyCode);
        }
    }
}
