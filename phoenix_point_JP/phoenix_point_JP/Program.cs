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
        public static readonly string WORKDIR = "./Resources/";
        public static readonly string PODIR = "./PO/";
        public static readonly string TSVDIR = "./TSV/";
        static void Main(string[] args)
        {
            // 引数チェック
            if (args.Length != 1)
            {
                Console.WriteLine("正しい引数を入力してね");
                return;
            }
            string mode = args[0];


            //TSV->po
            if (mode == "-p")
            {
                Console.WriteLine("TSV->po");
                IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                WORKDIR, "*.tsv", System.IO.SearchOption.AllDirectories);

                //ファイルを列挙する
                foreach (string f in files)
                {
                    Console.WriteLine(f);
                    CreatePO(f);
                }
            }


            //po->TSV
            if (mode == "-t")
            {
                Console.WriteLine("po->TSV");
                IEnumerable<string> files =
                System.IO.Directory.EnumerateFiles(
                WORKDIR, "*.tsv", System.IO.SearchOption.AllDirectories);

                //ファイルを列挙する
                foreach (string f in files)
                {
                    Console.WriteLine(f);
                    CreateTSV(f);
                }
            }
            

#if DEBUG
            Console.ReadKey();
            #endif

        }

        static void CreatePO(string fileName)
        {
            PPData pPData = new PPData(fileName);
            pPData.exportPO(PODIR);
            pPData.exportPOT(PODIR);
            pPData.exportJA(PODIR);
        }

        static void CreateTSV(string fileName)
        {
            PPData pPData = new PPData(fileName);
            pPData.exportTSV(TSVDIR);
        }
    }
}
