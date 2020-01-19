using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace phoenix_point_JP
{
    class Program
    {
        const string WORKDIR = "./Resources/";
        static void Main(string[] args)
        {
            IEnumerable<string> files =
            System.IO.Directory.EnumerateFiles(
            WORKDIR, "*.tsv", System.IO.SearchOption.AllDirectories);

            //ファイルを列挙する
            foreach (string f in files)
            {
                Console.WriteLine(f);
                ReadTSV(f);
            }
        #if DEBUG
            Console.ReadKey();
        #endif

        }

        static void ReadTSV(string fileName)
        {
            StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
            try
            {
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    //string[] fields = line.Split(',');
                    string[] fields = line.Split('\t'); //TSVファイルの場合

                    for (int i = 0; i < fields.Length; i++)
                    {
                        Console.Write(fields[i] + "  ");
                    }
                    Console.Write("------\r\n");

                }
            }
            finally
            {
                sr.Close();
            }
            
        }
    }
}
