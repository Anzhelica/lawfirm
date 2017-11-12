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
using System.Windows.Shapes;

namespace kursova
{
    /// <summary>
    /// Interaction logic for AddDocumentData.xaml
    /// </summary>
    public partial class AddDocumentData : Window
    {
        public string documentType = "";
        public string documentId = "";
        SqlConnection con;
        public AddDocumentData(string documType)
        {
            InitializeComponent();
            documentType = documType;
        }

        void ButtonProc( string[] arr, string id)
        { foreach (string str in arr)
                {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            con.Open();
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            using (SqlCommand cmd = new SqlCommand("ins_doc_data", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               
                    param1 = new SqlParameter("@id", SqlDbType.Int);
                    param1.Value = id;
                    param2 = new SqlParameter("@title", SqlDbType.NVarChar);
                    param2.Value = str;
                    cmd.Parameters.Add(param1);
                    cmd.Parameters.Add(param2);

                    cmd.ExecuteNonQuery();
                }
            }

            con.Close();
        }
        void getId(string Docum)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("select * from Documents where document_type = '" + Docum+"'", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Documents");

            foreach (DataRow t in (ds.Tables[0].Rows))
            {
                documentId  = t[0].ToString();
            }
        }
        private void InsertDoc(string Docum)
        {
            string query = "INSERT INTO Documents (document_type) " +
                           "VALUES (@document_type) ";
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {

                cmd.Parameters.Add("@document_type", SqlDbType.NVarChar, 100).Value = Docum;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
       
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string st = textBox.Text.Replace(" ", string.Empty);
            IList<string> names = st.Split(';').ToList<string>();
            
            InsertDoc(documentType);
            getId(documentType);
          
            ButtonProc(names.ToArray(), documentId);
            CreateTable(documentType, names.ToArray());

        }

        private void CreateTable(string Docum, string [] arr)
        {
            string sqlsc;
            sqlsc =  "CREATE TABLE " + Docum + "(";
            
                sqlsc += "\n [id] int IDENTITY(1,1) PRIMARY KEY NOT NULL, id_requests int, ";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i != arr.Length - 1 && arr[i] != "")
                {
                    if(arr[i] == "текст" || arr[i] == "Текст")
                        sqlsc += "\n [" + arr[i] + "] text, ";
                    else
                    sqlsc += "\n [" + arr[i] + "] nvarchar(100), ";
                }
                else
                {
                    if (arr[i] != "")
                    {
                        if ((arr[i] == "текст" || arr[i] == "Текст"))
                            sqlsc += "\n [" + arr[i] + "] text \n );";
                        else
                            sqlsc += "\n [" + arr[i] + "] nvarchar(100) \n );";
                    }
                    else
                    {
                        int ind = sqlsc.LastIndexOf(",");
                        sqlsc = sqlsc.Remove(ind, 1);
                        sqlsc += " \n );";
                    }
                }
            }

            
        using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sqlsc, cn))
            {
             
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
