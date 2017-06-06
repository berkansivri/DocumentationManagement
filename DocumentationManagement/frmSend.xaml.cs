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
using System.Net;
using System.Net.Mail;

namespace DocumentationManagement
{
    /// <summary>
    /// Interaction logic for frmSend.xaml
    /// </summary>
    public partial class frmSend : Window
    {
        public frmSend()
        {
            InitializeComponent();
        }

        private void btnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void btnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void topGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                MessageBox.Show("Email sent sucessfully!","E-mail",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void frmSend1_Loaded(object sender, RoutedEventArgs e)
        {
            this.Show();
            MessageBox.Show("This feature will come at 'Document Management v2'","Coming Soon",MessageBoxButton.OK,MessageBoxImage.Hand);
            this.Close();
        }
    }
}
