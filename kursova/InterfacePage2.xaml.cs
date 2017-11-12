using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for InterfacePage2.xaml
    /// </summary>
    public partial class InterfacePage2 : Page
    {
        SqlConnection con;
        ComboBox obj = new ComboBox();
        DataTable dta = new DataTable();
        int LenghtProp = 0;
        string TabName = "";
        string SelectString = "";
        int ind = 0;
        public InterfacePage2()
        {
            InitializeComponent();
            fillCombo(comboBox);
        }
        void fillCombo(object obj)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where Not(TABLE_NAME like 'sysdiagrams')" , conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "INFORMATION_SCHEMA");
            (obj as ComboBox).ItemsSource = ds.Tables[0].DefaultView;
            (obj as ComboBox).DisplayMemberPath = ds.Tables[0].Columns["TABLE_NAME"].ToString();

        }
        void GetColumn(string Tab)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('"+ Tab+"') ", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "sys.columns");

            obj.ItemsSource = ds.Tables[0].DefaultView;
            obj.DisplayMemberPath = ds.Tables[0].Columns["name"].ToString();

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectString = "";
            pannelFind.Children.Clear();
            string str = ((sender as ComboBox).SelectedItem as DataRowView).Row["TABLE_NAME"] as string;
            TabName = str;
            List<string> lst = new List<string>();
            string[] arr = { };
            GetColumn(str);
            foreach(var t in obj.Items)
            {
                lst.Add((t as DataRowView).Row["name"].ToString());
            }
            arr = lst.ToArray();
            for(int i = 0; i <arr.Length; i++)
            {
                if (arr[i] != "id")
                {
                    StackPanel pan = new StackPanel();
                    pan.Name = arr[i];
                    Label lb = new Label();
                    lb.MinWidth = 120;
                    lb.FontSize = 20;
                    lb.Content = arr[i];
                    pan.Children.Add(lb);
                    TextBox tb = new TextBox();
                    tb.FontSize = 20;
                    pan.Children.Add(tb);
                    pannelFind.Children.Add(pan);
                }
            }
            LenghtProp = arr.Length;
        }
        private void getTableInDataGrid(string str)
        {
            string sql = "";
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                if (str != "")
                    sql = "SELECT * from [" + TabName + "]" + " WHERE " + str;
                else
                    sql = "SELECT * from [" + TabName + "]";
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        adapter.Fill(ds, TabName);

                        dta = ds.Tables[0];
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGrid.ItemsSource = dt.DefaultView;
                    }
                    catch (System.ArgumentException)
                    {
                        MessageBox.Show("Таблица не выбрана");
                    }

                }
            }
        }
        private void generateStr(string param, string val)
        {
            if (SelectString != "")
                SelectString += " AND " + param + " LIKE '" + val + "%' ";
            else
            {
                if(param != "id")
                SelectString += " " + param + " LIKE '" + val + "%' ";
                else
                    SelectString += " " + param + "=" + val + " ";
            }
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in pannelFind.Children)
            {
                string param = (t as StackPanel).Name.ToString(); ;
                string val = ((t as StackPanel).Children[1] as TextBox).Text;
                if(val != "")
                generateStr(param, val);
            }
            getTableInDataGrid(SelectString);
            SelectString = "";
            
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtStatus.Text = "";
            string str = "";
            var t = (sender as ListView).SelectedIndex;
            if (t == 0)
            {
                str = "SELECT DISTINCT id_client, Surname, Name,Patronymic, Citizenship, birthdate, identification_number, passport_ID," +
                    "passport_Series,residential_address FROM clients_servises inner join Clients on clients_servises.id_client = Clients.id" +
                  " WHERE id_client in (SELECT l2.id_client" +
                  " FROM clients_servises l2" +
                   " GROUP BY l2.id_client" +
                  " HAVING COUNT(*) = " + txtcount.Text +
                 " ); ";
                
            }
            if (t == 1)
            {
                str = "Select * from Clients inner join clients_servises on Clients.id = clients_servises.id_client where date='"+dateFrom+"'";

            }
            if (t == 2)
            {
                str = "Select * from Clients inner join clients_servises on Clients.id = clients_servises.id_client where date > '" + dateFrom + "' AND date < '"+dateTo+"'";

            }
            if (t == 3)
            {
                str = "Select * from Clients inner join clients_servises on Clients.id = clients_servises.id_client where status !='true' OR status is null";

            }
            AddedSelect(str);
        }
        private void AddedSelect(string str)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = str;
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGrid.ItemsSource = dt.DefaultView;
                    }
                    catch (System.Data.SqlClient.SqlException)
                    {
                        MessageBox.Show("Дополнительные данные не введены!");
                    }
                }
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridColumn t2 =  dataGrid.ColumnFromDisplayIndex(ind);
            Regex regex = new Regex("[^0-9.-]+"); 

            DataView MyDataView = new DataView(dta);
            var c = dataGrid.SelectedCells;
            

            if (!regex.IsMatch(textBox.Text))
                MyDataView.RowFilter = "Convert(" + t2.Header + ", 'System.String') " + " Like '" + textBox.Text.ToString() + "%'";
            else
            {
              
                    MyDataView.RowFilter = "" + t2.Header + " like '%" + textBox.Text.ToString() + "%'";
               
            }
            dataGrid.ItemsSource = MyDataView;
           
            
        }

        private void dataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                ind = dataGrid.SelectedCells[0].Column.DisplayIndex;
                txtStatus.Text = "Фильтр для " + dataGrid.SelectedCells[0].Column.Header;
            }
        }

        private void label2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));

        }

        private void TextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           var t =  (sender as TextBox).Parent;
        }
        
        
    }
}
