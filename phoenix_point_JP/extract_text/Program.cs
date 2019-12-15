using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace extract_text
{
    class Program
    {
        const string WORKDIR = "./Resources/";
        static void Main(string[] args)
        {
            IEnumerable<string> files =
            System.IO.Directory.EnumerateFiles(
            WORKDIR, "*", System.IO.SearchOption.AllDirectories);

            //ファイルを列挙する
            foreach (string f in files)
            {
                Console.WriteLine(f);
                byte[] raw = ReadFile(f);
            }
            

            #if DEBUG
                Console.ReadKey();
            #endif
        }

        //ヘッダー？情報の28バイト分スキップ
        public static byte[] ReadFile(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }

}
