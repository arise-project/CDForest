using System;
using System.IO;

namespace CDSearchApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Folder to search:");
            var folder = Console.ReadLine();

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("NOT FOUND");
                Console.ReadKey();
                return;
            }


            foreach (var file in Directory.GetFiles(folder, "*.search.*"))
            {

            }
        }
    }
}
