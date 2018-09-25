using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS;
using DNS.Messages.Components;
using DNS.Messages;
using DNS.Messages.Records;

namespace DNS.Serialization
{
    public class Serializer
    {
        public static byte[] Serialize(DnsMessage message)
        {
            byte[][] qts = new byte[message.QuestionCount][];
            byte[][] ans = new byte[message.AnswerCount][];
            for (int i = 0; i < message.QuestionCount; i++)
                qts[i] = message.Questions[i].Serialize();
            byte[] questions = Combine(qts);
            for (int i = 0; i < message.AnswerCount; i++)
                ans[i] = message.Answers[i].Serialize();
            byte[] answers = Combine(ans);
            return Combine(GetHeader(message), questions, answers);
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

        public static byte[] GetHeader(Header header)
        {
            byte[] hdr = new byte[12];
            hdr[0] = (byte)(header.Id >> 8);
            hdr[1] = (byte)(header.Id);
            byte[] rc = GetReplyCode(header);
            hdr[2] = rc[0];
            hdr[3] = rc[1];
            hdr[4] = (byte)(header.QuestionCount >> 8);
            hdr[5] = (byte)(header.QuestionCount);
            hdr[6] = (byte)(header.AnswerCount >> 8);
            hdr[7] = (byte)(header.AnswerCount);
            hdr[8] = (byte)(header.AuthorityCount);
            hdr[9] = (byte)(header.AuthorityCount >> 8);
            hdr[10] = (byte)(header.AdditionalCount);
            hdr[11] = (byte)(header.AdditionalCount >> 8);
            return hdr;
        }

        public static byte[] GetReplyCode(ReplyCode rc)
        {
            byte[] rply = new byte[2];
            rply[0] |= (byte)(GetBool(!rc.IsQuery) << 7);
            rply[0] |= (byte)((byte)((int)(rc.OpCode)) << 3);
            rply[0] |= (byte)(GetBool(rc.AuthoritativeAnswer) << 2);
            rply[0] |= (byte)(GetBool(rc.Truncation) << 1);
            rply[0] |= (byte)(GetBool(rc.RecursionDesired));
            rply[1] |= (byte)(GetBool(rc.RecursionAvailable) << 7);
            rply[1] |= (byte)(((int)rc.ResponseCode));
            return rply;
        }

        private static byte GetBool(bool t)
        {
            if (t) return 1; else return 0;
        }
    }
}
