using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace wapp
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string WORKDIR = "./work/";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string dir = "";
            var dlg = new CommonOpenFileDialog();

            //設定
            dlg.IsFolderPicker = true;  //フォルダ選択ダイアログの場合はtrue
            dlg.InitialDirectory = @"c:\";  //開いておくフォルダ
            dlg.DefaultDirectory = @"c:\";  //最近使用したフォルダが利用できない場合の代替えフォルダ

            dlg.Title = "Phoenix Pointのexeがあるフォルダを指定してください";

            //表示
            var Path = dlg.ShowDialog();
            if (Path == CommonFileDialogResult.Ok)
            {
                dir = dlg.FileName;
            }
            textBox.Text = dir;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string ppfolder = textBox.Text+ "\\PhoenixPointWin64_Data\\";
            if (!Directory.Exists(ppfolder))
            {
                MessageBox.Show("Phoenix Pointのexeがあるフォルダを指定してください", "エラー" ,MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string subpath = ppfolder + "\\Unity_Assets_Files\\sharedassets0\\";
            if (!Directory.Exists(subpath))
            {
                Directory.CreateDirectory(subpath);
            }

            Directory.SetCurrentDirectory(WORKDIR);

            Process pbat = new Process();
            pbat.StartInfo.FileName = @".\make.bat";
            pbat.Start();
            pbat.WaitForExit();
            pbat.Close();

            File.Copy(@"./REPACK/sharedassets0_00001.-240", subpath + @"sharedassets0_00001.-240", true);
            File.Copy(@"./REPACK/sharedassets0_00002.-240", subpath + @"sharedassets0_00002.-240", true);
            File.Copy(@"./REPACK/sharedassets0_00003.-240", subpath + @"sharedassets0_00003.-240", true);
            File.Copy(@"./REPACK/sharedassets0_00004.-240", subpath + @"sharedassets0_00004.-240", true);
            File.Copy(@"./REPACK/sharedassets0_00006.-240", subpath + @"sharedassets0_00006.-240", true);

            Process punityex = new Process();
            punityex.StartInfo.FileName = @".\UnityEX.exe";
            punityex.StartInfo.Arguments = $"import \"{ppfolder}sharedassets0.assets\"";
            punityex.Start();
            //punityex.WaitForExit();
            System.Threading.Thread.Sleep(10000);
            punityex.Close();

            MessageBox.Show("日本語化が完了しました", "終了", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
