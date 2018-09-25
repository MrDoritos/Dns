using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNS.Deserialization;
using Opcode = DNS.Messages.Components.Header.OpCodes;
using ResponseCodes = DNS.Messages.Components.Header.ResponseCodes;

namespace ResponsecodeVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[2];
            int byt = 0;
            int sel = 0;
            GetData(buffer);
            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.D0:
                        int val = (key.Key == ConsoleKey.D0) ? 0 : 1;
                        if (sel < 7 && byt < 2)
                        {
                            buffer[byt] |= (byte)(val << sel);
                            GetData(buffer);
                            sel++;
                        }
                        else
                           if (byt < 2)
                        {
                            buffer[byt] |= (byte)(val << sel);
                            GetData(buffer);
                            sel = 0;
                            byt++;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (sel > -1 && byt > -1 && byt < buffer.Length)
                        {
                            if (sel < 1)
                            {
                                byt--;
                                if (byt > -1)
                                {
                                    sel = 7;
                                    buffer[byt] = (byte)(~(~buffer[byt] ^ (1 << sel)) & buffer[byt]);
                                    GetData(buffer);
                                }
                                else
                                {
                                    byt++;
                                }
                            }
                            else
                            {
                                sel--;
                                buffer[byt] = (byte)(~(~buffer[byt] ^ (1 << sel)) & buffer[byt]);
                                GetData(buffer);
                            }
                        }
                        else
                            if (byt > 0)
                        {
                            byt--;
                            sel = 7;
                            buffer[byt] = (byte)(~(~buffer[byt] ^ (1 << sel)) & buffer[byt]);
                            GetData(buffer);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        static void GetData(byte[] buffre)
        {
            Console.Clear();
            Console.WriteLine($"{((buffre[0] & 1) == 1 ? 1 : 0)}{((buffre[0] & 2) == 2 ? 1 : 0)}{((buffre[0] & 4) == 4 ? 1 : 0)}{((buffre[0] & 8) == 8 ? 1 : 0)} {((buffre[0] & 16) == 16 ? 1 : 0)}{((buffre[0] & 32) == 32 ? 1 : 0)}{((buffre[0] & 64) == 64 ? 1 : 0)}{((buffre[0] & 128) == 128 ? 1 : 0)} {((buffre[1] & 1) == 1 ? 1 : 0)}{((buffre[1] & 2) == 2 ? 1 : 0)}{((buffre[1] & 4) == 4 ? 1 : 0)}{((buffre[1] & 8) == 8 ? 1 : 0)} {((buffre[1] & 16) == 16 ? 1 : 0)}{((buffre[1] & 32) == 32 ? 1 : 0)}{((buffre[1] & 64) == 64 ? 1 : 0)}{((buffre[1] & 128) == 128 ? 1 : 0)}");
            Console.WriteLine($"Query = {(buffre[0] & 1) == 1}");
            Console.WriteLine($"Authoritative Answer = {(buffre[0] & 16) == 16}");
            Console.WriteLine($"Truncation = {(buffre[0] & 32) == 32}");
            Console.WriteLine($"Recursion Desired = {(buffre[0] & 64) == 64}");
            Console.WriteLine($"Recursion Available = {(buffre[0] & 128) == 128}");
            Console.WriteLine($"Opcode = {(Opcode)((buffre[0] & 14) >> 1)}");
            Console.WriteLine($"Responsecode = {(ResponseCodes)((buffre[1] & 240) >> 4)}");
        }

    }
}
