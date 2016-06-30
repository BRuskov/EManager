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
using System.Windows.Shapes;
using ExpenseManagerWPF.EMCore;

namespace ExpenseManagerWPF
{
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
            InitEM();
        }

        public void InitEM()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            LoadFiles(path);
        }

        private void LoadFiles(string path)
        {
            string[] fileEntries = Directory.GetFiles(path, "*.xml");
            cbUsers.ItemsSource = fileEntries;
        }

        private void selectUserBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(cbUsers.SelectedItem.ToString(), false);
            mw.Show();
            this.Close();
        }

        private void newUserBtn_Click(object sender, RoutedEventArgs e)
        {
            //    //Log MB
            //    string path = tbFileName.Text + ".xml";
            //    MessageBox.Show(path);

            MainWindow mw = new MainWindow(tbFileName.Text, true);
            mw.Show();
            this.Close();
        }
    }
}
