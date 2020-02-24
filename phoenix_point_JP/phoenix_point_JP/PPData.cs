using Karambolo.PO;
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
        //復元用に必要なデータ
        private Header header;
        private string footer;
        private Dictionary<string, string> rawtext;

        public Dictionary<LangCode, LangData> LangDic;
        public Header Header { get => header; }
        public string Footer { get => footer; }


        public PPData(string filename)
        {
            this.header = new Header();
            LangDic = new Dictionary<LangCode, LangData>();
            rawtext = new Dictionary<string, string>();

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
                header.langcode = langCode;
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
                        rawtext.Add(key, line);
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

        public void exportTSV(string folder)
        {
            var ja = readJAPO(header.name);
            int enpos = Array.IndexOf(header.langcode, "en");
            Debug.Assert(enpos >= 0);

            string subpath = folder + "/";
            if (!Directory.Exists(subpath))
            {
                Directory.CreateDirectory(subpath);
            }

            string filename = SettingInfo.GetIniValue(Program.WORKDIR + "lang.ini", header.name, "FileName");
            string filepath = subpath + filename + ".tsv";

            StreamWriter sw = new StreamWriter(filepath, false);
            //header出力
            sw.WriteLine(header.raw[0]);
            sw.WriteLine(header.raw[1]);
            sw.WriteLine(header.raw[2]);
            sw.WriteLine(header.raw[3]);

            foreach (KeyValuePair<string, string> raw in rawtext)
            {
                string[] fields = raw.Value.Split('\t'); //TSVファイルの場合
                fields[(int)TSVCOL.Begin + enpos] = $"\"{ja[raw.Key]}\"";
                sw.WriteLine(string.Join("\t", fields));
            }
            sw.WriteLine(footer);
            sw.Close();

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
                        Debug.Assert(LangDic.ContainsKey("en"));
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
                        Debug.Assert(LangDic.ContainsKey("en"));
                    }
                    sw.WriteLine($"msgctxt \"{id}\"");
                    sw.WriteLine($"msgid \"{template}\"");
                    sw.WriteLine($"msgstr \"{text}\"");
                    sw.WriteLine();
                }
                sw.Close();
            }

        }

        public void exportJA(string folder)
        {
            var pairdic = LangDic["en"];
            {
                string subpath = folder + "/ja";
                if (!Directory.Exists(subpath))
                {
                    Directory.CreateDirectory(subpath);
                }

                string filepath = subpath + "/" + Header.name + ".po";
                StreamWriter sw = new StreamWriter(filepath, false);
                //sw.WriteLine("#" + DateTime.Now.ToString());
                foreach (KeyValuePair<string, string> pairdata in pairdic)
                {
                    string id = pairdata.Key;
                    string text = FixText(pairdata.Value);;
                    string template = text;
                    if (LangDic.ContainsKey("en"))
                    {
                        template = FixText(LangDic["en"][id]);
                    }
                    else
                    {
                        Debug.Assert(LangDic.ContainsKey("en"));
                    }
                    sw.WriteLine($"msgctxt \"{id}\"");
                    sw.WriteLine($"msgid \"{template}\"");
                    sw.WriteLine($"msgstr \"\"");
                    sw.WriteLine();
                }
                sw.Close();

                filepath = subpath + "/" + Header.name + ".pot";
                sw = new StreamWriter(filepath, false);
                //sw.WriteLine("#" + DateTime.Now.ToString());
                foreach (KeyValuePair<string, string> pairdata in pairdic)
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
            if(data == "") { return ""; }
            return data.Substring(1, data.Length - 2);//先頭と末尾の"をとる
        }
        private string FixText(string text)
        {
            text = text.Replace("\"", "\\\"");
            return text;
        }

        private string ReFixText(string text)
        {
            text = text.Replace("\\\"", "\"");
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
        private LangData readJAPO(string filename)
        {
            LangData ja = new LangData();

            var parser = new POParser(new POParserSettings
            {
                //
            });

            TextReader reader = new StreamReader($"../locales/ja/{filename}.po", Encoding.UTF8);
            var result = parser.Parse(reader);
            if (result.Success)
            {
                var catalog = result.Catalog;
                foreach(var par in catalog)
                {
                    string id = par.Key.ContextId;
                    string value = par[0];
                    if(value == "")
                    {
                        value = par.Key.Id;
                    }
                    ja.Add(id, value);
                }
                // process the parsed data...
            }
            else
            {
                Debug.Assert(false);
            }

            /*
            StreamReader sr = new StreamReader($"./locales/ja/{filename}.po", Encoding.UTF8);
            try
            {
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] fields = line.Split(' ');
                   if(fields[0] == "msgctxt")//データのはじまり
                    {
                        string text = "";
                        while (sr.EndOfStream == false)
                        {
                            string msgidline = sr.ReadLine();
                            string[] msgidfields = msgidline.Split(' '); 
                            if (msgidfields[0] == "msgstr") //ただのmsgstrの場合次に移動
                            {
                                string msgstrline = msgidline;
                                while (sr.EndOfStream == false)
                                {
                                    for (int i = 1; i < msgstrfields.Length; i++)
                                    {
                                        jointtext += msgstrfields[i];
                                    }

                                    string msgstrline = sr.ReadLine();
                                    string[] msgstrfields = msgstrline.Split(' ');
                                    string jointtext = "";
                                    for (int i = 1; i < msgstrfields.Length; i++)
                                    {
                                        jointtext += msgstrfields[i];
                                    }
                                    text += TrimStr(jointtext);
                                }
                                break;
                            }
                            if (msgidline == "") { break; }//ただの改行の場合次のIDに移動
                        }
                        string key = "";
                        for (int i=1;i< fields.Length; i++)
                        {
                            key += fields[i];
                        }
                        ja.Add(TrimStr(key), text);
                    }
                }
            }
            finally
            {
                sr.Close();
            }
            */
            return ja;
        }
    }

    struct Header
    {
        //string Type;
        public string name;
        public string[] langcode;
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
