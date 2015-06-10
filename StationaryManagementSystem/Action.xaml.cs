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
    /// Interaction logic for Action.xaml
    /// </summary>
    public partial class Action : UserControl
    {
        public static SqlConnection Conn;
        public Action()
        {
            InitializeComponent();
            LoadCategoryCountId();
            LoadCategoryId();
            LoadTotalCategoryList();
            SubcategoryId();
            LoadTotalSubcategoryList();
            SubcategoryCountId();
            LoadSubcategoryName();
        }

       
        /* --  Category Starts From Here  -- */
         
         private void LoadTotalCategoryList()
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select * from category");

            try
            {

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            
            SqlCommand cmd = new SqlCommand(query, Conn);
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            CategoryListViewItem.DataContext = ds.Tables[0].DefaultView;  // ListView
            CategoryListViewItem.AutoGenerateColumns = false;
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void LoadCategoryCountId()
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }

  
            String query = string.Format("select Count(c_id) from category");
            
            try
            {
                
                SqlCommand cmd = new SqlCommand(query,Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());

                    CategoryIdTextBox.Text = (sid + 1).ToString();
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        
        }
       

        private void TabItem_Initialized(object sender, EventArgs e)
        {
            LoadCategoryCountId();
           
        }


        private void LoadSubcategoryName()
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select sc_name from subcategory");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();

                    SubcategoryNameComboBox.Items.Add(sname);

                    SubcategoryNamecomboBox.Items.Add(sname);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        
        }

        private void LoadCategoryId() 
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select c_id from category");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());

                    CategoryIdComboBox.Items.Add(sid);
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
                
        }


        private string GetSubName(int sid)
        {
            string name="" ;

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select sc_name from subcategory where sc_id='" + sid + "'");

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

        private int GetSubId(string name)
        {
            int id = 0;
            

             Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select sc_id from subcategory where sc_name='"+name+"'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                  id += int.Parse(reader[0].ToString());

                  
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return id;
        }


        private void CategoryIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select c_name,sc_id from category where c_id='"+CategoryIdComboBox.SelectedItem+"'");



            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname =reader[0].ToString();

                    CategoryNameBox.Text = sname;

                    int scid = int.Parse(reader[1].ToString());
                   
                     SubcategoryNamecomboBox.Text = GetSubName(scid);

                }
                reader.Close();
                Conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   

        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Enter Category Name:");
            }

            else if (!Regex.Match(CategoryNameTextBox.Text,
               @"^([A-Z][a-z]+||[A-Z][A-Z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Category Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);


            }

            else if(SubcategoryNameComboBox.Text==string.Empty)
            {
                MessageBox.Show("Please select a Subcategory");


            }

            else if (true)
            {

                string category = CategoryNameTextBox.Text;

                string name = SubcategoryNameComboBox.Text;

                int key = GetSubId(name);
               

                
                Conn = DatabaseConnection.GetConnection();

                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }


                String query = string.Format("insert into category values('{0}','{1}')",category,key);


                try
                {

                    SqlCommand cmd = new SqlCommand(query, Conn);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data is inserted Successfully!!");

                    CategoryNameTextBox.Text = string.Empty;
                    SubcategoryNameComboBox.Text = string.Empty;
                    CategoryIdTextBox.Text = string.Empty;
                    LoadCategoryCountId();
                    LoadTotalCategoryList();
                    Conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            
        }
}
    
        private void UpdateCategoryButton_Click(object sender, RoutedEventArgs e)
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            if(CategoryNameBox.Text==string.Empty)
            {
                MessageBox.Show("Enter Category Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(CategoryNameBox.Text,
                @"^([A-Z][a-z]+||[A-Z][A-Z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Category Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            else if (SubcategoryNamecomboBox.Text == string.Empty)
            {
                MessageBox.Show("Please Select a Subcategory Name:","Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            else if (true)
            {
                string subcatname = SubcategoryNamecomboBox.Text;

                int subcatid = GetSubId(subcatname);

                String query = string.Format("update category set c_name='" + CategoryNameBox.Text + "', sc_id='"+subcatid+"'  where c_id = '" + CategoryIdComboBox.SelectedItem + "'");


                try
                {


                    SqlCommand cmd = new SqlCommand(query, Conn);
                    cmd.ExecuteNonQuery();
                    Conn.Close();

                    MessageBox.Show("Ur Data is Updated Successfully!!!");
                    LoadTotalCategoryList();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        
        private void CategoryListViewItem_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            DataRowView dr = CategoryListViewItem.CurrentCell.Item as DataRowView;

            if (dr != null)
            {
                string sid = dr.Row[0].ToString();
                string sname = dr.Row[1].ToString();

                CategoryIdComboBox.Text = sid;

                CategoryNameBox.Text = sname;

                Tc.SelectedIndex = 2;

            }

            else
            {
                MessageBox.Show("U have selected Empty Row!!");
            }
        }

           

        /*-- SubCategory TabItem Starts From Here -- */


        private void SubcategoryId()
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select sc_id from subcategory");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());

                    SubcategoryIdComboBox.Items.Add(sid);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   

        }


        private void SubcategoryCountId()
        {


            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select Count(sc_id) from subcategory");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int sid = int.Parse(reader[0].ToString());

                    SubcategoryIdTextBox.Text = (sid + 1).ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
        
        }


        private void SubcategoryIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select sc_name from subcategory where sc_id='" +SubcategoryIdComboBox.SelectedItem + "'");

            try
            {

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();

                    SubcategoryNameBox.Text = sname;



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   


        }
        

        private void LoadTotalSubcategoryList()
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }


            String query = string.Format("select * from subcategory");

            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                SubcategoryListItem.DataContext = ds.Tables[0].DefaultView;  // ListView
                SubcategoryListItem.AutoGenerateColumns = false;


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        private void AddSubcategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubcategoryNameTextBox.Text == string.Empty)
            {
                MessageBox.Show("Enter Subcategory Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            else if (!Regex.Match(SubcategoryNameTextBox.Text,
              @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid Subcategory Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (true)
            {
                string subname = SubcategoryNameTextBox.Text;


                Conn = DatabaseConnection.GetConnection();

                if (Conn.State.ToString() == "closed")
                {
                    Conn.Open();
                }


                String query = string.Format("insert into subcategory values('{0}')", subname);


                try
                {

                    SqlCommand cmd = new SqlCommand(query, Conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data is inserted Successfully!!");
                    SubcategoryIdTextBox.Text = "";
                    SubcategoryNameTextBox.Text = "";
                    SubcategoryCountId();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }


        private void UpdateSubcategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubcategoryNameBox.Text == string.Empty)
            {

                MessageBox.Show("Invalid User type Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (!Regex.Match(SubcategoryNameBox.Text,
               @"^([A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+||[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Invalid User type Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            else if (true)
            {
                try
                {

                    Conn = DatabaseConnection.GetConnection();

                    if (Conn.State.ToString() == "closed")
                    {
                        Conn.Open();
                    }


                    String query = string.Format("update subcategory set sc_name='" + SubcategoryNameBox.Text + "' where sc_id='" + SubcategoryIdComboBox.SelectedItem + "'");

                    SqlCommand cmd = new SqlCommand(query, Conn);
                    cmd.ExecuteNonQuery();
                    Conn.Close();

                    MessageBox.Show("Ur Data is Updated Successfully!!!");

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void SubcategoryListItem_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataRowView dr = SubcategoryListItem.CurrentCell.Item as DataRowView;

            if (dr != null)
            {
                string sid = dr.Row[0].ToString();
                string sname = dr.Row[1].ToString();

                SubcategoryIdComboBox.Text = sid;

                SubcategoryNameBox.Text = sname;

                Tc.SelectedIndex = 5;

            }

            else
            {
                MessageBox.Show("U have selected Empty Row!!");
            } 
        }

   

    }
}