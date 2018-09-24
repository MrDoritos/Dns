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

        public Answer[] GetAnswers(Question question)
        {
            List<Answer> answers = new List<Answer>();
            switch (question.Type)
            {
                case BaseRecord.Types.A:
                    foreach (var a in A_Records)
                        answers.Add(new Answer(question.QNAME,a));
                    break;
                default:
                    break;
            }
            return answers.ToArray();
        }
    }
}
