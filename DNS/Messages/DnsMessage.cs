using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Messages.Components;

namespace DNS.Messages
{
    public class DnsMessage : Header
    {
        public Question[] Questions { get; set; }
        public Answer[] Answers { get; set; }

        public DnsMessage() { }
        public DnsMessage(HeaderBuilder header) : base(header) { }
        public DnsMessage(KeyValuePair<Question, Answer> questionsAndAnswers) { }
        public DnsMessage(HeaderBuilder header, Question[] questions) : base(header) { Questions = questions; }

        public override string ToString()
        {
            return $"{Id.ToBitString()}\r\n{this.ToBitString()}\r\n{QuestionCount.ToBitString()}\r\n{AnswerCount.ToBitString()}\r\n{AuthorityCount.ToBitString()}\r\n{AdditionalCount.ToBitString()}";
        }        
    }

    static class RandomExtensions
    {
        static public string ToBitString(this ushort value)
        {
            return $"{value.DoThing(32767)} {value.DoThing(16383)} {value.DoThing(8192)} {value.DoThing(4096)} {value.DoThing(2048)} {value.DoThing(1024)} {value.DoThing(512)} {value.DoThing(256)} {value.DoThing(128)} {value.DoThing(64)} {value.DoThing(32)} {value.DoThing(16)} {value.DoThing(8)} {value.DoThing(4)} {value.DoThing(2)} {value.DoThing(1)}";
        }

        static public string ToBitString(this ReplyCode value)
        {
            return $"{value.IsQuery.DoThing()} {((int)value.OpCode).DoThing(3)} {value.AuthoritativeAnswer.DoThing()} {value.Truncation.DoThing()} {value.RecursionDesired.DoThing()} {value.RecursionAvailable.DoThing()} 0 0 0 0 {((int)value.ResponseCode).DoThing(4)}";
        }

        static private string DoThing(this int value, int places)
        {
            if (places > 15 * 2 - 1) places = 15 * 2 - 1;
            return ((ushort)value).ToBitString().Substring(0,places * 2 - 1);
        }
        
        static private char DoThing(this ushort value, int place)
        {
            if ((value & place) == place) return '1'; else return '0';
        }

        static private char DoThing(this bool value)
        {            
            if (value) return '1'; else return '0';
        }        
    }
}
