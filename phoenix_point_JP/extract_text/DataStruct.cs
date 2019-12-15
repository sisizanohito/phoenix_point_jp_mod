using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace extract_text
{
    class DataStruct
    {
        private string dataName;
        private Dictionary<string, string> idText;//IDとテキストデータを入れるところ

        public DataStruct(string dataName)
        {
            DataName = dataName;
            idText = new Dictionary<string, string>();
        }

        public string DataName { get => dataName; set => dataName = value; }
    }
}
