using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DNS.Messages.Records;

namespace DNS.Messages.Components
{
    public class Record
    {
        public Record() { }

        public List<A> A_Records = new List<A>();
        public List<AAAA> AAAA_Records = new List<AAAA>();
        public List<CNAME> CNAME_Records = new List<CNAME>();

        public Answer[] GetAnswers(Question question)
        {
            List<Answer> answers = new List<Answer>();
            switch (question.Type)
            {
                case BaseRecord.Types.A:
                    foreach (var a in A_Records)
                        answers.Add(new Answer(question.QNAME, a));
                    break;
                case BaseRecord.Types.AAAA:
                    foreach (var a in AAAA_Records)
                        answers.Add(new Answer(question.QNAME, a));
                    break;
                default:
                    break;
            }
            foreach (var a in CNAME_Records)
                answers.Add(new Answer(question.QNAME, a));
            return answers.ToArray();
        }
    }
}
