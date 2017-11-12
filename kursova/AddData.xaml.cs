using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace kursova
{
    /// <summary>
    /// Interaction logic for AddData.xaml
    /// </summary>
    public class ButtonsList
    {
        public ComboBox cb { get; set; }
    }
    public partial class AddData : Window
    {
        SqlConnection con;
        DataSet ds;
        string TabName = "";
        public AddData(object[] arr, string tabName)
        {
            InitializeComponent();
            lbldesk.Content += " "+tabName;
            TabName = tabName;
            DataTable dt = new DataTable();
            DataColumn column;
            
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "S1";
            column.ReadOnly = true;
            

            dt.Columns.Add(column);

            column = new DataColumn();
         
            column.DataType = Type.GetType("System.String");
           column.ColumnName = "S2";
            dt.Columns.Add(column);

            foreach (var t in arr)
            {
                if ((t as ClassName).Col1 != "id")
                {
                    string str = (t as ClassName).Col1.ToString().Substring(0, 3);
                    DataRow row = dt.NewRow();

                    row["S1"] = (t as ClassName).Col1;
                    dt.Rows.Add(row);
                }
            }
            dataGrid1.ItemsSource = dt.DefaultView;
            dataGrid1.CanUserAddRows = false;
        }
        void ButtonProc(string nameProg, string [] arr)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            con.Open();
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            using (SqlCommand cmd = new SqlCommand(nameProg, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (nameProg == "ins_doc")
                {
                    param1 = new SqlParameter("@document_type", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                }
                if (nameProg == "ins_cons")
                {
                    param1 = new SqlParameter("@name", SqlDbType.NVarChar);
                    param1.Value = arr[0];

                }
                if (nameProg == "ins_doc_data")
                {
                    param1 = new SqlParameter("@title", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@id", SqlDbType.Int);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                }
                if (nameProg == "ins_records_on_cons")
                {
                    param1 = new SqlParameter("@id_client", SqlDbType.Int);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@id_working_staff", SqlDbType.Int);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@date", SqlDbType.Date);
                    param2.Value = arr[2];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@id_consultations", SqlDbType.Int);
                    param2.Value = arr[3];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@finished", SqlDbType.Bit);
                    param2.Value = arr[4];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@additional_text", SqlDbType.Text);
                    param2.Value = arr[5];
                    cmd.Parameters.Add(param2);
                
                }
                if (nameProg == "ins_req_doc")
                {
                    param1 = new SqlParameter("@id_clients", SqlDbType.Int);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@id_servises", SqlDbType.Int);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@id_document", SqlDbType.Int);
                    param2.Value = arr[2];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@data", SqlDbType.Date);
                    param2.Value = arr[3];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@status", SqlDbType.Bit);
                    param2.Value = arr[4];
                    cmd.Parameters.Add(param2);
                }
                if (nameProg == "ins_serv")
                {
                    param1 = new SqlParameter("@description", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                }
                if (nameProg == "ins_work_staff")
                {
                    param1 = new SqlParameter("@surname", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@name", SqlDbType.NVarChar);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@patronymic", SqlDbType.NVarChar);
                    param2.Value = arr[2];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@post", SqlDbType.NVarChar);
                    param2.Value = arr[3];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@salary", SqlDbType.Money);
                    param2.Value = arr[4];
                    cmd.Parameters.Add(param2);
                }
                if (nameProg == "ins_client")
                {
                    param1 = new SqlParameter("@surname", SqlDbType.NVarChar);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@name", SqlDbType.NVarChar);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@Patronymic", SqlDbType.NVarChar);
                    param2.Value = arr[2];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@Citizenship", SqlDbType.NVarChar);
                    param2.Value = arr[3];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@birthdate", SqlDbType.Date);
                    param2.Value = arr[4];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@identification_number", SqlDbType.Int);
                    param2.Value = arr[5];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@passport_id", SqlDbType.Int);
                    param2.Value = arr[6];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@passport_series", SqlDbType.Char);
                    param2.Value = arr[7];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@residential_address", SqlDbType.NVarChar);
                    param2.Value = arr[8];
                    cmd.Parameters.Add(param2);
                }
                if (nameProg == "ins_client_serv")
                {
                    param1 = new SqlParameter("@id_client", SqlDbType.Int);
                    param1.Value = arr[0];
                    param2 = new SqlParameter("@id_service", SqlDbType.Int);
                    param2.Value = arr[1];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@date", SqlDbType.Date);
                    param2.Value = arr[2];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@price", SqlDbType.Money);
                    param2.Value = arr[3];
                    cmd.Parameters.Add(param2);
                    param2 = new SqlParameter("@id_working_staff", SqlDbType.Int);
                    param2.Value = arr[4];
                    cmd.Parameters.Add(param2);
                }
                cmd.Parameters.Add(param1);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Вы не заполнили все необходимые поля верно");
                }
            }
            con.Close();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            foreach (DataRowView t in dataGrid1.Items)
            {
                list.Add(t["S2"].ToString());
            }
            string[] arr = list.ToArray();
            if (TabName == "Documents")
            {
                AddDocumentData addDocumData = new AddDocumentData(arr[0]);
                addDocumData.Show();

            }
            else if (TabName == "Consultations")
                ButtonProc("ins_cons", arr);
            else if (TabName == "documents_data")
                ButtonProc("ins_doc_data", arr);
            else if (TabName == "records_on_consultations")
            {
                arr[5] = (arr[5]).ToString();
            ButtonProc("ins_records_on_cons", arr);
             }
            else if (TabName == "requests_documents")
                ButtonProc("ins_req_doc", arr);
            else if (TabName == "Services")
                ButtonProc("ins_serv", arr);
            else if (TabName == "working_staff")
                ButtonProc("ins_work_staff", arr);
            else if (TabName == "Clients")
                ButtonProc("ins_client", arr);
            else if (TabName == "clients_servises")
                ButtonProc("ins_client_serv", arr);

            else
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                TableInDataGrid();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                Insert(arr);
            }
        }
        string CreateInsertSelect()
        {
            string str = "INSERT INTO " + TabName + "( ";
            object obj = TabName as object;
           
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    if (col.ColumnName.ToString() != "id")
                    {
                        str += " " + col.ColumnName.ToString() + ", ";
                    }
                }
                int ind = str.LastIndexOf(",");
                str = str.Remove(ind, 1);
                str += ") values(";
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (col.ColumnName.ToString() != "id")
                {
                    str += " @" + col.ColumnName.ToString() + ", ";
                }
            }
            ind = str.LastIndexOf(",");
            str = str.Remove(ind, 1);
            str += ")";
            return str;
        }
        void Insert(string[]arr)
        {
            con.Open();
            DataTable dt = new DataTable();
            string qry = @"select * from " + TabName;
            string upd = CreateInsertSelect();
            SqlConnection conn = con;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(qry, conn);

            DataSet ds = new DataSet();
            da.Fill(ds, TabName);

            DataTable dt2 = ds.Tables[TabName];

            SqlCommand cmd = new SqlCommand(upd, conn);
            int i = 0;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                
                    if (col.ColumnName.ToString() != "id")
                    {
                    if (col.ColumnName.ToString() != "id_requests")
                        i++;
                        if (col.DataType.Name == "String")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.NVarChar, 100, col.ColumnName.ToString()).Value = arr[i];
                        if (col.DataType.Name == "DateTime")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.DateTime, 12, col.ColumnName.ToString()).Value = arr[i];
                        if (col.DataType.Name == "Int32")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Int, 12, col.ColumnName.ToString()).Value = arr[i];
                        if (col.DataType.Name == "Boolean")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Bit, 12, col.ColumnName.ToString()).Value = arr[i];
                        if (col.DataType.Name == "Double")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Float, 12, col.ColumnName.ToString()).Value = arr[i];
                        if (col.DataType.Name == "Decimal")
                            cmd.Parameters.Add("@" + col.ColumnName.ToString(), SqlDbType.Money, 12, col.ColumnName.ToString()).Value = arr[i];

                
                }
            }
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private void TableInDataGrid()
        {
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = "SELECT * from [" + TabName + "]";
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    ds = new DataSet();
                    adapter.Fill(ds, TabName);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    
                }
            }
        }
    }
}

