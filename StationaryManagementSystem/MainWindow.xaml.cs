using System.Windows;
using System.Windows.Controls;
using StationaryManagementSystem;

namespace StockManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            MessageBoxResult key = MessageBox.Show("Are you sure you want to quit?", "Confirm", MessageBoxButton.YesNo,
                MessageBoxImage.Question, MessageBoxResult.No);
            e.Cancel = (key == MessageBoxResult.No);


        }

 private void Book_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Children.Clear();
            Book book = new Book();
            ContentControl.Children.Add(book);
               


            
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {

            ContentControl.Children.Clear();
            Other other = new Other();
            ContentControl.Children.Add(other);
        }

       

        private void Stock_Click(object sender, RoutedEventArgs e)
        {

            ContentControl.Children.Clear();
            Stock stock = new Stock();
            ContentControl.Children.Add(stock);
        }

        private void Action_Click(object sender, RoutedEventArgs e)
        {

            ContentControl.Children.Clear();
            Action action = new Action();
            ContentControl.Children.Add(action);
        }

      
        private void Developer_Click(object sender, RoutedEventArgs e)
        {

            ContentControl.Children.Clear();
            Developer developer = new Developer();
            ContentControl.Children.Add(developer);
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Children.Clear();
            Report rpt=new Report();
            ContentControl.Children.Add(rpt);
        }

        private void AboutSystem_Click(object sender, RoutedEventArgs e)
        {
            ContentControl.Children.Clear();
            AboutSystem abt = new AboutSystem();
            ContentControl.Children.Add(abt);
            
        }
        
    }
}
