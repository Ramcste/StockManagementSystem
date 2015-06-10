using System;
using System.Collections.Generic;
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
using StockManagementSystem;

namespace StationaryManagementSystem
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        public Report()
        {
            InitializeComponent();
            GetLoadSoldList();
            GetTotalCustomerList();

        }

        public static SqlConnection Conn;
        private void DatabaseCallMethod()
        {
            Conn = DatabaseConnection.GetConnection();

            if (Conn.State.ToString() == "closed")
            {
                Conn.Open();
            }
        }


        private void GetLoadSoldList()
        {

            DatabaseCallMethod();

            String query = string.Format("select so_id,so_itemsname,so_itemsedition,so_noofitems,so_totalprice,so_billno from sold ");

            try
            {

                DataSet ds = new DataSet();


                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(ds);

                BookSoldListDataGrid.DataContext = ds.Tables[0].DefaultView;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }  
        }


        private void GetTotalCustomerList()
        {

            DatabaseCallMethod();

            String query = string.Format("select cu_name,cu_mobileno,date,cu_billno from customer  ");

            try
            {

                DataSet ds = new DataSet();


                SqlDataAdapter adapter = new SqlDataAdapter();

                SqlCommand cmd = new SqlCommand(query, Conn);

                adapter.SelectCommand = cmd;

                adapter.Fill(ds);

                CustomerListDataGrid.DataContext = ds.Tables[0].DefaultView;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }  


        }
    }
}
