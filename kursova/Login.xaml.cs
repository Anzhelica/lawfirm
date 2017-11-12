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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SqlConnection con;
        public Login()
        {
            InitializeComponent();
        }
       // Registration registration = new Registration();
        MainWindow welcome = new MainWindow();
       public static  string role = "";
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an name.";
                textBoxEmail.Focus();
            }
            
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Registration where NameEnter='" + email + "'  and password='" + password + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    string username = dataSet.Tables[0].Rows[0]["NameEnter"].ToString() + " " + dataSet.Tables[0].Rows[0]["password"].ToString();
                    role = dataSet.Tables[0].Rows[0]["NameEnter"].ToString();
                    Close();
                    con.Dispose();
                  //  EnterAsRole(role);
                    // welcome.TextBlockName.Text = username;//Sending value from one form to another form.
                    welcome.Show();
                    //Close();
                }
                else
                {
                    errormessage.Text = "Sorry! Please enter existing name/password.";
                }
                con.Close();
            }
        }
        void EnterAsRole(string role)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);

            con.Open();
            SqlCommand cmd = new SqlCommand("  Execute as USER  = '" + role + "'");
            cmd.Connection = con as SqlConnection;
          
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }
        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
         //  registration.Show();
          //  Close();
        }

    }
}