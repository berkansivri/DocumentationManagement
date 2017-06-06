using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Shapes;

namespace DocumentationManagement
{
    /// <summary>
    /// Interaction logic for ShowDoc.xaml
    /// </summary>
    public partial class ShowDoc : Window
    {
        public ShowDoc()
        {
            InitializeComponent();
        }

        private void btnKapat_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void btnOK_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
            
        }
        private void label3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public int Id;
        public string RepliedNumber;
        SqlTask task = new SqlTask(); 
        private void frmDetails_Loaded(object sender, RoutedEventArgs e)
        {
            lblId.Visibility = Visibility.Hidden;
            Details doc = task.AllDetails(Id);
            OpenWindow(doc);
        }
        private void OpenWindow(Details doc)
        {
            tbDocNumber.Text = doc.DocNumber;
            tbDate.Text = doc.Datee.ToShortDateString();
            tbTitle.Text = doc.Title;
            tbType.Text = doc.Type;
            tbState.Text = doc.State;
            tbStatus.Text = doc.Status;
            tbClass.Text = doc.Clas;
            tbAuthor.Text = doc.Author;
            tbRepliedNo.Text = doc.RepliedNumber;
            tbDescription.Text = doc.Description;

            tbSubDate.Text = doc.SubDatee.ToShortDateString();
            tbSubTo.Text = doc.SubmittedTo;
            tbSubBy.Text = doc.SubmittedBy;
            tbSurBra.Text = doc.SubmitterBranch;
            tbSudBra.Text = doc.SubmittedBranch;

            tbRoomNo.Text = doc.RoomNo;
            tbShelfNo.Text = doc.ShelfNo;
            tbFileNo.Text = doc.FileNo;
            tbBorrower.Text = doc.Borrower;

            if (string.IsNullOrEmpty(doc.DigitalCopy) == false)
            {
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(doc.DigitalCopy);
                using (Bitmap bmp = icon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    imgFile.Source = BitmapFrame.Create(stream);
                }
            }
            if (string.IsNullOrEmpty(doc.RepliedNumber) == false)
            {
                btnReply.Visibility = Visibility.Visible;
                RepliedNumber = doc.RepliedNumber;
            }
            else
                btnReply.Visibility = Visibility.Hidden;
        }
        private void btnReply_Click(object sender, RoutedEventArgs e)
        {
            ShowDoc sd = new ShowDoc();
            sd.Show();
            Details ReplyDoc = task.GetReplyDocument(RepliedNumber);
            sd.OpenWindow(ReplyDoc);
        }
        private void imgFile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            String DigitalCopy = task.GetDigitalCopy(tbDocNumber.Text);
            try
            {
                if (File.Exists(DigitalCopy))
                    Process.Start(new ProcessStartInfo(DigitalCopy));
            }
            catch (Exception)
            {
            }
        }
    }
}
