using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace StockManagementSystem
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    /// 
    public partial class Stock
    {
        public static SqlConnection Conn;

        public Stock()
        {
            InitializeComponent();
            LoadStockId();
            LoadCategoryName();
            LoadTotalStockDetails();
            StockupdateId();
            LoadBooksId();
            DatePickerTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
            DatePickerBox.Text = DateTime.Today.ToString("dd-MM-yyyy");
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

                    ItemsCategoryComboBox.Items.Add(sname);
                    ItemsCategoryBox.Items.Add(sname);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private string GetEdtionName()
        {
            string name = "";

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select s_edition from stock where s_id='" + StockIdcomboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    name = reader[0].ToString();



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return name;
        }

      

        private void LoadStockId()
        {
            DatabaseCallMethod();

            StockIdcomboBox.Items.Clear();

            String query = string.Format("select s_id from stock");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());

                    StockIdTextBox.Text = (sid + 1).ToString();

                    StockIdcomboBox.Items.Add(sid);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void LoadBooksId()
        {
            BooksIdComboBox.Items.Clear();

            DatabaseCallMethod();

            String query = string.Format("select b_id from book");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());


                    BooksIdComboBox.Items.Add(sid);

                    BooksIdTextBox.Text = (sid + 1).ToString();

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void LoadTotalStockDetails()
        {
            DatabaseCallMethod();

            String query = string.Format("select * from stock");

            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                TotalBookListItem.DataContext = ds.Tables[0].DefaultView; // ListView
                TotalBookListItem.AutoGenerateColumns = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }



        }

        private void TotalBookListItem_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataRowView reader = TotalBookListItem.CurrentCell.Item as DataRowView;

            if (reader != null)
            {

                string sid = reader[0].ToString();

                string bid = reader.Row[1].ToString();

                string sisbn = reader.Row[2].ToString();

                string sname = reader.Row[3].ToString();

                string spublication = reader.Row[4].ToString();

                string sauthor1 = reader.Row[5].ToString();

                string sauthor2 = reader.Row[6].ToString();

                string sauthor3 = reader.Row[7].ToString();





                StockIdbox.Text = sid;

                BooksIdComboBox.Text = bid;

                BookIsbnBox.Text = sisbn;

                BookNameBox.Text = sname;

                Authorname1Box.Text = sauthor1;

                Authorname2Box.Text = sauthor2;

                Authorname3Box.Text = sauthor3;

                PublicationBox.Text = spublication;


                Tc.SelectedIndex = 5;


            }

            else
            {
                MessageBox.Show("U have selected Empty Row!!");
            }



        }

        private void AddStockButton_Click(object sender, RoutedEventArgs e)
        {

            if (StockIdTextBox.Text == "" || ItemsBoughtTextBox.Text == "" || ItemsEditionTextBox.Text == "" ||
                TotalPurchasecostTextBox.Text == "" || DatePickerTextBox.Text == "" ||
                ItemsCategoryComboBox.Text == string.Empty)
            {
                MessageBox.Show("Fill up all the Blank places", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(ItemsBoughtTextBox.Text,
                @"^(\d{2,4})$").Success)
            {
                MessageBox.Show(" Items Bought Must be integer", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(ItemsEditionTextBox.Text,
                @"^([1-9](st|nd|rd|th)||([1-9][0-9](st|nd|rd|th)))$").Success)
            {
                MessageBox.Show(" Items Bought Must be integer", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(TotalPurchasecostTextBox.Text,
                @"^(\d{3,10})$").Success)
            {
                MessageBox.Show(" It can't be a string ", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            else if (true)
            {
                // StockDataGrid.Items.Clear();

                DatabaseCallMethod();

                // int sid=int.Parse(StockIdTextBox.Text);

                int noofitems = int.Parse(ItemsBoughtTextBox.Text);

                string category = ItemsCategoryComboBox.Text;

                string edition = ItemsEditionTextBox.Text;

                decimal purchaseprice = decimal.Parse(TotalPurchasecostTextBox.Text);

                string date = DatePickerTextBox.Text;


                decimal itemsbuyingprice = purchaseprice/noofitems;

                decimal itemssellingprice = itemsbuyingprice + (itemsbuyingprice*10/100);

                string query = string.Format("insert into stock values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                    noofitems, category, edition, purchaseprice, date, itemsbuyingprice, itemssellingprice);


                var cmd = new SqlCommand(query, Conn);


                cmd.ExecuteNonQuery();


                MessageBox.Show("Stock Information is Saved Successfully!!");
                LoadTotalStockDetails();
                GetClearStockSaveTextBoxes();
                LoadStockId();
                Conn.Close();



            }
        }

        
        public void GetClearStockSaveTextBoxes()
        {
            StockIdTextBox.Text = "";
            ItemsBoughtTextBox.Text = "";
            ItemsCategoryComboBox.Text = "";
            ItemsEditionTextBox.Text = "";
            TotalPurchasecostTextBox.Text = "";
            DatePickerTextBox.Text = "";
        }

        private void StockIdcomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DatabaseCallMethod();

            String query =
                string.Format("select s_category,s_edition from stock where s_id='" + StockIdcomboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string scategoryname = reader[0].ToString();

                    string sedition = reader[1].ToString();


                    ItemsCategoryBox.Text = scategoryname;

                    editionBox.Text = sedition;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateStockButton_Click(object sender, RoutedEventArgs e)
        {


            if (StockIdcomboBox.Text == "" || ItemsBoughtBox.Text == "" || editionBox.Text == "" ||
                TotalPurchasecostBox.Text == "" || DatePickerBox.Text == "" || ItemsCategoryBox.Text == "" ||
                StockUpdateIdTextBox.Text == " ")
            {
                MessageBox.Show("Fill up all the Blank places", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(ItemsBoughtBox.Text,
                @"^(\d{2,4})$").Success)
            {
                MessageBox.Show(" Items Bought Must be integer", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(editionBox.Text,
                @"^([1-9](st|nd|rd|th)||([1-9][0-9](st|nd|rd|th)))$").Success)
            {
                MessageBox.Show(" Items Bought Must be integer", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(TotalPurchasecostBox.Text,
                @"^(\d{3,10})$").Success)
            {
                MessageBox.Show(" It can't be a string ", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (true)
            {


                int sid = int.Parse(StockIdcomboBox.Text);

                int uid = int.Parse(StockUpdateIdTextBox.Text);

                int noofitems = int.Parse(ItemsBoughtBox.Text);

                string edition = editionBox.Text;

                decimal purchaseprice = decimal.Parse(TotalPurchasecostBox.Text);

                string date = DatePickerBox.Text;

                int totalitems = noofitems + TotalItems();

                decimal totalcost = purchaseprice + TotalPurchasecost();

                decimal buyingprice = totalcost/totalitems;

                string category = ItemsCategoryBox.Text;

                decimal sellingprice = buyingprice + (buyingprice*10/100);

                string previousedition = GetEdtionName();

                if (edition == previousedition)
                {
                    string query1 =
                        string.Format("update stock set noofitems = '" + totalitems + "', s_pcost = '" + totalcost +
                                      "' where s_id ='" + StockIdcomboBox.SelectionBoxItem + "'");

                    var cmd1 = new SqlCommand(query1, Conn);

                    cmd1.ExecuteNonQuery();



                }

                else
                {


                    string query1 =
                        string.Format("insert into edition values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", uid,
                            noofitems, category, edition, purchaseprice, date, buyingprice, sellingprice);

                    string query2 = string.Format(
                        "insert into stock values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", noofitems, category,
                        edition, purchaseprice, date, buyingprice, sellingprice);


                    var cmd1 = new SqlCommand(query1, Conn);

                    var cmd2 = new SqlCommand(query2, Conn);

                    cmd1.ExecuteNonQuery();

                    cmd2.ExecuteNonQuery();
                }


                MessageBox.Show("Stock is updated  and stockupdate tables have a values!!");

                Conn.Close();

            }
        }

        private void StockupdateId()
        {

            DatabaseCallMethod();

            String query = string.Format("select e_id from edition");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());


                    StockUpdateIdTextBox.Text = (sid + 1).ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }



        private int TotalItems()
        {
            int titems = 0;


            DatabaseCallMethod();

            String query = string.Format("select noofitems from stock where s_id='" + StockIdcomboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    titems = int.Parse(reader[0].ToString());




                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



            return titems;
        }

        private decimal TotalPurchasecost()
        {
            decimal tcost = (decimal) 0.0;


            DatabaseCallMethod();

            String query = string.Format("select s_pcost from stock where s_id='" + StockIdcomboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tcost = decimal.Parse(reader[0].ToString());


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return tcost;

        }

        private void BooksIdComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DatabaseCallMethod();

            String query =
                string.Format(
                    "select s_id,b_isbn,b_name,b_publication,b_author1,b_author2,b_author3 from book where b_id='" +
                    BooksIdComboBox.SelectedItem + "'");

            try
            {

                var cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int sid = int.Parse(reader[0].ToString());

                        string sisbn = reader[1].ToString();

                        string sbname = reader[2].ToString();

                        string spublication = reader[3].ToString();

                        string sauthorname1 = reader[4].ToString();

                        string sauthorname2 = reader[5].ToString();

                        string sauthorname3 = reader[6].ToString();

                        StockIdbox.Text = sid.ToString();

                        BookIsbnBox.Text = sisbn;

                        BookNameBox.Text = sbname;

                        Authorname1Box.Text = sauthorname1;

                        Authorname2Box.Text = sauthorname2;

                        Authorname3Box.Text = sauthorname3;

                        PublicationBox.Text = spublication;



                    }
                }

                else
                {
                    MessageBox.Show("Empty");

                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void StockDataGrid_Initialized(object sender, EventArgs e)
        {

            DatabaseCallMethod();

            String query = string.Format("select * from stock");

            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                StockDataGrid.DataContext = ds.Tables[0].DefaultView; // ListView



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        private void StockIdBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            DatabaseCallMethod();

            String query = string.Format("select s_category,s_edition from stock where s_id='" + StockIdBox.Text + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string scat = reader[0].ToString();

                    string edition = reader[1].ToString();

                    CategoryTextBox.Text = scat;

                    EditionTextBox.Text = edition;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void StockIdbox_TextChanged(object sender, TextChangedEventArgs e)
        {

            DatabaseCallMethod();

            String query = string.Format("select s_category,s_edition from stock where s_id='" + StockIdbox.Text + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string scat = reader[0].ToString();

                    string edition = reader[1].ToString();

                    CategoryBox.Text = scat;

                    EditionBox.Text = edition;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void TotalBookListItem_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseCallMethod();

            String query = string.Format("select * from book");

            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                TotalBookListItem.DataContext = ds.Tables[0].DefaultView; // ListView
                TotalBookListItem.AutoGenerateColumns = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void AddBookDiscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookIsbnTextBox.Text == "" || BookNameTextBox.Text == "" || Authorname1TextBox.Text == "" ||
                PublicationTextBox.Text == "" || EditionTextBox.Text == "")
            {
                MessageBox.Show("Fill up all the Blank places", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(BookIsbnTextBox.Text, @"^(97(8|9))?\d{9}(\d|X)$").Success)
            {
                MessageBox.Show("Invalid  Book Isbn Number.ISBN number must be 10 or 13 digit", "Message",
                    MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(BookNameTextBox.Text,
                @"^(([A-Z][a-z]+\s)+)$").Success)
            {
                MessageBox.Show("Invalid Book Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(Authorname1TextBox.Text,
                @"^((([A-Z].)*\s+([A-Z])[a-z]+)+||[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+)))$")
                .Success)
            {
                MessageBox.Show("Invalid Author Name 1", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(Authorname2TextBox.Text,
                @"^((([A-Z].)*\s+([A-Z])[a-z]+)+||[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+)))$")
                .Success)
            {
                MessageBox.Show("Invalid Author Name 2", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }

            else if (
                !Regex.Match(Authorname3TextBox.Text,
                    @"^((([A-Z].)*\s+([A-Z])[a-z]+)+||[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+)))$")
                    .Success)
            {
                MessageBox.Show("Invalid Author Name 3", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(PublicationTextBox.Text,
                @"^([A-Z][a-z]\s+)+||([A-Z]+||(([A-Z].)?)+[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Publication Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }

            else if (!Regex.Match(EditionTextBox.Text,
                @"^(1st||2nd||3rd||[4-9]th||[1 2][0-9]th||21st)$").Success)
            {
                MessageBox.Show("Invalid Editon", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (IsThisBookIsbnExists(BookIsbnTextBox.Text))
            {
                MessageBox.Show("Books ISBN Numbers are Unique.Two books can't have the same ISBN Number.", "Invalid Message", MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }

            else if (IsThisBooksDescriptionexists(int.Parse(StockIdBox.Text)))
            {
                MessageBox.Show("U are not allowed to add this books description because this stock id books description already exists.",
                    "Invalid Message",MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

            else if (true)
            {
                int sid = int.Parse(StockIdBox.Text);

                int bid = int.Parse(BooksIdTextBox.Text);

                string isbn = BookIsbnTextBox.Text;

                string name = BookNameTextBox.Text;

                string author1 = Authorname1TextBox.Text;

                string author2 = Authorname2TextBox.Text;

                string author3 = Authorname3TextBox.Text;

                string publication = PublicationTextBox.Text;






                DatabaseCallMethod();

                String query =
                    string.Format("insert into book values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", bid,
                        isbn, name, publication, author1, author2, author3, sid);

                var cmd = new SqlCommand(query, Conn);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Book Information Saved Successfully!!");

                Conn.Close();

                GetAddBookTextBoxesClear();
                LoadBooksId();
            }
        }

        private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            if (StockIdbox.Text == "" || BookIsbnBox.Text == "" || BookNameBox.Text == "" || Authorname1Box.Text == "" ||
                PublicationBox.Text == "" || EditionBox.Text == "")
            {
                MessageBox.Show("Fill up all the Blank places", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(BookIsbnBox.Text,
                @"^(97(8|9))?\d{9}(\d|X)$").Success)
            {
                MessageBox.Show("Invalid  Book Isbn Number.ISBN number should be 10 or 13 digit", "Message",
                    MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(BookNameBox.Text,
                @"^(([A-Z][a-z]+\s)+)$").Success)
            {
                MessageBox.Show("Invalid Book Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(Authorname1Box.Text,
                @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+)||(([A-Z].)*\s+([A-Z])[a-z]+)+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+))$")
                .Success)
            {
                MessageBox.Show("Invalid Author Name 1", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(Authorname2Box.Text,
                @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+)||(([A-Z].)*\s+([A-Z])[a-z]+)+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+))$")
                .Success)
            {
                MessageBox.Show("Invalid Author Name 2", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }

            else if (!Regex.Match(Authorname3Box.Text,
                @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s([A-Z].||[A-Z][a-z]+)?\s[A-Z][a-z]+||[A-Z][.]\s+[A-Z][a-z]+)||(([A-Z].)*\s+([A-Z])[a-z]+)+||((([A-Z].)*)+[A-Z][a-z]+||([A-Z][a-z]+\s[A-Z].[A-Z][a-z]+))$")
                .Success)
            {
                MessageBox.Show("Invalid Author Name 3", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }


            else if (!Regex.Match(PublicationBox.Text,
                @"^(([A-Z][a-z]\s+)+||([A-Z]+)||(([A-Z].)?)+[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Publication Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                PublicationBox.Focus();

            }

            else if (!Regex.Match(EditionBox.Text,
                @"^(1st||2nd||3rd||[4-9]th||[1 2][0-9]th||21st)$").Success)
            {
                MessageBox.Show("Invalid Editon", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                EditionBox.Focus();

            }





            else if (true)
            {

                int sid = int.Parse(StockIdbox.Text);

                string isbn = BookIsbnBox.Text;

                string name = BookNameBox.Text;

                string author1 = Authorname1Box.Text;

                string author2 = Authorname2Box.Text;

                string author3 = Authorname3Box.Text;

                string publication = PublicationBox.Text;


                DatabaseCallMethod();

                String query =
                    string.Format("update book set s_id='" + sid + "', b_isbn='" + isbn + "', b_name='" + name +
                                  "', b_publication='" + publication + "', b_author1='" + author1 + "', b_author2='" +
                                  author2 + "', b_author3='" + author3 + "' where b_id='" + BooksIdComboBox.SelectedItem +
                                  "'  ");

                var cmd = new SqlCommand(query, Conn);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Book Information  is  Updated Successfully!");

                Conn.Close();

            }
        }


        private void GetAddBookTextBoxesClear()
        {
            StockIdBox.Text = "";
            BooksIdTextBox.Text = "";
            BookIsbnTextBox.Text = "";
            Authorname1TextBox.Text = "";
            Authorname2TextBox.Text = "";
            Authorname3TextBox.Text = "";
            PublicationTextBox.Text = "";
            BookNameTextBox.Text = "";
            EditionTextBox.Text = "";
            CategoryTextBox.Text = "";
        }


        public bool IsThisBookIsbnExists(string isbn)
        {

            DatabaseCallMethod();

            bool isbnexists = false;

            String query = string.Format("select b_isbn from book where b_isbn='" + isbn + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    isbnexists = true;
                    break;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return isbnexists;
        }

        public bool IsThisBooksDescriptionexists(int id)
        {
            DatabaseCallMethod();

            bool sidexists = false;

            String query = string.Format("select s_id from book where s_id='" + id + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sidexists = true;
                    break;


                }
                reader.Close();
                Conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return sidexists;

        }

        private void ReturnBooksUpdate_OnClick(object sender, RoutedEventArgs e)
        {
           
         

            if (ReturnBookCategoryTextBox.Text == "" || ReturnBookNoofBooksTextBox.Text == "")
            {
                MessageBox.Show("Fill out the boxes:", "Invalid Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else
            {


                int billno = int.Parse(ReturnBookBillnoTextBox.Text);

                string bookname = ReturnBookNameTextBox.Text;

                string category = ReturnBookCategoryTextBox.Text;

                int noofitems = int.Parse(ReturnBookNoofBooksTextBox.Text);

                int totalbooks = GetStockNumberofItemsForReturnBookUpdate(bookname);

                int id = GetStockIdForReturnBookUpdate(bookname);

                totalbooks = totalbooks + noofitems;

                MessageBox.Show(id.ToString());

                DatabaseCallMethod();


                string query1 = "delete from sold where so_billno='" + billno + "'";

                string query = "update stock set noofitems='" + totalbooks + "' where s_id='" + id + "'";

                SqlCommand command1 = new SqlCommand(query1, Conn);
      
                SqlCommand command = new SqlCommand(query, Conn);

             

                if (command.ExecuteNonQuery() > 0 && command1.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Sold Information Deleted and Stock Update after Returning Books");
                }
                else
                {
                    MessageBox.Show("Operation Failed", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
                Conn.Close();

            }


        }


        public bool HasThisBillNoexists(int billno)
        {

            DatabaseCallMethod();

            bool billnoexists = false;


            String query = string.Format("select so_billno from sold where so_billno='" + billno + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    billnoexists = true;
                    break;


                }

                reader.Close();
                Conn.Close();
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return billnoexists;

        }


        public bool HasThisBookExistsUnderId(string bookname)
        {
            DatabaseCallMethod();

            bool bookexists = false;



            String query = string.Format("select so_itemsname from sold where so_itemsname='" + bookname + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bookexists = true;
                    break;


                }

                reader.Close();
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return bookexists;

        }


        public int GetStockNumberofItemsForReturnBookUpdate(string bookname)
        {
            DatabaseCallMethod();

            int totalnums = 0;
    
            string query =
                "select distinct(s.noofitems) from stock as s join book as b on s.s_id=b.s_id join sold as so on b.b_name=so.so_itemsname where b.b_name='"+bookname+"' ";

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    totalnums += int.Parse(reader["noofitems"].ToString());

                }

                reader.Close();
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return totalnums;
        }


        public int GetStockIdForReturnBookUpdate(string bookname)
        {
            DatabaseCallMethod();

            int sid = 0;
            string query ="select distinct(s_id) from book as b join sold as so on b.b_name=so.so_itemsname where so_itemsname='" +bookname + "' ";

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    sid += int.Parse(reader["s_id"].ToString());

                }

                reader.Close();
                Conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return sid;
        }


       

        private void ReturnBookShowDetails_OnClick(object sender, RoutedEventArgs e)
        {

            if (ReturnBookBillnoTextBox.Text == "" || ReturnBookNameTextBox.Text == "")
            {
                MessageBox.Show("Fill all the boxes of above", "Invalid Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            else
            {



                int billno = int.Parse(ReturnBookBillnoTextBox.Text);

                string bookname = ReturnBookNameTextBox.Text;



            
                if (!HasThisBillNoexists(billno))
                {
                    MessageBox.Show("Enter Valid bill no", "Invalid Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                else if (!HasThisBookExistsUnderId(bookname))
                {
                    MessageBox.Show("Enter the correct book bought under this bill no:", "Invalid Message",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {

                    DatabaseCallMethod();


                    String query =
                        string.Format("select so_itemscategory,so_noofitems from sold where so_billno='" + billno +
                                      "' and so_itemsname='" + bookname + "'");

                    try
                    {

                        SqlCommand cmd = new SqlCommand(query, Conn);

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string category = reader["so_itemscategory"].ToString();
                            int noofitems = int.Parse(reader["so_noofitems"].ToString());

                            ReturnBookCategoryTextBox.Text = category;

                            ReturnBookNoofBooksTextBox.Text = noofitems.ToString();

                        }
                        reader.Close();
                        Conn.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                }
            }



        }
    }

}
