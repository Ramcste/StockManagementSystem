using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace StockManagementSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();
            Remember();
            
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            UsernameTextBox.Text = "";
            PasswordTextBox.Password = "";
           
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            
            if (UsernameTextBox.Text == string.Empty || PasswordTextBox.Password == string.Empty)
            {
                MessageBox.Show("Enter values on both fields !!", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }



            else if (
                !Regex.Match(UsernameTextBox.Text,
                    @"^([A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+|[A-Z][a-z]+\s[A-Z][a-z]+\s[A-Z][a-z]+)$").Success)
            {
                MessageBox.Show("Error in User Name", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (!Regex.Match(PasswordTextBox.Password, @"^([a-z0-9_-]{6,18})$").Success)
            {
                MessageBox.Show("Error in  Password", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

            else if (true)
            {

                int value;

                   
                    String query =
                        string.Format("select sp_name,sp_password from  shopkeeper  where sp_name= '" + UsernameTextBox.Text +
                                      "' and  sp_password='" + PasswordTextBox.Password + "' ");

                    if (RememberCheckBox.IsChecked == true)
                    {
                        value = 1;
                    }
                    else
                    {
                        value = 0;
                    }

                    String query1 = string.Format("update shopkeeper set sp_remember='"+value+"' where sp_name='"+UsernameTextBox.Text+"'");
                
                var connection = new SqlConnection(connectionString);

                    try
                    {
                        connection.Open();
                        var cmd = new SqlCommand(query, connection);

                        var cmd1 = new SqlCommand(query1,connection);

                        cmd1.ExecuteNonQuery();

                        SqlDataReader reader = cmd.ExecuteReader();
                        int count = 0;
                        while (reader.Read())
                        {
                            count++;
                        }
                        if (count == 1)
                        {
                            //  MessageBox.Show("Welcome ur Successfully Logged In");
                            var main = new MainWindow();
                            Close();
                            main.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Username or Password  or Usertype is Wrong try again");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        connection.Close();

                    }

                }
           
        }


        private void Remember() 
        {

        
            String query = string.Format("select sp_name,sp_password,sp_remember from shopkeeper where sp_remember='"+1+"'");
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string sname = reader[0].ToString();
                    
                    string spass = reader[1].ToString();
                    
                    bool  value = bool.Parse(reader[2].ToString());
                  
                    UsernameTextBox.Text = sname;

                    PasswordTextBox.Password = spass;

                    RememberCheckBox.IsChecked = value;
                  

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
 
        
        }
        
    }
}
