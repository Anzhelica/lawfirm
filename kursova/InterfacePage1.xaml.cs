using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for InterfacePage1.xaml
    /// </summary>
    /// 
    
    public class ClassName
    {
        public string Col1 { get; set; }

    }
    
        public partial class InterfacePage1 : Page
    {
        List<string> ltt = new List<string>();

        
        string tabName = "";
        SqlConnection con;
        DataSet ds;
        DataTable mainTab = new DataTable();
        SqlDataAdapter adapter2;
        DataGridColumn dgc;
        string cht = "";
        public InterfacePage1()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;
            ShowTabelsName();
        }
       
        
        void ShowTabelsName()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlCommand s = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES where Not(TABLE_NAME like 'sysdiagrams') Order BY TABLE_NAME", con);
            con.Open();
            s.CommandTimeout = 100;
                SqlDataReader R = s.ExecuteReader();
                if (R.HasRows)
                {
                    while (R.Read())
                    {
                        ListBoxItem item2 = new ListBoxItem();
                        item2.Content = R[0].ToString();
                        item2.Selected += Table_Selected;
                        listBox.Items.Add(item2);
                    }
                }
           
            con.Close();
        }
        private void getTableInDataGrid(string tab)
        {
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = "SELECT * from [" + tab + "]";
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    ds = new DataSet();
                    adapter.Fill(ds, tab);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    adapter2 = adapter;

                    mainTab = dt;
                    dataGrid.ItemsSource = dt.DefaultView;
                }
            }
        }
        
        private bool IsExistItem(string item)
        {
            for (int i = 0; i < tabs.Items.Count; i++)
            {
                if ((tabs.Items[i] as TabItem).Header.ToString() == item)
                    return true;
            }
            return false;
        }
        private void Table_Selected(object sender, RoutedEventArgs e)
        {
            TabItem tab = new TabItem();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            ListBoxItem t = (ListBoxItem)sender;
            getTableInDataGrid(t.Content.ToString());
            ContextMenu c = new ContextMenu();
            MenuItem m = new MenuItem();
            m.Header = "Закрыть";
            m.Click += MenuItem_Click;
            tab = new TabItem
            {
                Header = t.Content.ToString(),
                ContextMenu = c,
            };
            if (!IsExistItem(t.Content.ToString()))
            {
                c.Items.Add(m);
                tabs.SelectionChanged += Change_Tab;
                tabs.Items.Add(tab);
            }

            for (int i = 0; i < tabs.Items.Count; i++)
            {
                if ((tabs.Items[i] as TabItem).Header == t.Content)
                    tabs.SelectedIndex = i;
            }
            tabs.SelectedItem = tab;
            con.Close();
            tabName = t.Content.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var target = (FrameworkElement)sender;
            while (target is ContextMenu == false)
                target = (FrameworkElement)target.Parent;
            var tabItem = (target as ContextMenu).PlacementTarget;
            tabs.Items.Remove(tabItem);
        }
        void DeleteTabel()
        {
            con.Open();
            DataTable dt = new DataTable();
            dt = ((DataView)dataGrid.ItemsSource).ToTable();
            string qry = @"select * from " + tabName;
            string del = "DROP TABLE " + tabName;
            SqlConnection conn = con;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(qry, conn);

            DataSet ds = new DataSet();
            da.Fill(ds, tabName);

            SqlCommand cmd = new SqlCommand(del, conn);
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        string CreateDeleteSelect()
        {
            string str = "DELETE from " + tabName + " WHERE id = @id ";
            object obj = tabName as object;
            return str;
        }

        private void Change_Tab(object sender, RoutedEventArgs e)
        {
            try
            {
                var t = tabs.SelectedItem as TabItem;
                var target2 = t.Header;
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                tabName = target2.ToString();
                getTableInDataGrid(target2.ToString());
            }
            catch (NullReferenceException)
            {
                tabs.SelectedIndex = tabs.SelectedIndex;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);

            var list = new List<ClassName>();
            ClassName[] allRecords = null;
            string sql = @"select c.name from sys.columns c inner join sys.objects o 
                on c.object_id=o.object_id where o.name = '" + tabName + "'order by o.name,c.column_id";
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    list = new List<ClassName>();
                    while (reader.Read())
                        list.Add(new ClassName { Col1 = reader.GetString(0) });
                    allRecords = list.ToArray();
                }
            }
            if (list.Count != 0)
            {
                AddData adddata = new AddData(allRecords, tabName);
                adddata.Show();
            }
            else
            {
                MessageBox.Show("Выберите таблицу");
            }
        }

        void ButtonProc(string nameProg, string[] arr)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            con.Open();
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            using (SqlCommand cmd = new SqlCommand(nameProg, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (nameProg == "del_docum_data")
                {
                    param1 = new SqlParameter("@title", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@id_document", SqlDbType.Int);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                }
                else
                {
                    param1 = new SqlParameter("@id", SqlDbType.Int);
                    param1.Value = arr[0];
                }

                cmd.Parameters.Add(param1);
                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
           
                DataRowView drv = (DataRowView)dataGrid.SelectedItem;
                List<string> result = new List<string>();
            try
            {
                result.Add(drv[0].ToString());
                if (tabName == "Services")
                {
                    ButtonProc("del_service", result.ToArray());
                }
                else if (tabName == "requests_documents")
                {
                    ButtonProc("del_req_docum", result.ToArray());
                }
                else if (tabName == "records_on_consultations")
                {
                    ButtonProc("del_record_cons", result.ToArray());
                }
                else if (tabName == "documents_data")
                {
                    result.Add(drv[1].ToString());
                    ButtonProc("del_docum_data", result.ToArray());
                }
                else if (tabName == "Documents")
                {
                    DropTable(drv[1].ToString());
                    ButtonProc("del_docum", result.ToArray());
                }
                else if (tabName == "Consultations")
                {
                    ButtonProc("del_cons", result.ToArray());
                }
                else if (tabName == "clients_servises")
                {
                    ButtonProc("del_client_servis", result.ToArray());
                }
                else if (tabName == "Clients")
                {
                    ButtonProc("del_client", result.ToArray());
                }
                else if (tabName == "working_staff")
                {
                    ButtonProc("del_working_staff", result.ToArray());
                }

                else 
                {
                        con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                        Delete();
                    
                }
            }
            catch (System.NullReferenceException) { MessageBox.Show("Выберите строку"); }
           
            try
            {
                var target2 = (tabs.SelectedItem as TabItem).Header;
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                tabName = target2.ToString();
                getTableInDataGrid(target2.ToString());
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Выберите таблицу");
            }
        }
        
        void Delete()
        {
            con.Open();
            DataTable dt = new DataTable();
            dt = ((DataView)dataGrid.ItemsSource).ToTable();
            string qry = @"select * from " + tabName;
            string upd = CreateDeleteSelect();
            SqlConnection conn = con;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(qry, conn);

            DataSet ds = new DataSet();
            da.Fill(ds, tabName);

            DataTable dt2 = ds.Tables[tabName];
            var cellInfo = dataGrid.SelectedIndex;
            int row = dataGrid.SelectedIndex;
            
            SqlCommand cmd = new SqlCommand(upd, conn);

            cmd.Parameters.Add("@id", SqlDbType.Int, 4, "id").Value = Convert.ToInt32(dt2.Rows[row][0].ToString());
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void DropTable(string Docum)
        {
            string sqlsc;
            sqlsc = "DROP TABLE [" + Docum + "];";

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sqlsc, cn))
            {

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        private void AddClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clients client = new Clients();
            AddClient addclient = new AddClient(client, (sender as Label).Content.ToString());
            addclient.Show();
        }

        private void label1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            requests_documents reqDoc = new requests_documents();
            AddClient addclient = new AddClient(reqDoc, (sender as Label).Content.ToString());
            addclient.Show();
        }

        private void AddConsultation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            records_on_consultations recCons = new records_on_consultations();
            AddClient addclient = new AddClient(recCons, (sender as Label).Content.ToString());
            addclient.Show();
        }

        private void AddService_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            clients_servises clientService = new clients_servises();
            AddClient addclient = new AddClient(clientService, (sender as Label).Content.ToString());
            addclient.Show();
        }

        private void AddDocumOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            documents_data documData = new documents_data();
            AddClient addclient = new AddClient(documData, (sender as Label).Content.ToString());
            addclient.Show();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count - 1 > dataGrid.SelectedIndex)
            {
                dataGrid.SelectedIndex++;
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex > 0)
            {
                dataGrid.SelectedIndex--;
            }
        }

        private void ShowService_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Consultation.xaml", UriKind.Relative));
        }

        private void lblPrev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));
        }

        private void Row_Click(object sender, MouseButtonEventArgs e)
        {
        }
       
        string CreateUpdateSelect()
        {
            string str = "UPDATE " + tabName + " SET ";
            object obj = tabName as object;
            if (obj.ToString() != "documents_data")
            {
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    if (col.ColumnName.ToString() != "id")
                    {
                        str += " " + col.ColumnName.ToString() + " = @" + col.ColumnName.ToString() + ", ";
                    }
                }
                int ind = str.LastIndexOf(",");
                str = str.Remove(ind, 1);
                str += " WHERE id = @id";
            }
            return str;
        }
        void EditTable()
        {
            con.Open();
            DataTable dt = new DataTable();
            dt = ((DataView)dataGrid.ItemsSource).ToTable();
            string qry = @"select * from " + tabName;
            string upd = CreateUpdateSelect();
            SqlConnection conn = con;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(qry, conn);

            DataSet ds = new DataSet();
            da.Fill(ds, tabName);

            DataTable dt2 = ds.Tables[tabName];
            var cellInfo = dataGrid.SelectedIndex;
            int row = dataGrid.SelectedIndex;

            dt2.Rows[row][dgc.Header.ToString()] = cht;

            SqlCommand cmd = new SqlCommand(upd, conn);
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (col.ColumnName.ToString() != "id")
                {
                    if (col.DataType.Name == "String")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.NVarChar, 100, col.ColumnName.ToString());
                    if (col.DataType.Name == "DateTime")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.DateTime, 12, col.ColumnName.ToString());
                    if (col.DataType.Name == "Int32")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Int, 12, col.ColumnName.ToString());
                    if (col.DataType.Name == "Boolean")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Bit, 12, col.ColumnName.ToString());
                    if (col.DataType.Name == "Double")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Float, 12, col.ColumnName.ToString());
                    if (col.DataType.Name == "Decimal")
                        cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Money, 12, col.ColumnName.ToString());
                }
            }
            SqlParameter parm = cmd.Parameters.Add("@id", SqlDbType.Int, 4, "id");
            parm.SourceVersion = DataRowVersion.Original;
            da.UpdateCommand = cmd;
            da.Update(ds, tabName);
        }

        private void dataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            EditTable();
        }
        
        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox t = e.EditingElement as TextBox;  
                dgc = e.Column;
                cht = t.Text;
            }
            catch (System.NullReferenceException)
            {
                CheckBox ch = e.EditingElement as CheckBox;
                dgc = e.Column;
                cht = ch.IsChecked.ToString();
            }
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var target2 = (tabs.SelectedItem as TabItem).Header;
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                tabName = target2.ToString();
                getTableInDataGrid(target2.ToString());
                listBox.Items.Clear();
                ShowTabelsName();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Выберите таблицу");
            }

        }
    }
}
