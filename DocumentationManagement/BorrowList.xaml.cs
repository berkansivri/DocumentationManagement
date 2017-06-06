using System;
using System.Collections.Generic;
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

namespace DocumentationManagement
{
    /// <summary>
    /// Interaction logic for BorrowList.xaml
    /// </summary>
    public partial class BorrowList : Window
    {
        public BorrowList()
        {
            InitializeComponent();
        }
        SqlTask task = new SqlTask();
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void kapat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void frmBorrow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Details> borrowlist = new List<Details>();
            borrowlist = task.BorrowList();
            listdoc.ItemsSource = borrowlist;
            listdoc.SelectedIndex = -1;
            btnBorrowNew.Visibility = Visibility.Collapsed;
        }
        private void doc_Closed(object sender, EventArgs e)
        {
            frmBorrow.IsEnabled = true;
            List<Details> borrowlist = task.BorrowList();
            listdoc.ItemsSource = borrowlist;
            listdoc.SelectedIndex = -1;
        }
        private void btnBorrowNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            newBorrow neww = new newBorrow();
            neww.Show();
            frmBorrow.IsEnabled = false;
            neww.Closed += doc_Closed;
        }
        private void btnBorrowEdit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Document for Modify", "Modify", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                Details doc = listdoc.SelectedItem as Details;
                editDoc edit = new editDoc();
                edit.selected = doc;
                frmBorrow.IsEnabled = false;
                edit.Show();
                edit.Closed += doc_Closed;
            }
        }
        private void btnBorrowDelete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Details doc = listdoc.SelectedItem as Details;
            if (listdoc.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Document for Remove", "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure about the remove " + doc.Title + " document ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SqlTask task = new SqlTask();
                task.DeleteBorrow(doc.Id);
                List<Details> borrowlist = task.BorrowList();
                listdoc.ItemsSource = borrowlist;
                listdoc.SelectedIndex = -1;
            }
            else
            {
                return;
            }
            
        }
        private void listdoc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
                return;
            else
            {
                Details doc = listdoc.SelectedItem as Details;
                ShowDoc sd = new ShowDoc();
                sd.Id = doc.Id;
                sd.Show();
            }
        }

       
    }
}
