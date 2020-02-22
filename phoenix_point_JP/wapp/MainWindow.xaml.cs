using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
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

            dlg.Title = "PP日本語化mod";

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


        }
    }
}
