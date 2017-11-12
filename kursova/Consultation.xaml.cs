using Microsoft.Office.Interop.Word;
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
    /// Interaction logic for Consultations.xaml
    /// </summary>
    public partial class Consultation : System.Windows.Controls.Page
    {
        SqlConnection con;
        string result = "";
        System.Data.DataTable dt = new System.Data.DataTable();
        DataSet ds;
        public Consultation()
        {
            InitializeComponent();
            dataGrid.CanUserAddRows = false;
            dataGrid1.CanUserAddRows = false;
            string sql = "SELECT Consultations.name as 'Название', records_on_consultations.date as 'Дата проведения'"+
                ", working_staff.Surname as 'Проводит', working_staff.Name,  working_staff.Patronymic from Consultations  inner join records_on_consultations" +
                " on Consultations.id = records_on_consultations.id_consultations inner join working_staff on records_on_consultations.id_working_staff = "+
                " working_staff.id  WHERE records_on_consultations.date  >= '"+DateTime.Now+ "' GROUP BY  Consultations.name, records_on_consultations.date, "+
                "working_staff.Surname, working_staff.Name,working_staff.Patronymic ORDER BY records_on_consultations.date";
            getTableInDataGrid(sql,dataGrid);
            sql = "SELECT Consultations.name as 'Название', records_on_consultations.date as 'Дата проведения'" +
                 ", working_staff.Surname as 'Проводит', working_staff.Name,  working_staff.Patronymic from Consultations  inner join records_on_consultations" +
                 " on Consultations.id = records_on_consultations.id_consultations inner join working_staff on records_on_consultations.id_working_staff = " +
                 " working_staff.id  WHERE records_on_consultations.date  < '" + DateTime.Now + "' GROUP BY  Consultations.name, records_on_consultations.date, " +
                 "working_staff.Surname, working_staff.Name,working_staff.Patronymic ORDER BY records_on_consultations.date";
            getTableInDataGrid(sql, dataGrid1);
            sql = "SELECT Consultations.name as 'Название', records_on_consultations.date as 'Дата проведения'" +
                 ", working_staff.Surname as 'Проводит', working_staff.Name,  working_staff.Patronymic from Consultations  inner join records_on_consultations" +
                 " on Consultations.id = records_on_consultations.id_consultations inner join working_staff on records_on_consultations.id_working_staff = " +
                 " working_staff.id  GROUP BY  Consultations.name, records_on_consultations.date, " +
                 "working_staff.Surname, working_staff.Name,working_staff.Patronymic ORDER BY records_on_consultations.date";
            getData(sql);



        }
        private void getData(string sql)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            using (SqlConnection sc = con)
            {
                sc.Open();
                SqlCommand com = new SqlCommand(sql, sc);
                using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                {
                    //   DataSet dta = new DataSet();
                    //   adapter.Fill(dta);
                    //   dt = dta.Tables[0];
                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    adapter.Fill(dt2);
                    int i = 0;
                    DataGridTextColumn dc2 = new DataGridTextColumn();
                    System.Data.DataTable dt33 = new System.Data.DataTable();

                    dt33.Columns.Add("Название");
                    dt33.Columns.Add("Дата проведения");
                    dt33.Columns.Add("Проводит");
                    foreach (DataRow t in dt2.Rows)
                    {
                        var row = dt33.NewRow();
                        row["Название"] = t[0].ToString();
                        row["Дата проведения"] = t[1].ToString();
                        row["Проводит"] = t[2].ToString() + " " + t[3].ToString() + " " + t[4].ToString();
                        dt33.Rows.Add(row);

                        // dg.ItemsSource = dt2.DefaultView;
                    }
                    dt = dt33;
                }
            }
        }
        private void getTableInDataGrid(string sql, DataGrid dg)
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
                 //   dt = dta.Tables[0];

                    System.Data.DataTable dt2 = new System.Data.DataTable();
                    adapter.Fill(dt2);
                    int i = 0;
                    DataGridTextColumn dc2 = new DataGridTextColumn();
                    System.Data.DataTable dt33 = new System.Data.DataTable();
                  
                    dt33.Columns.Add("Название");
                    dt33.Columns.Add("Дата проведения");
                    dt33.Columns.Add("Проводит");
                    foreach (DataRow t in dt2.Rows)
                    {
                        var row = dt33.NewRow();
                        row["Название"] = t[0].ToString();
                        row["Дата проведения"] = t[1].ToString();
                        row["Проводит"] = t[2].ToString()+" " + t[3].ToString()+" " + t[4].ToString();
                        dt33.Rows.Add(row);
                        
                       // dg.ItemsSource = dt2.DefaultView;
                    }
                    dt = dt33;
                    dg.ItemsSource = dt33.DefaultView;
                }
            }
        }
        private void label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("/InterfacePage1.xaml", UriKind.Relative));
        }
        private void CreateDocument(string name, int column, int rows)
        {
            string s = "";
            //Create an instance for word app
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
            winword.Visible = true;
            object missing = System.Reflection.Missing.Value;
            //Create a new document
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            //Add header into the document
            foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
            {
                //Get the header range and add the header details.
                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
                headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
                headerRange.Font.Size = 10;
                headerRange.Text = name;
            }
            
            ////adding text to document
            document.Content.SetRange(0, 0);
            document.Content.Text =name + Environment.NewLine;

            //Add paragraph with Heading 1 style
           Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
            //  object styleHeading1 = "Heading 1";
           // //   para1.Range.set_Style(ref styleHeading1);
         //   para1.Range.Text = text;
            para1.Range.InsertParagraphAfter();

            //  //Add paragraph with Heading 2 style
           // Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
            ////  object styleHeading2 = "Heading 2";
            ////  para2.Range.set_Style(ref styleHeading2);
          //  para2.Range.Text = text2;
           // para2.Range.InsertParagraphAfter();

            //Create a 5X5 table and insert some dummy record
            Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(para1.Range, rows, column, ref missing, ref missing);

            firstTable.Borders.Enable = 1;
            foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    //Header row
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex - 1 < 3) {
                            cell.Range.Text = dt.Columns[cell.ColumnIndex - 1].ColumnName.ToString();
                            cell.Range.Font.Bold = 1;
                            //other format properties goes here
                            cell.Range.Font.Name = "verdana";
                            cell.Range.Font.Size = 10;
                            //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                            //  cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                            //Center alignment for the Header cells
                            cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                            cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                        }

                    }
                    
                    //Data row
                    else
                    {
                        try
                        {
                             //  if(cell.ColumnIndex - 1 <2)
                                cell.Range.Text = (dt.Rows[cell.RowIndex - 2] as DataRow).Field<string>(Convert.ToString(dt.Columns[cell.ColumnIndex - 1].ToString()).ToString());
                              //  else
                               //     s+= " "+(dt.Rows[cell.RowIndex - 2] as DataRow).Field<string>(Convert.ToString(dt.Columns[cell.ColumnIndex - 1].ToString()).ToString()); 
                           
                        } //dt.Columns[cell.ColumnIndex-1].ToString();// (cell.RowIndex - 2 + cell.ColumnIndex).ToString();
                        catch (System.InvalidCastException)
                        {
                            cell.Range.Text = (dt.Rows[cell.RowIndex - 2] as DataRow).Field<DateTime>(cell.ColumnIndex - 1).ToString();
                        }
                       // if (cell.ColumnIndex - 1 == 3)
                      //  {
                      //      row.Cells[3].Range.Text = s;
                      //      s = "";
                            
                     //   }
                    }
                   
                }

            }
            // Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
            Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
            
            para2.Range.Text = "Проведено "+result+" консультаций"+ " на "+DateTime.Now.Year+"/"+DateTime.Now.Month+"/"+DateTime.Now.Day;
            para2.Range.InsertParagraphAfter();
            //Save the document
            // object filename = @"c:\Users\NEO\Desktop\do1.docx";
            // document.SaveAs2(ref filename);
            //document.Open
            //document.Close(ref missing, ref missing, ref missing);
            //  document = null;
            // winword.Quit(ref missing, ref missing, ref missing);
            //  winword = null;
            MessageBox.Show("Document created successfully !");

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
                    DataSet dt1 = new DataSet();
                    adapter.Fill(dt1);
                    result = dt1.Tables[0].Rows[0]["Count"].ToString();
                }
            }
        }
        private void btnCreateDoc_Click(object sender, RoutedEventArgs e)
        {
            Select("SELECT  COUNT(id) AS 'Count' FROM records_on_consultations");
            CreateDocument("Отчет консультаций", dt.Columns.Count, Convert.ToInt32(result) + 1);
        }

    }
}
