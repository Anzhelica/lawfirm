using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;

namespace kursova
{
    /// <summary>
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        SqlConnection con;
        string NameTab = "";
        string IdTab = "";
        string[] arr2;
        string IDClient = "";
        string typeItem = "";
        ComboBox cmb = new ComboBox();
        object objTab;
        string content = "";
        string id_req = "";
        public AddClient(object obj, string lblcontent)
        {
            InitializeComponent();
            content = lblcontent;
            objTab = obj;
            double all = 2;
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (!(propertyInfo.PropertyType.IsAbstract)&& propertyInfo.Name != "id")
                {
                    if (obj.GetType().Name == "Clients")
                    {
                        NameTab = "Clients";
                        TextBox textbox = new TextBox();
                        textbox.FontSize = 18;
                        textbox.Margin = new Thickness(all);
                        panel.Children.Add(textbox);
                        Label label = new Label();
                        label.FontSize = 18;
                        label.Content = propertyInfo.Name;
                        label.HorizontalAlignment = HorizontalAlignment.Right;
                        pannel.Children.Add(label);
                    }
                    if (obj.GetType().Name == "requests_documents")
                    {
                        NameTab = "requests_documents";
                        if (propertyInfo.Name != "data" && propertyInfo.Name != "status" && propertyInfo.Name != "Documents"
                            && propertyInfo.Name != "Clients" && propertyInfo.Name != "Services")
                        {
                            ComboBox listBox = new ComboBox();
                            listBox.Height = 30;
                            listBox.IsEditable = true;
                            listBox.FontSize = 18;
                            if (propertyInfo.Name == "id_clients")
                                fillCombo(listBox, "Clients", "Surname", "Name", "Patronymic");
                            if (propertyInfo.Name == "id_servises")
                                fillCombo(listBox, "Services", "description","","");
                            if (propertyInfo.Name == "id_document")
                                fillCombo(listBox, "Documents","document_type","","");
                            
                            listBox.Margin = new Thickness(all);
                            panel.Children.Add(listBox);

                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                        if (propertyInfo.Name == "data")
                        {
                            DatePicker datePick = new DatePicker();
                            datePick.FontSize = 18;
                            panel.Children.Add(datePick);
                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                    }

                    if (obj.GetType().Name == "records_on_consultations")
                    {
                        NameTab = "records_on_consultations";
                        if (propertyInfo.Name != "additional_text" && propertyInfo.Name != "date" && propertyInfo.Name != "finished" && propertyInfo.Name != "Consultations"
                            && propertyInfo.Name != "Clients" && propertyInfo.Name != "working_staff")
                        {
                            ComboBox listBox = new ComboBox();
                            listBox.Height = 30;
                            listBox.IsEditable = true;
                            listBox.FontSize = 18;
                            if (propertyInfo.Name == "id_client")
                                fillCombo(listBox, "Clients", "Surname","Name","Patronymic");
                            if (propertyInfo.Name == "id_working_staff")
                                fillCombo(listBox, "working_staff", "Surname", "Name", "Patronymic");
                            if (propertyInfo.Name == "id_consultations")
                                fillCombo(listBox, "Consultations", "name","","");
                            
                            listBox.Margin = new Thickness(all);
                            panel.Children.Add(listBox);

                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                        if (propertyInfo.Name == "date")
                        {
                            DatePicker datePick = new DatePicker();
                            datePick.FontSize = 18;
                            panel.Children.Add(datePick);
                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                        if (propertyInfo.Name == "additional_text")
                        {
                            TextBox datePick = new TextBox();
                            datePick.FontSize = 18;
                            datePick.Height = 100;
                            datePick.TextWrapping = TextWrapping.Wrap;
                            datePick.AcceptsReturn = true;
                            panel.Children.Add(datePick);
                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                    }

                    if (obj.GetType().Name == "clients_servises")
                    {
                        NameTab = "clients_servises";
                        if (propertyInfo.Name != "Price" && propertyInfo.Name != "date" && propertyInfo.Name != "status" && propertyInfo.Name != "Services"
                            && propertyInfo.Name != "Clients" && propertyInfo.Name != "working_staff")
                        {
                            ComboBox listBox = new ComboBox();
                            listBox.Height = 30;
                            listBox.IsEditable = true;
                            listBox.FontSize = 18;
                            if (propertyInfo.Name == "id_client")
                                fillCombo(listBox, "Clients", "Surname","Name","Patronymic");
                            if (propertyInfo.Name == "id_working_staff")
                                fillCombo(listBox, "working_staff", "Surname", "Name", "Patronymic");
                            if (propertyInfo.Name == "id_service")
                                fillCombo(listBox, "Services", "description","","");
                            
                            listBox.Margin = new Thickness(all);
                            panel.Children.Add(listBox);

                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                        if (propertyInfo.Name == "date")
                        {
                            DatePicker datePick = new DatePicker();
                            datePick.FontSize = 18;
                            panel.Children.Add(datePick);
                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                        if (propertyInfo.Name == "Price")
                        {
                            TextBox datePick = new TextBox();
                            datePick.Height = 30;
                            datePick.FontSize = 18;
                            panel.Children.Add(datePick);
                            Label label = new Label();
                            label.FontSize = 18;
                            label.Content = propertyInfo.Name;
                            label.HorizontalAlignment = HorizontalAlignment.Right;
                            pannel.Children.Add(label);
                        }
                    }
                    
                }

            }
            if (obj.GetType().Name == "documents_data")
            {
                NameTab = "documents_data";
                Label label = new Label();
                label.FontSize = 18;
                label.Content = "Выберите документ:";
                label.HorizontalAlignment = HorizontalAlignment.Right;

                pannel4.Children.Add(label);
                label = new Label();
                label.Content = "Выберите клиента:";
                label.FontSize = 18;
                label.HorizontalAlignment = HorizontalAlignment.Right;

                pannel5.Children.Add(label);
                ComboBox listBox = new ComboBox();
                listBox.FontSize = 18;
                listBox.SelectionChanged += ComboBox_SelectionChanged2;
                
                pannel3.Children.Add(listBox);
                fillCombo(listBox, "Documents", "document_type","","");

                ComboBox listBox2 = new ComboBox();
                listBox2.FontSize = 18;
                listBox2.SelectionChanged += ComboBox_SelectionChanged;

                pannel3.Children.Add(listBox2);
                fillCombo(listBox2, "Clients", "Surname", "Name", "Patronymic");

            }
        }
        private void ComboBox_SelectionChanged2(object sender, SelectionChangedEventArgs e)
        {
            cmb = (sender as ComboBox);
            if (IDClient != "")
            {
                typeItem = (sender as ComboBox).SelectedValue.ToString();
                IdTab = typeItem;
                pannel.Children.Clear();
                panel.Children.Clear();
                getTextBox(typeItem, IDClient);
                NameTab = ((sender as ComboBox).SelectedItem as DataRowView).Row[1].ToString();
            }

        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IDClient = (sender as ComboBox).SelectedValue.ToString();
            ComboBox_SelectionChanged2(cmb, e);
        }
       
        void getTextBox(string Docum, string IDClient)
        {
            id_req = "";
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("select * from documents_data where id_document = "+ Docum, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "documents_data");
            DataSet ds2 = new DataSet();
            SqlDataAdapter da2 = new SqlDataAdapter("select * from requests_documents where id_document = " + Docum+" AND id_clients = "+IDClient + "AND (status = 'False' OR status is null) ", conn);
            da2.Fill(ds2, "documents_data");

           
            if (ds2.Tables[0].Rows.Count != 0)
            {
                id_req = ds2.Tables[0].Rows[0][0].ToString();
                foreach (DataRow t in (ds.Tables[0].Rows))
                {
                    if (t[0].ToString() == "текст" || t[0].ToString() == "Текст")
                    {
                        TextBox textbox = new TextBox();
                        textbox.Height = 150;
                        textbox.FontSize = 18;
                        textbox.TextWrapping = TextWrapping.Wrap;
                        textbox.AcceptsReturn = true;
                        panel.Children.Add(textbox);
                        double all = 2;
                        textbox.Margin = new Thickness(all);
                    }
                    else
                    {
                        TextBox textbox = new TextBox();
                        textbox.Height = 30;
                        textbox.FontSize = 18;
                        panel.Children.Add(textbox);
                        double all = 2;
                        textbox.Margin = new Thickness(all);
                    }
                    Label label = new Label();
                    label.FontSize = 18;
                    label.Content = t[0].ToString();
                    label.HorizontalAlignment = HorizontalAlignment.Right;

                    pannel.Children.Add(label);
                    
                }
            }
            else
            {
                MessageBox.Show("Документ не может быть выдан из-за отсутствия запроса!");
            }
        }
       
        void fillCombo(object obj, string objName, string objProp,string str2, string str3)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("select * from "+ objName, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, objName);
            (obj as ComboBox).ItemsSource = ds.Tables[0].DefaultView;
            
               (obj as ComboBox).DisplayMemberPath = ds.Tables[0].Columns[objProp].ToString();
            (obj as ComboBox).SelectedValuePath = ds.Tables[0].Columns["id"].ToString();

        }
        
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            List<string> lst = new List<string>();
            try {
                try
                {

                    if (panel.Children[panel.Children.Count - 1].GetType().Name == "DatePicker")
                    {
                        for (int i = 0; i < panel.Children.Count; i++)
                        {
                            if (i < panel.Children.Count - 1)
                            {
                                lst.Add((panel.Children[i] as ComboBox).SelectedValue.ToString());

                            }
                            else

                                lst.Add((panel.Children[i] as DatePicker).ToString());
                        }
                    }
                    else if (panel.Children[0].GetType().Name == "TextBox")
                    {
                        foreach (TextBox t in panel.Children)
                        {
                            lst.Add(t.Text);
                        }
                    }
                    else if (panel.Children[panel.Children.Count - 1].GetType().Name == "TextBox")
                    {
                        for (int i = 0; i < panel.Children.Count; i++)
                        {
                            if (i < 2 || i == 3)

                                lst.Add((panel.Children[i] as ComboBox).SelectedValue.ToString());
                            else if (i == 2)

                                lst.Add((panel.Children[i] as DatePicker).ToString());
                            else
                                lst.Add((panel.Children[i] as TextBox).Text.ToString());
                        }
                    }
                    else if (panel.Children[panel.Children.Count - 1].GetType().Name == "ComboBox")
                    {
                        for (int i = 0; i < panel.Children.Count; i++)
                        {
                            if (i < 2 || i == 4)

                                lst.Add((panel.Children[i] as ComboBox).SelectedValue.ToString());
                            else if (i == 2)

                                lst.Add((panel.Children[i] as DatePicker).ToString());
                            else
                                lst.Add((panel.Children[i] as TextBox).Text.ToString());
                        }
                    }
                    else
                    {
                        foreach (TextBox t in panel.Children)
                        {
                            lst.Add(t.Text);
                        }
                    }
                    string[] arr = lst.ToArray();
                    
                    if (content != "Выдать документ")
                        Insert(arr, NameTab);
                    else
                    {
                        Insert(arr, NameTab);
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Документ добавлен. Вы хотите распечатать его?", "Подтверждение печати", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {

                            string sql = @"SELECT top 1 * FROM " + NameTab + " ORDER BY id desc ";
                            getColumn(NameTab, sql);
                            CreateDoc(NameTab);
                        }

                        ChangeStatus();
                    }
                }
                catch (System.NullReferenceException)
            {
                    MessageBox.Show("Заполните поля");
                }
                }
                catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Заполните поля");
            }
        }

        private void CreateDoc(string name)
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

            winword.Visible = true;

            object missing = System.Reflection.Missing.Value;
            
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            
            Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);

            para1.Range.Font.Size = 20;
            para1.Range.Bold = 1;
            para1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            para1.Range.Text = name + (dt.Rows[0] as DataRow).Field<int>(Convert.ToString(dt.Columns[0].ToString()).ToString());
            
            para1.Range.InsertParagraphAfter();
            Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
        
            for (int i = 2; i < dt.Columns.Count; i++)
            {
                para2.Range.Text += (dt.Rows[0] as DataRow).Field<string>(Convert.ToString(dt.Columns[i].ToString()).ToString());       
            }
            para2.Range.InsertParagraphAfter();
        }
        DataTable dt = new DataTable();
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

