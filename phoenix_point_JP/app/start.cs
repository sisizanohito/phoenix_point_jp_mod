using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace app
{
    public partial class start : Form
    {
        public start()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ret = "";
            var dlg = new CommonOpenFileDialog();

            //設定
            dlg.IsFolderPicker = true;  //フォルダ選択ダイアログの場合はtrue
            dlg.InitialDirectory = @"c:\";  //開いておくフォルダ
            dlg.DefaultDirectory = @"~";  //最近使用したフォルダが利用できない場合の代替えフォルダ

            dlg.Title = "Phoenix Pointのゲームフォルダを選択してください";

            //表示
            var Path = dlg.ShowDialog();
            if (Path == CommonFileDialogResult.Ok)
            {
                ret = dlg.FileName;
            }
            Console.WriteLine(ret);
        }
    }
}
