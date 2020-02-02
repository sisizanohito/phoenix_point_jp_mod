using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace phoenix_point_JP
{
    using LangData = System.Collections.Generic.Dictionary<string, string>;
    using LangCode = String;

    class PPData
    {
        private Header header;
        private string footer;//復元用に必要なデータらしい

        public Dictionary<LangCode, LangData> LangData;
        public Header Header { get => header; }
        public string Footer { get => footer; }

        public PPData(string filename)
        {
            this.header = new Header();
            LangData = new Dictionary<LangCode, LangData>();

            StreamReader sr = new StreamReader(filename, Encoding.UTF8);
            try
            {
                //ヘッダの読み込み
                string Type = sr.ReadLine();
                string Name = sr.ReadLine();
                string hoge = sr.ReadLine();//謎
                string row = sr.ReadLine();

                header.raw = new string[4] { Type, Name, hoge, row };
                header.name = Name.Split('\t')[1];
                header.row = row.Split('\t')[1];

                string[] langCode = SettingInfo.GetIniValues(Program.WORKDIR + "lang.ini", header.name, "Lang");
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
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

    struct Header
    {
        //string Type;
        public string name;
        public string row;
        public string[] raw; //生データ
    }
}
