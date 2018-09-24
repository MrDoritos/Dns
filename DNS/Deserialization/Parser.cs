using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages;
using DNS.Messages.Components;
using Opcode = DNS.Messages.Components.Header.OpCodes;
using ResponseCodes = DNS.Messages.Components.Header.ResponseCodes;

namespace DNS.Deserialization
{
    public class Parser
    {
        public static DnsMessage Parse(byte[] buffer)
        {
            HeaderBuilder header = new HeaderBuilder();
            header.id = (ushort)((buffer[0] << 8) | buffer[1]);
            header.isQuery = (buffer[2] & 1) == 1;
            header.authoritativeAnswer = (buffer[2] & 16) == 16;
            header.truncation = (buffer[2] & 32) == 32;
            header.recursionDesired = (buffer[2] & 64) == 64;
            header.recursionAvailable = (buffer[2] & 128) == 128;
            header.opCode = (Opcode)(buffer[2] & 14);
            header.responseCode = (ResponseCodes)(buffer[3] & 240);
            header.questions = (ushort)((buffer[4] << 8) | buffer[5]);
            header.answers = (ushort)((buffer[6] << 8) | buffer[7]);
            header.authorities = (ushort)((buffer[8] << 8) | buffer[9]);
            header.additional = (ushort)((buffer[10] << 8) | buffer[11]);
            return new DnsMessage(header, GetQuestions(12, buffer, header.questions));
        }

        public static Question[] GetQuestions(int position, byte[] questions, ushort count)
        {
            Question[] questionArray = new Question[count];
            for (int i = 0; i < count; i++)
                questionArray[i] = GetQuestion(ref position, questions);
            return questionArray;
        }

        public static Question GetQuestion(ref int position, byte[] question)
        {
            byte length;
            List<byte[]> d = new List<byte[]>();
            while ((length = question[position]) != 0)
            {                
                position++;
                byte[] toadd = new byte[length];
                for (int i = 0; i < length; i++)
                    toadd[i] = question[position + i];
                position += length;
                d.Add(toadd);
            }
            position++;
            BaseRecord.Types type = (BaseRecord.Types)((question[position] << 8) | question[position + 1]);
            BaseRecord.Classes @class = (BaseRecord.Classes)((question[position + 2] << 8) | question[position + 3]);
            position += 4;
            return new Question(d.ToArray(), type, @class);
        }
    }
}
