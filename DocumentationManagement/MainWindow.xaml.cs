using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
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

namespace DocumentationManagement
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

        #region Window

        private void mainkapat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void minimize_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        SqlTask task = new SqlTask();
        void doc_Closed(object sender, EventArgs e)
        {
            anapencere.IsEnabled = true;
            listdoc.SelectedIndex = -1;
            List<Details> docdetails = task.ListDocuments();
            listdoc.ItemsSource = docdetails;
            BorrowListButton.IsEnabled = false;
            foreach (Details det in docdetails)
            {
                if (string.IsNullOrEmpty(det.Borrower) == false)
                    BorrowListButton.IsEnabled = true;  
            }
        }
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listdoc.Items.Clear();
            List<Details> docdetails = task.ListDocuments();
            
            listdoc.ItemsSource = docdetails;
            BorrowListButton.IsEnabled = false;
            BorrowListButton.Visibility = Visibility.Collapsed;
            foreach (Details det in docdetails)
            {
                if (string.IsNullOrEmpty(det.Borrower) == false)
                    BorrowListButton.IsEnabled = true;
            }
        }
        private void btnNew_MouseUp(object sender, MouseButtonEventArgs e)
        {
            newDoc neww = new newDoc();
            neww.Show();
            anapencere.IsEnabled = false;
            neww.Closed += doc_Closed;
        }
        public void btnDuzenlee()
        {
                editDoc edit = new editDoc();
                Details doc = listdoc.SelectedItem as Details;
                edit.selected = doc;
                edit.DigitalCopy = doc.DigitalCopy;
                edit.Show();
                anapencere.IsEnabled = false;
                edit.Closed += doc_Closed;
        }
        public void btnSill()
        {
            Details doc = listdoc.SelectedItem as Details;
            MessageBoxResult result = MessageBox.Show("Are you sure about the remove " + doc.Title + " document ?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                task.DeleteDocument(doc.Id);
            }
            else
            {
                return;
            }
            
            List<Details> docdetails = task.ListDocuments();
            listdoc.ItemsSource = docdetails;
            BorrowListButton.IsEnabled = false;
            foreach (Details det in docdetails)
            {
                if (string.IsNullOrEmpty(det.Borrower) == false)
                    BorrowListButton.IsEnabled = true;
            }

        }
        void btnDuzenle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Document for Modify", "Modify", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            btnDuzenlee();
        }
        private void btnSil_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
            {
                MessageBox.Show("Select a Document for Remove", "Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            btnSill();
        }
        private void searchtask()
        {
            List<Details> searchlist = new List<Details>();
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                switch (cmbSearch.SelectedIndex)
                {
                    case 0:
                        searchlist = task.SearchAll(txtSearch.Text);
                        break;
                    case 1:
                        searchlist = task.Search("DocNumber", txtSearch.Text);
                        break;
                    case 2:
                        searchlist = task.Search("Datee", txtSearch.Text);
                        break;
                    case 3:
                        searchlist = task.Search("Title", txtSearch.Text);
                        break;
                    case 4:
                        searchlist = task.Search("Clas", txtSearch.Text);
                        break;
                    case 5:
                        searchlist = task.Search("Type", txtSearch.Text);
                        break;
                    case 6:
                        searchlist = task.Search("State", txtSearch.Text);
                        break;
                    case 7:
                        searchlist = task.Search("Author", txtSearch.Text);
                        break;
                    case 8:
                        searchlist = task.Search("Status", txtSearch.Text);
                        break;
                    case 9:
                        searchlist = task.Search("Description", txtSearch.Text);
                        break;
                    default: return;
                }
                listdoc.ItemsSource = searchlist;
            }
            else
            {
                List<Details> docdetails = task.ListDocuments();
                listdoc.ItemsSource = docdetails;
            }
        }
        private void btnSearch_MouseUp(object sender, MouseButtonEventArgs e)
        {
            searchtask();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchtask();
        }
        private void CmDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
            {
                return;
            }
            btnSill();
        }
        private void cmModify_Click(object sender, RoutedEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
            {
                return;
            }
            else
                btnDuzenlee();
        }
        private void cmDetails_Click(object sender, RoutedEventArgs e)
        {
            if (listdoc.SelectedIndex == -1)
                return;
            else
            {
                Details doc = listdoc.SelectedItem as Details;
                ShowDoc sd = new ShowDoc();
                sd.Id = doc.Id;
                sd.Show();
                sd.Closed += doc_Closed;
            }
        }
        private void BorrowListButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BorrowList brw = new BorrowList();
            brw.Show();
            brw.Closed += doc_Closed;
        }
        private void BorrowListButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(BorrowListButton.IsEnabled == false)
                BorrowListButton.Visibility = Visibility.Collapsed;
            else
                BorrowListButton.Visibility = Visibility.Visible;
        }
        private void cmDigitalCopy_Click(object sender, RoutedEventArgs e)
        {
            Details selected = listdoc.SelectedItem as Details;
            string DigitalCopy = task.GetDigitalCopy(selected.DocNumber);
            if (string.IsNullOrEmpty(DigitalCopy) == false)
            {
                try
                {
                    if (File.Exists(DigitalCopy))
                        Process.Start(new ProcessStartInfo(DigitalCopy));
                }
                catch (Exception)
                {
                }
            }
            else
                MessageBox.Show("This document does not have a digital copy", "No File", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void btnYolla_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            frmSend send = new frmSend();
            send.Show();
        }
        
    }
}
