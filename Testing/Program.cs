using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Deserialization;
using DNS.Serialization;
using DNS.Messages;
using DNS.Messages.Components;
using System.Net.Sockets;
using System.Net;

namespace Testing
{
    class Program
    {
        static Socket udpSock = new Socket(SocketType.Dgram, ProtocolType.Udp);
        static EndPoint _groupEp = new IPEndPoint(IPAddress.Any, 53);
        static Domain Org = new Domain("org");
        static Domain Iansweb = Org.AddSubdomain(new Domain("iansweb"));
        
        
        public static byte[] ahh = { 0x00, 0x02, 0x85, 0x80, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x05, 0x66, 0x6f, 0x72, 0x75, 0x6d, 0x07, 0x69, 0x61, 0x6E,
                                     0x73, 0x77, 0x65, 0x62, 0x03, 0x6f, 0x72, 0x67, 0x00, 0x00, 0x01, 0x00, 0x01, 0x04, 0x66, 0x75, 0x63, 0x6b, 0x03, 0x6f, 0x72, 0x67, 0x00, 0x00, 0x01, 0x00, 0x01};
        static void Main(string[] args)
        {
            Iansweb.A_Records.Add(new DNS.Messages.Records.A(new byte[] { 127, 0, 0, 1 }));
            Org.AddSubdomain(new Domain("bob")).AddSubdomain(new Domain("test")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 100, 100, 100, 100 }));
            Org.AddSubdomain(new Domain("hmm"));
            //Console.WriteLine("Press any key to start benchmark...");
            udpSock.Bind(new IPEndPoint(IPAddress.Any, 53));

            while (true)
            {
                //Console.ReadKey(true);
                DoThing();
                //Benchmark(1);
                //Console.WriteLine(Parser.Parse(Recieve()).ToString());
            }
        }

        static void DoThing()
        {
            var parsed = Parser.Parse(Recieve());

            List<Answer> answers = new List<Answer>();

            foreach (var question in parsed.Questions)
            {
                var ams = Org.RetrieveSubDomain(question.RawBytes);
                Answer[] answerss = new Answer[0];
                if (ams != null)
                    answerss = ams.GetAnswers(question);
                if (ams == null)
                    Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> Name not found");
                else
                {
                    if (answerss.Length < 1)
                    {
                        Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> No records found");
                    }
                    else
                    {
                        Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> {ams.FQDN}");
                        answers.AddRange(answerss);
                        foreach (var a in answerss)
                            Console.WriteLine($"\t{a.StringFQDN} -> {a.a}");
                    }
                }
            }

            ReplyCodeBuilder replyCodeBuilder = new ReplyCodeBuilder();
            replyCodeBuilder.answers = (ushort)answers.Count;
            replyCodeBuilder.questions = parsed.QuestionCount;
            replyCodeBuilder.isQuery = false;
            replyCodeBuilder.recursionAvailable = false;
            replyCodeBuilder.recursionDesired = false;
            replyCodeBuilder.responseCode = Header.ResponseCodes.NOERROR;
            replyCodeBuilder.truncation = false;
            replyCodeBuilder.authorities = 0;
            replyCodeBuilder.authoritativeAnswer = true;
            replyCodeBuilder.additional = 0;
            HeaderBuilder headerBuilder = new HeaderBuilder(replyCodeBuilder);
            headerBuilder.id = parsed.Id;
            DnsMessage dnsMessage = new DnsMessage(headerBuilder) { Answers = answers.ToArray(), Questions = parsed.Questions };
            udpSock.SendTo(Serializer.Serialize(dnsMessage), _groupEp);
        }

        static private byte[] Recieve()
        {
            while (true)
            {
                if (udpSock.Available > 0)
                {
                    byte[] toReturn = new byte[udpSock.Available];
                    udpSock.ReceiveFrom(toReturn, ref _groupEp);
                    return toReturn;
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        static void Benchmark(int repetitions)
        {            
            Domain domain = new Domain("org");
            var thing = domain.AddSubdomain(new Domain("iansweb"));
            thing.AddSubdomain(new Domain("forum")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 127, 0, 0, 1 }));
            thing.A_Records.Add(new DNS.Messages.Records.A(new byte[] { 127, 0, 0, 1 }));
            var now = DateTime.Now;
            for (int i = 0; i < repetitions; i++)
            {
                var parsed = Parser.Parse(ahh);
                //var ans = record.GetAnswer(parsed.Questions[0]);
                List<Answer> answers = new List<Answer>();


                foreach (var question in parsed.Questions)
                {
                    var ams = domain.RetrieveSubDomain(question.RawBytes);
                    Answer[] answerss = new Answer[0];
                    if (ams != null)
                        answerss = ams.GetAnswers(question);
                    if (ams == null)
                        Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> Name not found");
                    else
                    {
                        if (answerss.Length < 1)
                        {
                            Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> No records found");
                        }
                        else
                        {
                            Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> {ams.FQDN}");
                            foreach (var a in answerss)
                                Console.WriteLine($"\t{a.StringFQDN} -> {a.a}");
                        }
                    }

                }
                ReplyCodeBuilder replyCodeBuilder = new ReplyCodeBuilder();
                replyCodeBuilder.answers = (ushort)answers.Count;
                replyCodeBuilder.questions = parsed.QuestionCount;
                replyCodeBuilder.isQuery = false;
                replyCodeBuilder.recursionAvailable = false;
                replyCodeBuilder.recursionDesired = false;
                replyCodeBuilder.responseCode = Header.ResponseCodes.NOERROR;
                replyCodeBuilder.truncation = false;
                replyCodeBuilder.authorities = 0;
                replyCodeBuilder.authoritativeAnswer = true;
                replyCodeBuilder.additional = 0;
                HeaderBuilder headerBuilder = new HeaderBuilder(replyCodeBuilder);
                headerBuilder.id = parsed.Id;
                DnsMessage dnsMessage = new DnsMessage(headerBuilder) { Answers = answers.ToArray(), Questions = parsed.Questions };
                var pa = Parser.Parse(Serializer.Serialize(dnsMessage));
            }
            var time = DateTime.Now.Subtract(now);
            Console.WriteLine($"Approximate Execution Time:\r\n{((double)time.Ticks * 100000 / (double)repetitions)} Picoseconds\r\n{(double)(((double)time.Ticks / (double)repetitions) * 100)} Nanoseconds\r\n{((double)time.Milliseconds / repetitions)} Milliseconds\r\n({time.Ticks} Ticks * 100000 / {repetitions} Repetitions)");
        }
    }
}
