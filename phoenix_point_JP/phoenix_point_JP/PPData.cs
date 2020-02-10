using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private string footer;//復元用に必要なデータ

        public Dictionary<LangCode, LangData> LangDic;
        public Header Header { get => header; }
        public string Footer { get => footer; }

        public PPData(string filename)
        {
            this.header = new Header();
            LangDic = new Dictionary<LangCode, LangData>();

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
                LangCode[] langCode = SettingInfo.GetIniValues(Program.WORKDIR + "lang.ini", header.name, "Lang");
                int now = 0;
                int max = Int32.Parse(header.row);
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] fields = line.Split('\t'); //TSVファイルの場合
                    if(now< max)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fields[i] = TrimStr(fields[i]);
                        }

                        string key = fields[(int)TSVCOL.Key];
                        int langNum = Int32.Parse(fields[(int)TSVCOL.Num]);
                        for (int i = 0; i < langNum; i++)
                        {
                            UpdateDic(langCode[i], key, fields[(int)TSVCOL.Begin+i]);
                            //Console.Write(fields[i] + "  ");
                        }
                        //Console.Write("------\r\n");
                    }
                    else
                    {
                        footer = line;
                        break;
                    }

                    now++;
                }
                
            }
            finally
            {
                sr.Close();
            }
        }
        public void exportPO(string folder)
        {
            foreach (KeyValuePair<LangCode, LangData> pairdic in LangDic)
            {
                string subpath = folder + "/" + pairdic.Key;
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }
                
                string filepath = subpath + "/" + Header.name+ ".po";
                StreamWriter sw = new StreamWriter(filepath, false);
                //sw.WriteLine("#"+ DateTime.Now.ToString());
                foreach (KeyValuePair<string, string> pairdata in pairdic.Value)
                {
                    string id = pairdata.Key;
                    string text = FixText(pairdata.Value);
                    string template = text;
                    if (LangDic.ContainsKey("en"))
                    {
                        template = FixText(LangDic["en"][id]);
                    }
                    else
                    {
                        Debug.Assert(!LangDic.ContainsKey("en"));
                    }
                    sw.WriteLine($"msgctxt \"{id}\"");
                    sw.WriteLine($"msgid \"{template}\"");
                    sw.WriteLine($"msgstr \"{text}\"");
                    sw.WriteLine();
                }
                sw.Close();
            }
            
        }

        public void exportPOT(string folder)
        {
            foreach (KeyValuePair<LangCode, LangData> pairdic in LangDic)
            {
                string subpath = folder + "/" + pairdic.Key;
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }

                string filepath = subpath + "/" + Header.name + ".pot";
                StreamWriter sw = new StreamWriter(filepath, false);
                //sw.WriteLine("#" + DateTime.Now.ToString());
                foreach (KeyValuePair<string, string> pairdata in pairdic.Value)
                {
                    string id = pairdata.Key;
                    string text = "";
                    string template = text;
                    if (LangDic.ContainsKey("en"))
                    {
                        template = FixText(LangDic["en"][id]);
                    }
                    else
                    {
                        Debug.Assert(!LangDic.ContainsKey("en"));
                    }
                    sw.WriteLine($"msgctxt \"{id}\"");
                    sw.WriteLine($"msgid \"{template}\"");
                    sw.WriteLine($"msgstr \"{text}\"");
                    sw.WriteLine();
                }
                sw.Close();
            }

        }
        private string TrimStr(string data)
        {
            return data.Substring(1, data.Length - 2);//先頭と末尾の"をとる
        }
        private string FixText(string text)
        {
            text = text.Replace("\"", "{d-quotation}");
            return text;
        }
        private void UpdateDic(LangCode langCode, string key, string text)
        {
            if (!LangDic.ContainsKey(langCode))//はじめだったら
            {
                Dictionary<string, string> ldata = new LangData();
                ldata.Add(key, text);
                LangDic.Add(langCode, ldata);
                return;
            }
            LangData lang = LangDic[langCode];
            lang.Add(key, text);
        }
    }

    struct Header
    {
        //string Type;
        public string name;
        public string row;
        public string[] raw; //生データ
    }
    enum TSVCOL
    {
        Key,
        Type,
        SETSUMEI,
        Num,
        Begin
    }
}
