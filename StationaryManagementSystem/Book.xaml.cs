using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockManagementSystem
{
    /// <summary>
    /// Interaction logic for Book.xaml
    /// </summary>
    public partial class Book : UserControl
    {
        public static SqlConnection Conn;

     
      
        public Book()
        {
            InitializeComponent();

            LoadCategoryName();
         
            GetCustomerId();

            Todaysdate.Text = (DateTime.Today.Date).ToString();

            Invoice.IsEnabled = false;

            SoldId();
        }


        private void DatabaseCallMethod()
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
        }


        private void LoadCategoryName()
        {

            DatabaseCallMethod();


            String query = string.Format("select c_name from category");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();

           
                    categoryComboBox.Items.Add(sname);
                    CategoryComboBox.Items.Add(sname);

                 

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void SoldId()
        {
            DatabaseCallMethod();


            String query = string.Format("select count(cu_billno) from customer");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());


                    BillnoTextBox.Text = (sid + 1).ToString();



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void GetCustomerId()
        {
            DatabaseCallMethod();


            String query = string.Format("select count(cu_id) from customer");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());


                    CustomerIdTextBox.Text = (sid + 1).ToString();



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private int TotalBooks(string category,string edition)
        {
            int tbooks = 0;

            DatabaseCallMethod();


            String query = string.Format("select noofitems from stock where s_category='"+category+"' and s_edition='"+edition+"'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int nobooks  = int.Parse(reader[0].ToString());

                    tbooks += nobooks;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        

            return tbooks;
        
        
        }
  

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DatabaseCallMethod();


            String query = string.Format("select book.s_id,b_id, b_name, b_publication, b_author1, b_author2, b_author3, stock.s_id, s_edition, items_sellingprice from book INNER JOIN stock ON book.s_id = stock.s_id where s_category='" + categoryComboBox.SelectedItem + "' ");

            try
            {

                DataSet ds = new DataSet();


                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(ds);

                BookListView.DataContext = ds.Tables[0].DefaultView; 

                BookListView.AutoGenerateColumns = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }  
        }

        private void PrintBillButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(Invoice, "Print the Bill");
            }
        }

       
                
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BooknameComboBox.Items.Clear();
            
            DatabaseCallMethod();




            String query = string.Format("select b_name,book.s_id,stock.s_id from book INNER JOIN stock ON book.s_id=stock.s_id where s_category='" + CategoryComboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string sname = reader[0].ToString();

                        string sedition = reader[1].ToString();
                        BooknameComboBox.Items.Add(sname);

                        
                      
                    }
                }
                else
                {
                    MessageBox.Show("There is no book");

                    BooknameComboBox.Items.Clear();
                    UnitpriceTextBox.Text = string.Empty;
                    QuantityTextBox.Text = string.Empty;

                }

            }
            catch (Exception ex)
            {
                Conn.Close();
                MessageBox.Show(ex.Message);
            }


        }

        private void BooknameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DatabaseCallMethod();


            String query = string.Format("select items_sellingprice,stock.s_id,book.s_id from stock INNER JOIN book ON stock.s_id=book.s_id where b_name='" + BooknameComboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    decimal  suprice = decimal.Parse(reader[0].ToString());

                    UnitpriceTextBox.Text = suprice.ToString();
                
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
          

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string name;

            if(!NameRadioButton.IsChecked==true && !AuthornameRadioButton.IsChecked==true && !PublicationRadioButton.IsChecked==true)
            {
                MessageBox.Show("Select at least one of them");

            }

            else if (SearchTextBox.Text == "")
            {
                MessageBox.Show("Ente Something in the TextBox to search");
            }

            else if (NameRadioButton.IsChecked == true)
             {
                 DatabaseCallMethod();


                 name = SearchTextBox.Text;

                

                 String query = string.Format("select book.s_id,b_isbn, b_name, b_publication, b_author1, b_author2, b_author3, stock.s_id, s_edition, items_sellingprice from book INNER JOIN stock ON book.s_id = stock.s_id where b_name LIKE'" + name + "%' ");


                 DataSet ds = new DataSet();

                 SqlDataAdapter adapter = new SqlDataAdapter();
                 try
                 {

                     SqlCommand cmd = new SqlCommand(query, Conn);
                     adapter.SelectCommand = cmd;
                     adapter.Fill(ds);

                     SearchedListViewItem.DataContext = ds.Tables[0].DefaultView;
                     SearchedListViewItem.AutoGenerateColumns = false;
                 }


                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }

            
             }

             else if(AuthornameRadioButton.IsChecked==true)
             {
                 DatabaseCallMethod();

              
                 name = SearchTextBox.Text;
               

                  String query = string.Format("select book.s_id, b_isbn, b_name, b_publication, b_author1, b_author2, b_author3, stock.s_id, s_edition, items_sellingprice from book INNER JOIN stock ON book.s_id = stock.s_id where b_author1 LIKE'" + name + "%'");
                

                 DataSet ds = new DataSet();
                 SqlDataAdapter adapter = new SqlDataAdapter();
                 try
                 {

                     SqlCommand cmd = new SqlCommand(query, Conn);
                 
                     
                     adapter.SelectCommand = cmd;
                   
                     adapter.Fill(ds);
                     SearchedListViewItem.DataContext = ds.Tables[0].DefaultView;
                     SearchedListViewItem.AutoGenerateColumns = false;

                 }


                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }

            
             }

             else if(PublicationRadioButton.IsChecked==true)
             {

                 DatabaseCallMethod();

    
                 name = SearchTextBox.Text;

            

                 String query = string.Format("select book.s_id,b_isbn,b_name, b_publication, b_author1, b_author2, b_author3, stock.s_id, s_edition, items_sellingprice from book INNER JOIN stock ON book.s_id = stock.s_id where b_publication LIKE'" + name + "%' ");


                 DataSet ds = new DataSet();
                 SqlDataAdapter adapter = new SqlDataAdapter();
                 try
                 {

                     SqlCommand cmd = new SqlCommand(query, Conn);
                     adapter.SelectCommand = cmd;
                     adapter.Fill(ds);

                     SearchedListViewItem.DataContext = ds.Tables[0].DefaultView;
                     SearchedListViewItem.AutoGenerateColumns = false;
                 }


                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }

            
             }
           
        }


        private void FillBuyingDataGrid(int billno)
        {
            DatabaseCallMethod();

           

            String query = string.Format("select so_itemsname,so_itemsedition,so_itemscategory,so_noofitems,so_totalprice from sold where so_billno='"+billno+"' ");

            try
            {

                DataSet ds = new DataSet();


                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(ds);

                BuyingDataGrid.DataContext = ds.Tables[0].DefaultView;
                BuyingDataGrid.AutoGenerateColumns = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }  
        

        

        
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryComboBox.Text == "" || BooknameComboBox.Text == "" || QuantityTextBox.Text == "" || UnitpriceTextBox.Text == "" || DiscountComboBox.Text == "" || TotalTextBox.Text == "")
            {
                MessageBox.Show("Fill out all the blank first!!", " Message ");

            }

            else if (!Regex.Match(QuantityTextBox.Text, @"^([1-9]||[1-9][0-9])$").Success)
            {
                MessageBox.Show("Quantity must Be Integer Value", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }

            else if (HasThisSoldIdAlreadyExists(int.Parse(BillnoTextBox.Text)))
            {
               DatabaseCallMethod();

                string query = "delete from sold where so_billno='"+int.Parse(BillnoTextBox.Text)+"'";

                SqlCommand command=new SqlCommand(query,Conn);
                command.ExecuteNonQuery();
                MessageBox.Show("Successfully Runned");
                Conn.Close();
            }

            else if (true)
            {


                int sid=int.Parse(BillnoTextBox.Text);

                int cid = int.Parse(CustomerIdTextBox.Text);

                string cat = CategoryComboBox.Text;

                string book = BooknameComboBox.Text;

                int noofitems = int.Parse(QuantityTextBox.Text);

                decimal totalprice = decimal.Parse(TotalTextBox.Text);

                string solddate = Todaysdate.Text;

                decimal price = decimal.Parse(UnitpriceTextBox.Text);

                string edition = GetBooksEdition(cat, price);

                int totalbooks = GetTotalBooks(book);             

                int afterselling = totalbooks-noofitems;

             

                MessageBox.Show("U have "+afterselling+"more books");

                if (totalbooks > 1)
                {

                    DatabaseCallMethod();
            
       
                        string query = string.Format("insert into sold values('{0}','{1}','{2}','{3}','{4}','{5}')", book, edition, cat, noofitems,totalprice,cid);
                     
                        SqlCommand cmd = new SqlCommand(query, Conn);

                        cmd.ExecuteNonQuery();

                      string query1 = string.Format("update stock set noofitems='"+afterselling+"' where s_category='"+cat+"' and s_edition='"+edition+"'");

                      SqlCommand cmd1=new SqlCommand(query1,Conn);

                      cmd1.ExecuteNonQuery();


                  

                    GrandPriceTextBox.Text = GetTotalPrice(sid).ToString();

                    FillBuyingDataGrid(sid);
                }

                else 
                {
                    MessageBox.Show(" U Don't Have a Book to Sold!!", " Message ");
                }
            }
        }

   
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (CategoryComboBox.Text == "" || BooknameComboBox.Text == ""||QuantityTextBox.Text==""||DiscountComboBox.Text=="")
            {
                MessageBox.Show(" Please fill the entire field first!! ", " Message ");
            }

            else if (!Regex.Match(QuantityTextBox.Text,
         @"^([1-9]||[1-9][0-9])$").Success)
            {
                MessageBox.Show("Number of Books must  be Integer Value", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }



            else if (DiscountComboBox.Text == "")
            {

                MessageBox.Show(" Please select  one of Discount offered first!! ", " Message ");

            }



            else if (true)
            {
                int selecteditems = DiscountComboBox.SelectedIndex;

                if (selecteditems == 0)
                {

                    double unitprice = double.Parse(UnitpriceTextBox.Text);

                    int noofbook = int.Parse(QuantityTextBox.Text);

                    double tprice = unitprice * noofbook;

                    TotalTextBox.Text = tprice.ToString();



                }

                else if (selecteditems == 1)
                {

                    double unitprice = double.Parse(UnitpriceTextBox.Text);

                    int noofbook = int.Parse(QuantityTextBox.Text);

                    double bookppriceafterdiscount = unitprice - (unitprice * 0.05);

                    double tprice = bookppriceafterdiscount * noofbook;

                    TotalTextBox.Text = tprice.ToString();



                }


                else if (selecteditems == 2)
                {

                    double unitprice = double.Parse(UnitpriceTextBox.Text);

                    int noofbook = int.Parse(QuantityTextBox.Text);

                    double bookppriceafterdiscount = unitprice - (unitprice * 0.1);

                    double tprice = bookppriceafterdiscount * noofbook;

                    TotalTextBox.Text = tprice.ToString();



                }

            }




        }

        private void SellButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text == "" || MobilenoTextBox.Text=="")
            {
                MessageBox.Show("Fill out All the Customer Information to sell the books");
            }

       else if (!Regex.Match(NameTextBox.Text,
               @"^((([A-Z].)*\s+([A-Z])[a-z]+)+||[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Customer Name ", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
               

            }

            else if(!Regex.Match(MobilenoTextBox.Text,@"^\d{11}$").Success)
            {
                MessageBox.Show("Invalid Mobile Number Should be 11 digit", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            
            }

            else
            {

                string name = NameTextBox.Text;
                string mobileno = MobilenoTextBox.Text;
                string date = Todaysdate.Text;
                int billno = int.Parse(BillnoTextBox.Text);

                DatabaseCallMethod();
                string query = string.Format("insert into customer values('{0}','{1}','{2}','{3}')",name,mobileno, billno,date);
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Customer Information Saved Successfully");

                Invoice.IsEnabled = true;


                BillPrintTextBox.Text = billno.ToString();
                CustomernameBox.Text = name;
                CustomerMobilenoBox.Text = mobileno;
                GrandPriceDisplayTextBox.Text = GetTotalPrice(billno).ToString();
                GetPrintSoldBookDetails(billno);

                Tc.SelectedIndex = 3;

            }


        }

        private int GetTotalBooks(string bookname)
        {
            int totalbooks=0;
       
            
            DatabaseCallMethod();

            String query = string.Format("select stock.noofitems from stock inner join book  on book.s_id=stock.s_id and book.b_name='"+bookname+"'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();



                while (reader.Read())
                {

                    totalbooks += int.Parse(reader[0].ToString());
             
                }

                reader.Close();
                Conn.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return totalbooks;
        }


        private string  GetBooksEdition(string category,decimal price)
        {
            string  booksedition=""; 
          

            DatabaseCallMethod();

            String query = string.Format("select s_edition from stock where (items_sellingprice='" + price + "' and s_category='"+category+"')");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();



                while (reader.Read())
                {

                    booksedition = reader[0].ToString();

                }

                reader.Close();
                Conn.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return booksedition;
        }

        private decimal GetTotalPrice(int billno)
        {

            decimal totalprice = 0;



            DatabaseCallMethod();

            String query = string.Format("select Sum(so_totalprice) from sold where so_billno='" + billno + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();



                while (reader.Read())
                {

                    totalprice += decimal.Parse(reader[0].ToString());

                }

                reader.Close();
                Conn.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return totalprice;
        }



        private void GetPrintSoldBookDetails(int billno)
        {
        
        DatabaseCallMethod();

            String query = string.Format("select so_itemsname,so_itemsedition,so_itemscategory,so_noofitems,so_totalprice from sold where so_billno='"+billno+"' ");

            try
            {

                DataSet ds = new DataSet();


                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(ds);

               BuyingItem.DataContext = ds.Tables[0].DefaultView;
               BuyingItem.AutoGenerateColumns = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }  
        

        }



        public bool HasThisSoldIdAlreadyExists(int soid)
        {
            DatabaseCallMethod();

            bool soidxists = false;

            String query = string.Format("select s_id from book where s_id='" + soid + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    soidxists = true;
                    break;


                }
                reader.Close();
                Conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return soidxists;
        }

      
    


    }
        
            
    }

