using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace kursova
{
    /// <summary>
    /// Interaction logic for mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        SqlConnection con;
        public mainPage()
        {
            InitializeComponent();
        }

        private void btnInterface1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InterfacePage1.xaml", UriKind.Relative));
        }

        private void btnInterface2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InterfacePage2.xaml", UriKind.Relative));
        }

        private void btnInterface3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InterfacePage3.xaml", UriKind.Relative));
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddOptions.xaml", UriKind.Relative));
        }

        public void BackupDatabase(string filePath)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            con.Open();
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            using (SqlCommand cmd = new SqlCommand("backUpDataBase", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                param1 = new SqlParameter("@path", SqlDbType.NVarChar);
                param1.Value = filePath;

                cmd.Parameters.Add(param1);
                cmd.ExecuteNonQuery();
            }
            con.Close();
            con.Dispose();
        }


        private void RestoreDatabase(string path)
        {
            string connectionString = @"server=win-63fe7un06ob\mssql;DataBase=master; Integrated Security=true";
            con = new SqlConnection(connectionString);


            string DatabaseName = "firma";

            con.Open();
            SqlCommand command;
            string sql1 = "Alter Database " + DatabaseName + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE; ";
            sql1 += "Restore Database " + DatabaseName + " FROM Disk ='" + path + "' with replace; ";
            command = new SqlCommand(sql1, con);
            command.CommandTimeout = 100;
            command.ExecuteNonQuery();
            con.Close();
            con.Dispose();

        }

        private void btnBackUp_Click(object sender, RoutedEventArgs e)
        {
            BackupDatabase(@"C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\firma.bak");
            MessageBox.Show("Резервная копия базы данных успешно создана!");
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Backup files (*.bak)|*.bak|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                RestoreDatabase(System.IO.Path.GetFileName(openFileDialog.FileName));
                MessageBox.Show("База данных успешно восстановлена!");
            }
            //    RestoreDatabase();
        }

      

    }
}
