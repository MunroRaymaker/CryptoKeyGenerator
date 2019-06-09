using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    class Program
    {
        static readonly ConcurrentDictionary<string, int> Dict = new ConcurrentDictionary<string, int>();

        private static int keyLength;

        /// <summary>
        /// Test results:
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("**** Crypto Key Generator ****");

            int maxsize = 0;

            if (args.Length != 2)
            {
                Console.Write("Enter number of tries in loop:");
                maxsize = int.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.Write("Enter desired length of key:");
                keyLength = int.Parse(Console.ReadLine());
            }
            else
            {
                maxsize = int.Parse(args[0]);
                keyLength = int.Parse(args[1]);
            }

            int len = maxsize;

            Console.WriteLine("Running...");
            Console.WriteLine("Press [s] for status and <ESC> to stop");


            var timer = new Stopwatch();
            timer.Start();

            //do
            //{
            //    while (!Console.KeyAvailable && len > 0)
            //    {
            //        // Do something
            //        var code = CreateDibsOrderId();
            //        dict.AddOrUpdate(code, 1, (key, oldvalue) => oldvalue + 1);

            //        //Console.WriteLine("{0}", code);
            //        len--;
            //    }

            //    if (len == 0)
            //    {
            //        break;
            //    }

            //    var keypress = Console.ReadKey(true).Key;

            //    if (keypress == ConsoleKey.S)
            //    {
            //        // status
            //        Console.WriteLine("status: {0}/{1} processed", dict.Count, maxsize);
            //    }
            //    else if (keypress == ConsoleKey.Escape)
            //    {
            //        break;
            //    }

            //} while (len > 0);

            Parallel.For(1, maxsize, f =>
            {
                var code = CreateDibsOrderId(keyLength);

                if (maxsize < 10000)
                {
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.ManagedThreadId, code);
                }
                Dict.AddOrUpdate(code, 1, (key, oldvalue) => oldvalue + 1);
            });

            timer.Stop();

            Console.WriteLine("Execution time {0} ms.", timer.ElapsedMilliseconds);
            Console.WriteLine("dict contains {0} values", Dict.Count);
            Console.WriteLine("unique values: {0}", Dict.Count(x => x.Value == 1));
            Console.WriteLine("dupes: {0}", Dict.Count(x => x.Value > 1));

            Console.WriteLine("Maximun no of combinations: {0:n0}", Math.Pow(36, keyLength));
        }

        /// <summary>
        /// Character mask.
        /// </summary>
        /// <returns></returns>
        private static string CreateDibsOrderId(int size)
        {
            const string characters = "abcdefghijklmnopqrstuvwxyz1234567890";
            //const string characters = "abcdefghijklmnopqrstuvwxyz";

            char[] buffer = characters.ToCharArray();
            byte[] data = new byte[size];

            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);

            var result = new StringBuilder(size);

            //Random rnd = new Random();
            //result.Append(rnd.Next(1, 9));

            foreach (byte b in data)
            {
                result.Append(buffer[b % (buffer.Length - 1)]);
            }

            return result.ToString();
        }

        //private static string RNGCharacterMask()
        //{
        //    int maxSize = 8;
        //    int minSize = 5;
        //    char[] chars = new char[62];
        //    string a;
        //    a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    chars = a.ToCharArray();
        //    int size = maxSize;
        //    byte[] data = new byte[1];
        //    RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        //    crypto.GetNonZeroBytes(data);
        //    size = maxSize;
        //    data = new byte[size];
        //    crypto.GetNonZeroBytes(data);
        //    StringBuilder result = new StringBuilder(size);
        //    foreach (byte b in data)
        //    { result.Append(chars[b % (chars.Length - 1)]); }
        //    return result.ToString();
        //}
    }
}