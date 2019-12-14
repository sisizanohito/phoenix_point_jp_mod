using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace extract_text
{
    class Program
    {
        const string WORKDIR = "./Resources";
        static void Main(string[] args)
        {
            IEnumerable<string> files =
            System.IO.Directory.EnumerateFiles(
            WORKDIR, "*", System.IO.SearchOption.AllDirectories);

            //ファイルを列挙する
            foreach (string f in files)
            {
                Console.WriteLine(f);
            }

#if DEBUG
            Console.ReadKey();
            #endif
        }
    }
}
