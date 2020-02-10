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
        public static readonly string OUTDIR = "./Output/";
        static void Main(string[] args)
        {
            

            IEnumerable<string> files =
            System.IO.Directory.EnumerateFiles(
            WORKDIR, "*.tsv", System.IO.SearchOption.AllDirectories);

            //ファイルを列挙する
            foreach (string f in files)
            {
                Console.WriteLine(f);
                CreatePPData(f);
            }
        #if DEBUG
            Console.ReadKey();
        #endif

        }

        static void CreatePPData(string fileName)
        {
            PPData pPData = new PPData(fileName);
            pPData.exportPO(OUTDIR);
            pPData.exportPOT(OUTDIR);
        }
    }
}
