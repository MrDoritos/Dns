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
using DNS;
using System.Threading;

namespace Testing
{
    class Program
    {
        static Socket udpSock = new Socket(SocketType.Dgram, ProtocolType.Udp);
        static EndPoint _groupEp = new IPEndPoint(IPAddress.Any, 53);
        
        static DomainHandler domainHandler = new DomainHandler();
        
        public static byte[] ahh = { 0x00, 0x02, 0x85, 0x80, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x05, 0x66, 0x6f, 0x72, 0x75, 0x6d, 0x07, 0x69, 0x61, 0x6E,
                                     0x73, 0x77, 0x65, 0x62, 0x03, 0x6f, 0x72, 0x67, 0x00, 0x00, 0x01, 0x00, 0x01, 0x04, 0x66, 0x75, 0x63, 0x6b, 0x03, 0x6f, 0x72, 0x67, 0x00, 0x00, 0x01, 0x00, 0x01};
        static void Main(string[] args)
        {
            Domain org = domainHandler.AddTLD(new Domain("org"));
            Domain com = domainHandler.AddTLD(new Domain("com"));
            com.AddSubdomain(new Domain("instagram")).AddSubdomain(new Domain("i")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 185, 60, 216, 52 }));
            Domain gg = domainHandler.AddTLD(new Domain("gg"));
            Domain net = domainHandler.AddTLD(new Domain("net"));
            Domain Iansweb = org.AddSubdomain(new Domain("iansweb"));
            Domain discordapp = com.AddSubdomain(new Domain("discordapp"));
            Domain discordapp_net = net.AddSubdomain(new Domain("discordapp"));
            discordapp_net.AddSubdomain(new Domain("dl")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 104,16,247,144 }, 60));
            discordapp_net.AddSubdomain(new Domain("media")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 104, 16, 248, 144 }, 60));
            Domain discord = gg.AddSubdomain(new Domain("discord"));
            Domain gateway = discord.AddSubdomain(new Domain("gateway"));
            gateway.A_Records.Add(new DNS.Messages.Records.A(new byte[] { 104, 16, 60, 37 }, 60));
            Iansweb.A_Records.Add(new DNS.Messages.Records.A(new byte[] { 192, 168, 1, 7 }, 60));
            discordapp.A_Records.Add(new DNS.Messages.Records.A(new byte[] { 104, 16, 58, 5 }, 60));
            org.AddSubdomain(new Domain("bob")).AddSubdomain(new Domain("test")).A_Records.Add(new DNS.Messages.Records.A(new byte[] { 100, 100, 100, 100 }));
            org.AddSubdomain(new Domain("hmm"));
            //Console.WriteLine("Press any key to start benchmark...");
            udpSock.Bind(new IPEndPoint(IPAddress.Any, 53));
            //udpSock.Bind(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 53));

            new Thread(UIThread).Start();
            while (true)
            {
                //Console.ReadKey(true);
                try { DoThing(); } catch { }
                //var b = Serializer.GetHeader(new Header(new HeaderBuilder(new ReplyCode(new ReplyCodeBuilder() { authoritativeAnswer = true, answers = 0xFF, questions = 0xFF, responseCode = (Header.ResponseCodes)15, additional = 0, authorities = 0, isQuery = true, opCode = (Header.OpCodes)7, recursionAvailable = false, recursionDesired = false, truncation = false, })) { id = 0x9C40}));
                //Console.WriteLine(b);
                //Benchmark(1);
                //Console.WriteLine(Parser.Parse(Recieve()).ToString());
            }
        }

        static string Execute(string command)
        {
            string[] spl = command.ToLower().Split(' ');
            if (spl.Length > 0)
            {
                switch (spl[0])
                {
                    case "add":
                        try
                        {
                            string name = "";
                            byte[] address = new byte[4];
                            Domain current = null;
                            Domain last = null;
                            for (int i = 1; i < spl.Length; i++)
                            {
                                switch (i)
                                {
                                    case 1:
                                        //Get the name
                                        string[] domainname = spl[i].Split('.');
                                        byte[][] ss = new byte[domainname.Length][];
                                        for (int aaa = 0; aaa < domainname.Length; aaa++)
                                        {
                                            ss[aaa] = new byte[domainname[aaa].Length];
                                            for (int d = 0; d < domainname[aaa].Length; d++)
                                                ss[aaa][d] = (byte)domainname[aaa][d];
                                        }
                                        for (int s = 0; current == null &&  s < ss.Length; s++)
                                        {
                                            current = domainHandler.RetrieveSubDomain(ss);
                                            if (last == null)
                                                if (current != null)
                                                    last = current.AddSubdomain(new Domain(ss[s]));
                                                else
                                                    last = domainHandler.AddTLD(new Domain(ss[s]));
                                            else
                                                last = last.AddSubdomain(new Domain(ss[s]));
                                        }
                                        break;
                                    case 2:
                                        //Get the address
                                        string[] splt = spl[i].Split('.');
                                        for (int b = 0; b < splt.Length; b++)
                                            address[b] = byte.Parse(splt[b]);
                                        break;
                                }
                            }
                            if (last != null)
                            {
                                last.A_Records.Add(new DNS.Messages.Records.A(address));
                                return $"ADDED {last.FQDN} [{last.A_Records[0]}]";
                            }
                            else
                            {
                                return "Invalid name supplied";
                            }
                        }
                        catch (Exception e)
                        {
                            return $"Syntax Error (ADD) {e.Message} {e.Source}";
                        }
                        return "Syntax Error (ADD)";
                    case "del":
                        return "Syntax Error (DEL)";
                }
            }
            return "Syntax Error";
        }

        static void UIThread()
        {
            string buffer = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Backspace:
                        if (buffer.Length > 0) buffer = buffer.Substring(0, buffer.Length - 1);
                        WriteCharacters(buffer);
                        break;
                    case ConsoleKey.Enter:
                        if (buffer.Length > 0)
                        {
                            ClearCharacters();
                            Console.WriteLine(Environment.MachineName+ ">"+buffer);
                            Console.WriteLine(Execute(buffer));
                        }
                        buffer = "";
                        break;
                    default:
                        if (buffer.Length < Console.WindowWidth - 1)
                            try
                            {
                                buffer += key.KeyChar;
                                WriteCharacters(buffer);
                            }
                            catch { }
                        break;
                }
            }
        }

        static void WriteCharacters(string buffer)
        {
            int bottom = Console.WindowHeight + Console.WindowTop - 1;
            int last = Console.CursorTop;
            Console.SetCursorPosition(0, bottom);
            Console.Write(Environment.MachineName + ">" + buffer + " ");
            Console.SetCursorPosition(0, last);
        }

        static void ClearCharacters()
        {
            int bottom = Console.WindowHeight + Console.WindowTop - 1;
            int last = Console.CursorTop;
            char[] erase = new char[Console.WindowWidth - 1];
            for (int i = 0; i < erase.Length; i++)
                erase[i] = ' ';
            Console.SetCursorPosition(0, bottom);
            Console.Write(erase, 0, erase.Length);
            Console.SetCursorPosition(0, last);
        }

        static void DoThing()
        {
            var parsed = Parser.Parse(Recieve());

            List<Answer> answers = new List<Answer>();

            ReplyCodeBuilder replyCodeBuilder = new ReplyCodeBuilder();
            foreach (var question in parsed.Questions)
            {
                var ams = domainHandler.RetrieveSubDomain(question.RawBytes);
                Answer[] answerss = new Answer[0];
                if (ams != null)
                    answerss = ams.GetAnswers(question);
                if (ams == null)
                {
                    Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> Name not found");
                    replyCodeBuilder.responseCode = Header.ResponseCodes.NAMEERROR;
                }
                else
                {
                    if (answerss.Length < 1)
                    {
                        Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> No records found");                        
                    }
                    else
                    {
                        Console.WriteLine($"{question.Raw} {question.Type} {question.Class} -> {ams.FQDN}");
                        replyCodeBuilder.responseCode = Header.ResponseCodes.NOERROR;
                        answers.AddRange(answerss);
                        foreach (var a in answerss)
                            Console.WriteLine($"\t{a.StringFQDN} -> {a.a}");
                    }
                }
            }

            replyCodeBuilder.answers = (ushort)answers.Count;
            replyCodeBuilder.questions = parsed.QuestionCount;
            replyCodeBuilder.isQuery = false;
            replyCodeBuilder.recursionAvailable = false;
            replyCodeBuilder.recursionDesired = false;
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
                //System.Threading.Thread.Sleep(10);
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