        void Insert(string [] arr, string nameT)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            con.Open();
           
            string qry = @"select * from " + nameT;
            string upd = makeInsertQuery(nameT);
            SqlConnection conn = con;

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand(qry, conn);

            DataSet ds = new DataSet();
            da.Fill(ds, objTab.GetType().Name);


            int i = 0;
            SqlCommand cmd = new SqlCommand(upd, conn);
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (col.ColumnName.ToString() != "id" && col.ColumnName.ToString() != "status" && col.ColumnName.ToString() != "finished")
                {
                    if (col.ColumnName.ToString() != "id_requests")
                    {
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
                    
                    i++;}
                }
                
            }
            cmd.CommandType = CommandType.Text;
            try
            {
                cmd.ExecuteNonQuery();

                da.UpdateCommand = cmd;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Заполните коректно поля");
            }
            con.Close();
        }

        string[] selectTable(string Name)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);

            var list = new List<string>();
            string[] allRecords = null;
            string sql = @"select c.name from sys.columns c inner join sys.objects o 
                on c.object_id=o.object_id where o.name = '" + Name + "'order by o.name,c.column_id";
            using (var command = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(reader.GetString(0));
                    allRecords = list.ToArray();
                }
            }
            return allRecords;

    }
    string makeInsertQuery(string nameT)
        {
            string[] str = selectTable(nameT);
            string sql = "INSERT INTO " + nameT + "(id_requests, ";
            foreach (string s in str)
            {
                if(s != "id" && s != "status" && s != "finished"&& s != "id_requests")
                 sql += " " + s + ",";
            }
            int ind = sql.LastIndexOf(",");
            sql = sql.Remove(ind, 1);
            sql += ") values("+id_req+", ";
            foreach (string s in str)
            {
                if (s != "id"&& s != "status" && s != "finished" && s != "id_requests")
                    sql += " @" + s + ",";
            }
            ind = sql.LastIndexOf(",");
            sql = sql.Remove(ind, 1);
            sql += ")";
            return sql;
        }
        private void ChangeStatus()
        {
            string query = "Update requests_documents SET status = @status WHERE id_clients = "+IDClient + " AND id_document = "+ typeItem + " AND (status = 'False'  OR status is null)";
            
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = "True";

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        void getInform(string Id)
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString);
            SqlConnection conn = con;
            SqlDataAdapter da = new SqlDataAdapter("select * from documents_data where id_document = '" + Id+"'", conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "documents_data");
            List <string> lst = new List<string>();
            foreach (DataRow t in (ds.Tables[0].Rows))
            {
                lst.Add(t[0].ToString());
            }
            arr2 = lst.ToArray();
        }
    
        private void InsertToDocum(string [] arr, string NameTable)
        {
            string query = "INSERT INTO " + NameTable + "( ";
            for (int i = 0; i < arr2.Length; i++)
            {
                if (i != arr.Length - 1)
                    query += "[" + arr2[i] + "], ";
                else
                    query += "[" + arr2[i] + "]) ";
            }
            query += "VALUES( "; 
            for (int i = 0; i < arr2.Length; i++)
            {
                if (i != arr2.Length - 1)
                    query += "@" + arr2[i] + ", ";
                else
                    query += "@" + arr2[i] + ") ";
            }
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connect1"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                for (int i = 0; i < arr2.Length; i++)
                {
                    cmd.Parameters.Add("@"+arr2[i]+"", SqlDbType.NVarChar, 100).Value = arr[i];
                }
                    cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
    }
}
