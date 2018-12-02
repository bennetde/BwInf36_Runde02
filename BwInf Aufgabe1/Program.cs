using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BwInf_Aufgabe1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Eingabe: ");
            string input = Console.ReadLine();
            int parsedValue;
            while(input.ToLower() != "exit")
            {
                if (!Int32.TryParse(input, out parsedValue))
                {
                    if(input == "clear")
                    {
                        Console.Clear();
                    }
                }
                else
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Wall w = new Wall(parsedValue);
                    sw.Stop();

                    foreach (Row r in w.rows)
                    {
                        Console.WriteLine(r.ToString());
                    }
                    Console.WriteLine("Finished in " + sw.ElapsedMilliseconds + "ms");
                }
                Console.Write("Eingabe: ");
                input = Console.ReadLine();
            }

        }
    }
}
