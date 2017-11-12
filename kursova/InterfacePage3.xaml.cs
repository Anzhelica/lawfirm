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
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;

namespace kursova
{
    /// <summary>
    /// Interaction logic for InterfacePage3.xaml
    /// </summary>
    public partial class InterfacePage3 : System.Windows.Controls.Page
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        SqlConnection con;
        string result = "";
        string Text = "";
        public InterfacePage3()
        {
            InitializeComponent();
        }
        private void Select(string str)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = str;
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    DataSet dt = new DataSet();
                    adapter.Fill(dt);
                    result= dt.Tables[0].Rows[0]["Count"].ToString();
                }
            }
        }
        private void expander1_Expanded(object sender, RoutedEventArgs e)
        {
            Select("SELECT  COUNT( DISTINCT id_client) AS 'Count' FROM clients_servises");
            TextBox tb = new TextBox();
            Text = "Количество клиентов на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
            tb.IsReadOnly = true;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.AcceptsReturn = true;
            tb.Visibility = Visibility.Hidden;
            panExp1.Children.Add(tb);
        }
        private void btnShow1_Click(object sender, RoutedEventArgs e)
        {
            Select("SELECT  COUNT( DISTINCT id_client) AS 'Count' FROM clients_servises");
            TextBox tb = (panExp1.Children[3] as TextBox);
            tb.Visibility = Visibility.Visible;
            Text = "Количество клиентов на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
        }
        private void CreateDocument(string name, string text,string text2, int column, int rows)
        {
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
            
                winword.Visible = true;
                object missing = System.Reflection.Missing.Value;
            
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            
                foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
                {
                    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                    headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                    headerRange.Font.Size = 12;
                    headerRange.Text = name;
                }
                
                Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
            para1.Range.Font.Size = 14;
            para1.Range.Text = text;
                para1.Range.InsertParagraphAfter();
            
               Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
          
            para2.Range.Font.Size = 14;
            para2.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            para2.Range.Text = text2;
              para2.Range.InsertParagraphAfter();

                Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(para1.Range, rows, column, ref missing, ref missing);

                firstTable.Borders.Enable = 1;
                foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        if (cell.RowIndex == 1)
                        {
                            cell.Range.Text =  dt.Columns[cell.ColumnIndex-1].ColumnName.ToString();
                            cell.Range.Font.Bold = 1;
                            cell.Range.Font.Name = "calibri";
                            cell.Range.Font.Size = 14;
                            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                        }
                        else
                        {
                        try
                        {
                            try
                            {
                                cell.Range.Font.Size = 14;
                                cell.Range.Text = (dt.Rows[cell.RowIndex - 2] as DataRow).Field<string>(Convert.ToString(dt.Columns[cell.ColumnIndex - 1].ToString()).ToString());
                            }
                            catch (System.IndexOutOfRangeException) { cell.Range.Text = ""; }
                            } 
                        catch (System.InvalidCastException)
                        {
                            cell.Range.Font.Size = 12;
                            cell.Range.Text = (dt.Rows[cell.RowIndex - 2] as DataRow).Field<int>(cell.ColumnIndex - 1).ToString();
                        }
                        }
                    }
                }
           
        }

        private void expander2_Expanded(object sender, RoutedEventArgs e)
        {
            Select("SELECT  COUNT( DISTINCT id) AS 'Count' FROM working_staff");
            TextBox tb = new TextBox();
            Text = "Количество работников на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
            tb.IsReadOnly = true;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.AcceptsReturn = true;
            tb.Visibility = Visibility.Hidden;
            panExp2.Children.Add(tb);
        }
        private void btnShow2_Click(object sender, RoutedEventArgs e)
        {
            Select("SELECT  COUNT( DISTINCT id) AS 'Count' FROM working_staff");
            TextBox tb = (panExp2.Children[3] as TextBox);
            tb.Visibility = Visibility.Visible;
            Text = "Количество работников на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
        }
        private void btnOpen1_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT Clients.id, Clients.Surname, Clients.Name, Clients.Patronymic, " +
                    "Clients.Citizenship from clients inner join clients_servises on clients_servises.id_client = clients.id";
            getColumn("Clients",sql);
            TextBlock tb = ((btnOpen1.Parent as StackPanel).Children[0] as TextBlock);
            CreateDocument(tb.Text,Text,"Подробная информация о клиентах",dt.Columns.Count,Convert.ToInt32(result)+1);
        }

        
        private void btnOpen2_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT Surname, Name, Patronymic from working_staff";
            getColumn("working_staff", sql);
            TextBlock tb = ((btnOpen2.Parent as StackPanel).Children[0] as TextBlock);
            CreateDocument(tb.Text, Text, "Подробная информация о штате работников", dt.Columns.Count, Convert.ToInt32(result) + 1);
        }

        private void getColumn(string tabName, string sql)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    DataSet dta = new DataSet();
                    adapter.Fill(dta);
                    dt = dta.Tables[0];
                    System.Data.DataTable result = new System.Data.DataTable();

                }
            }
        }
        private void expander3_Expanded(object sender, RoutedEventArgs e)
        {
            Select2("select top 1 id_working_staff,working_staff.Name,working_staff.Surname, working_staff.Patronymic, " +
                "count(id_working_staff) as 'Count' from clients_servises " +
"inner join working_staff on clients_servises.id_working_staff = working_staff.id " +
"group by id_working_staff, working_staff.Name, working_staff.Surname, working_staff.Patronymic " +
"order by count(id_working_staff) desc");
            TextBox tb = new TextBox();
            Text = "Популярный работник на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
            tb.IsReadOnly = true;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.AcceptsReturn = true;
            tb.Visibility = Visibility.Hidden;
            panExp3.Children.Add(tb);
        }
        private void btnShow3_Click(object sender, RoutedEventArgs e)
        {
            Select2("select top 1 id_working_staff,working_staff.Name,working_staff.Surname, working_staff.Patronymic, " +
                "count(id_working_staff) as 'Count' from clients_servises " +
"inner join working_staff on clients_servises.id_working_staff = working_staff.id " +
"group by id_working_staff, working_staff.Name, working_staff.Surname, working_staff.Patronymic " +
"order by count(id_working_staff) desc");
            TextBox tb = (panExp3.Children[3] as TextBox);
            tb.Visibility = Visibility.Visible;
            Text = "Популярный работник на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
        }
        private void Select2(string str)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = str;
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    DataSet dt = new DataSet();
                    adapter.Fill(dt);
                    result =
                       dt.Tables[0].Rows[0]["Name"].ToString() +" "+
                       dt.Tables[0].Rows[0]["Surname"].ToString() + " " +
                       dt.Tables[0].Rows[0]["Patronymic"].ToString()+" с количеством обращений  "+ dt.Tables[0].Rows[0]["Count"].ToString();
                }
            }
        }

        private void btnOpen3_Click(object sender, RoutedEventArgs e)
        {
            Select("select count(id) as 'Count' from working_staff");
            string sql = "select  working_staff.id,working_staff.Name,working_staff.Surname, " +
                "working_staff.Patronymic, count(id_working_staff) as 'Count' " +
                "from working_staff left outer join  clients_servises on " +
                "clients_servises.id_working_staff = working_staff.id group by working_staff.id, " +
"working_staff.Name, working_staff.Surname, working_staff.Patronymic";
            getColumn("working_staff", sql);
            TextBlock tb = ((btnOpen3.Parent as StackPanel).Children[0] as TextBlock);
            CreateDocument(tb.Text, Text, "Подробная информация о популярности работников", dt.Columns.Count, Convert.ToInt32(result) + 1);
        }
        private void Select3(string str)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = str;
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    DataSet dt = new DataSet();
                    adapter.Fill(dt);
                    result = " "+
                       dt.Tables[0].Rows[0]["document_type"].ToString()+ " с количеством заказов "+dt.Tables[0].Rows[0]["Count"].ToString() ;
                }
            }
        }
        private void expander4_Expanded(object sender, RoutedEventArgs e)
        {
            Select3("select top 1 Documents.id, Documents.document_type, " +
                "count(requests_documents.id_document) as 'Count' from Documents " +
"inner join requests_documents on Documents.id = requests_documents.id_document " +
"group by Documents.id, Documents.document_type" +
" order by count(requests_documents.id_document) desc");
            TextBox tb = new TextBox();
            Text = "Популярный документ на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
            tb.IsReadOnly = true;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.AcceptsReturn = true;
            tb.Visibility = Visibility.Hidden;
            panExp4.Children.Add(tb);
        }
        private void btnShow4_Click(object sender, RoutedEventArgs e)
        {
            Select3("select top 1 Documents.id, Documents.document_type, " +
                "count(requests_documents.id_document) as 'Count' from Documents " +
"inner join requests_documents on Documents.id = requests_documents.id_document " +
"group by Documents.id, Documents.document_type" +
" order by count(requests_documents.id_document) desc");
            TextBox tb = (panExp4.Children[3] as TextBox);
            tb.Visibility = Visibility.Visible;
            Text = "Популярный документ на " + DateTime.Now.Day.ToString() + " число " + DateTime.Now.Month.ToString() + " месяца " + DateTime.Now.Year.ToString() + " года - " + result + " ";
            tb.Text = Text;
        }

        private void btnOpen4_Click(object sender, RoutedEventArgs e)
        {
            Select("select count(id_document) as 'Count' from requests_documents");
            string sql = "select Documents.id, Documents.document_type, " +
                "count(requests_documents.id_document) as 'Count' from Documents " +
"inner join requests_documents on Documents.id = requests_documents.id_document " +
"group by Documents.id, Documents.document_type" +
" order by count(requests_documents.id_document) desc";
            getColumn("Documents", sql);
            TextBlock tb = ((btnOpen4.Parent as StackPanel).Children[0] as TextBlock);
            CreateDocument(tb.Text, Text, "Подробная информация о популярности документов", dt.Columns.Count, Convert.ToInt32(result) + 1);
        }

        private void txtFIO_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            
            e.Handled = true;
            IList<string> FIO = txtFIO.Text.Split(' ').ToList<string>();
            string[] arr = FIO.ToArray();
            FindFIO(arr[0], arr[1], arr[2]);
        }
        void FindFIO(string f, string i, string o)
        {
            var list = new List<ClassName>();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                string sql = "SELECT Clients.Surname,Clients.Name,Clients.Patronymic,clients_servises.date,Services.description,Clients.Id  FROM clients_servises inner join Clients on Clients.id = clients_servises.id_client " +
                    "inner join Services on Services.id = clients_servises.id_service " +
                    " where Clients.Surname='"+f+
                    "' AND Clients.Name ='"+i+ "' AND Clients.Patronymic = '"+o+"'";
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                   
                        list = new List<ClassName>();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0).ToString() + reader.GetString(1).ToString() + reader.GetString(2).ToString() + Convert.ToString(reader.GetDateTime(3).ToString()) + reader.GetString(4).ToString();
                            ComboBoxItem item = new ComboBoxItem();
                            item.Content = result;
                            item.Tag = reader.GetInt32(5);
                            EnterSelect.Items.Add(item);
                        }

                    }
                  
                }
            }
        }
        private void SelectedItem(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            
            e.Handled = true;
            IList<string> FIO = txtFIO.Text.Split(' ').ToList<string>();
            string[] arr = FIO.ToArray();
            FindFIO(arr[0], arr[1], arr[2]);
        }

        private void EnterSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)EnterSelect.SelectedValue;
            string value = typeItem.Tag.ToString();

            string sql = "SELECT Clients.Surname,Clients.Name,Clients.Patronymic,Services.description, working_staff.Surname, working_staff.Name,working_staff.Patronymic,clients_servises.date,clients_servises.price  FROM clients_servises inner join Clients on Clients.id = clients_servises.id_client " +
                    "inner join Services on Services.id = clients_servises.id_service " +
                    "inner join working_staff on clients_servises.id_working_staff = working_staff.id " +
                    " where Clients.id = "+value;
            getColumn("Clients", sql);
            CreateCheck("Чек", "");

        }

        private void CreateCheck(string name, string text)
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

            winword.Visible = true;
            
            object missing = System.Reflection.Missing.Value;
            
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            
            foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
            {
                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                headerRange.Font.Size = 14;
                headerRange.Text = name;
               
            }
            
            Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
            para1.Range.Font.Size = 14;
            para1.Range.Text = text;
            para1.Range.InsertParagraphAfter();
            Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
            para2.Range.Font.Size = 14;
            string s = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i < 3)
                {
                    try
                    {
                        s += (dt.Rows[0] as DataRow).Field<string>(Convert.ToString(dt.Columns[i].ToString()).ToString())+" ";
                    }
                    catch (System.InvalidCastException)
                    {
                        s += dt.Columns[i].ToString() + " " + (dt.Rows[0] as DataRow).Field<int>(dt.Columns[i].ToString()).ToString();
                    }

                }
                if (i == 3)
                {
                    para2.Range.Text += "Клиент:  "+ s;
                    s = "";
                    para2.Range.InsertParagraphAfter();
                    Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);

                    para3.Range.Text += "Предоставленная услуга:  " + (dt.Rows[0] as DataRow).Field<string>(Convert.ToString(dt.Columns[i].ToString()).ToString());
                    para3.Range.Font.Size = 14;
                    para3.Range.InsertParagraphAfter();
                }
                
                if(i>3 && i < 7)
                {
                    

                    s +=  (dt.Rows[0] as DataRow).Field<string>(Convert.ToString(dt.Columns[i].ToString()).ToString())+" ";

                    
                }
                if (i == 7)
                {
                    Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
                    para3.Range.Text += "Обслуживал:  " + s;
                    para3.Range.Font.Size = 14;
                    para3.Range.InsertParagraphAfter();
                    para3 = document.Content.Paragraphs.Add(ref missing);

                    para3.Range.Text += "Дата:  " + (dt.Rows[0] as DataRow).Field<DateTime>(Convert.ToString(dt.Columns[i].ToString()).ToString());
                    para3.Range.Font.Size = 14;
                    para3.Range.InsertParagraphAfter();
                }
                if (i == 8)
                {
                    Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
                    var t = ((Convert.ToDouble((dt.Rows[0] as DataRow)[i])));
        para3.Range.Text += "Цена:  " + t.ToString() +" грн";
                    para3.Range.Font.Size = 14;
                    para3.Range.InsertParagraphAfter();
                }
            }
        }

        private void label2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/mainPage.xaml", UriKind.Relative));
        }

    }
}
