using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
    /// Interaction logic for editDoc.xaml
    /// </summary>

    public partial class editDoc : Window
    {
        OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\document.accdb;Persist Security Info=False;");
        SqlTask task = new SqlTask();
        public editDoc()
        {
            InitializeComponent();
        }

        private void kapatt_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public Details selected;
        public string DigitalCopy = "";
        private void btnModify_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txtClass.Text) || string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtType.Text) || dtpDate.SelectedDate == null || dtpSubmission.SelectedDate == null)
            {
                return;
            }
            else if ((string.IsNullOrEmpty(txtSubBy.Text) && string.IsNullOrEmpty(txtSurBra.Text)) || (string.IsNullOrEmpty(txtSubTo.Text) && string.IsNullOrEmpty(txtSudBra.Text)))
            {
                return;
            }

            Details det = new Details()
            {
                Datee = dtpDate.SelectedDate.Value.Date,
                Title = txtTitle.Text,
                Clas = txtClass.Text,
                Type = txtType.Text,
                State = cmbState.SelectionBoxItem.ToString(),
                Author = txtAuthor.Text,
                Status = cmbStatus.SelectionBoxItem.ToString(),
                DocNumber = txtDocNo.Text,
                RepliedNumber = txtReplied.Text,
                Description = txtDesc.Text,
                SubDatee = dtpSubmission.SelectedDate.Value.Date,
                SubmittedBy = txtSubBy.Text,
                SubmittedTo = txtSubTo.Text,
                SubmitterBranch = txtSurBra.Text,
                SubmittedBranch = txtSudBra.Text,
                RoomNo = txtRoomNo.Text,
                ShelfNo = txtShelfNo.Text,
                FileNo = txtFileNo.Text,
                Borrower = txtBorrower.Text,
                DigitalCopy = selected.DigitalCopy,
            };
            OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\document.accdb;Persist Security Info=False;");
            Details selectedd = task.AllDetails(selected.Id);
            if (task.EditCheckDocNo(det, selectedd) == true)
            {
                if(string.IsNullOrEmpty(det.RepliedNumber) == true || task.CheckReply(det) == true)
                {
                    try
                    {
                        conn.Open();
                        OleDbCommand update1 = new OleDbCommand("UPDATE Details SET Datee=@Datee, Title=@Title, Clas=@Clas, Type=@Type, State=@State, Author=@Author, Status=@Status, DocNumber=@DocNumber, RepliedNumber=@RepliedNumber, Description=@Description, DigitalCopy=@DigitalCopy WHERE Id=@Id", conn);
                        OleDbCommand update2 = new OleDbCommand("UPDATE Submission SET SubDatee=@SubDatee, SubmittedBy=@SubmittedBy, SubmittedTo=@SubmittedTo, SubmitterBranch=@SubmitterBranch, SubmittedBranch=@SubmittedBranch WHERE Id=@Id", conn);
                        OleDbCommand update3 = new OleDbCommand("UPDATE Location SET RoomNo=@RoomNo, ShelfNo=@ShelfNo, FileNo=@FileNo, Borrower=@Borrower WHERE Id=@Id", conn);

                        update1.Parameters.AddWithValue("Datee", OleDbType.Date).Value = det.Datee;
                        update1.Parameters.AddWithValue("Title", OleDbType.VarChar).Value = det.Title;
                        update1.Parameters.AddWithValue("Clas", OleDbType.VarChar).Value = det.Clas;
                        update1.Parameters.AddWithValue("Type", OleDbType.VarChar).Value = det.Type;
                        update1.Parameters.AddWithValue("State", OleDbType.VarChar).Value = det.State;
                        update1.Parameters.AddWithValue("Author", OleDbType.VarChar).Value = det.Author; ;
                        update1.Parameters.AddWithValue("Status", OleDbType.VarChar).Value = det.Status;
                        update1.Parameters.AddWithValue("DocNumber", OleDbType.VarChar).Value = det.DocNumber;
                        update1.Parameters.AddWithValue("RepliedNumber", OleDbType.VarChar).Value = det.RepliedNumber;
                        update1.Parameters.AddWithValue("Description", OleDbType.LongVarChar).Value = det.Description;
                        update1.Parameters.AddWithValue("DigitalCopy", OleDbType.VarChar).Value = det.DigitalCopy;
                        update1.Parameters.AddWithValue("Id", selected.Id);

                        update2.Parameters.AddWithValue("Datee", det.SubDatee);
                        update2.Parameters.AddWithValue("SubmittedBy", det.SubmittedBy);
                        update2.Parameters.AddWithValue("SubmittedTo", det.SubmittedTo);
                        update2.Parameters.AddWithValue("SubmitterBranch", det.SubmitterBranch);
                        update2.Parameters.AddWithValue("SubmittedBranch", det.SubmittedBranch);
                        update2.Parameters.AddWithValue("Id", selected.Id);

                        update3.Parameters.AddWithValue("RoomNo", det.RoomNo);
                        update3.Parameters.AddWithValue("ShelfNo", det.ShelfNo);
                        update3.Parameters.AddWithValue("FileNo", det.FileNo);
                        update3.Parameters.AddWithValue("Borrower", det.Borrower);
                        update3.Parameters.AddWithValue("Id", selected.Id);

                        update1.ExecuteNonQuery();
                        update2.ExecuteNonQuery();
                        update3.ExecuteNonQuery();

                        conn.Close();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        conn.Close();
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("There is no document number like written in 'Reply Document Number'", "Reply Document Number Doesn't Exist", MessageBoxButton.OK, MessageBoxImage.Information);
                    imgReply.Source = new BitmapImage(new Uri(@"\lib\docno.png", UriKind.Relative));
                }
            }

            else
            {
                if (string.IsNullOrEmpty(det.DocNumber))
                {
                    MessageBox.Show("This Document Already Saved", "Dublication Document Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("The Document Number* Already Saved", "Dublication Document Number", MessageBoxButton.OK, MessageBoxImage.Information);
                    imgDocNumber.Source = new BitmapImage(new Uri(@"\lib\docno.png", UriKind.Relative));
                }
            }
        }

        private void frmModify_Loaded_1(object sender, RoutedEventArgs e)
        {
            Details doc = task.AllDetails(selected.Id);
            if (doc != null)
            {
                dtpDate.SelectedDate = doc.Datee;
                txtTitle.Text = doc.Title;
                txtClass.Text = doc.Clas;
                txtType.Text = doc.Type;
                txtAuthor.Text = doc.Author;
                cmbState.SelectedIndex = doc.State == "Physical" ? 0 : 1;
                cmbStatus.SelectedIndex = doc.Status == "Received" ? 0 : 1;
                txtDesc.Text = doc.Description;
                txtDocNo.Text = doc.DocNumber;
                txtReplied.Text = doc.RepliedNumber;
                selected.DigitalCopy = doc.DigitalCopy;

                if(string.IsNullOrEmpty(doc.DigitalCopy) == false)
                {
                    Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(doc.DigitalCopy);
                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        var stream = new MemoryStream();
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        imgFile.Source = BitmapFrame.Create(stream);
                    }
                }

                dtpSubmission.SelectedDate = doc.SubDatee;
                txtSubTo.Text = doc.SubmittedTo;
                txtSubBy.Text = doc.SubmittedBy;
                txtSurBra.Text = doc.SubmitterBranch;
                txtSudBra.Text = doc.SubmittedBranch;

                txtRoomNo.Text = doc.RoomNo;
                txtShelfNo.Text = doc.ShelfNo;
                txtFileNo.Text = doc.FileNo;
                txtBorrower.Text = doc.Borrower;

                DataRowCollection dr = task.comboboxes();
                foreach (DataRow d in dr)
                {
                    txtClass.Items.Add(d["Clas"].ToString());
                    txtType.Items.Add(d["Type"].ToString());
                }
            }
            else
            {
                MessageBox.Show("empty!");
            }
            if (string.IsNullOrEmpty(txtTitle.Text) == false)
                imgTitle.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            if (string.IsNullOrEmpty(txtType.Text) == false)
                imgType.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            if (string.IsNullOrEmpty(dtpDate.Text) == false)
                imgDate.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            if (string.IsNullOrEmpty(txtClass.Text) == false)
                imgClass.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            if (string.IsNullOrEmpty(dtpSubmission.Text) == false)
                imgSubDate.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            if (string.IsNullOrEmpty(txtSubBy.Text) == false || string.IsNullOrEmpty(txtSurBra.Text) == false)
            {
                imgSubmittedBy.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSurBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
            if (string.IsNullOrEmpty(txtSubTo.Text) == false || string.IsNullOrEmpty(txtSudBra.Text) == false)
            {
                imgSubmittedTo.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSudBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
        }
        #region Stars
        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
                imgTitle.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            else
                imgTitle.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
        }

        private void txtClass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtClass.Text))
                imgClass.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            else
                imgClass.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
        }

        private void txtType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtType.Text))
                imgType.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            else
                imgType.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
        }

        private void dtpDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDate.SelectedDate == null)
                imgDate.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            else
                imgDate.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
        }

        private void dtpSubmission_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpSubmission.SelectedDate == null)
                imgSubDate.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            else
                imgSubDate.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
        }

        private void txtSubBy_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubBy.Text) && string.IsNullOrEmpty(txtSurBra.Text))
            {
                imgSubmittedBy.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
                imgSurBranch.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            }
            else
            {
                imgSubmittedBy.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSurBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
        }

        private void txtSubTo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubTo.Text) && string.IsNullOrEmpty(txtSudBra.Text))
            {
                imgSudBranch.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
                imgSubmittedTo.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            }
            else
            {
                imgSubmittedTo.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSudBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
        }

        private void txtSurBra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubBy.Text) && string.IsNullOrEmpty(txtSurBra.Text))
            {
                imgSurBranch.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
                imgSubmittedBy.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            }
            else
            {
                imgSurBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSubmittedBy.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
        }

        private void txtSudBra_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubTo.Text) && string.IsNullOrEmpty(txtSudBra.Text))
            {
                imgSudBranch.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
                imgSubmittedTo.Source = new BitmapImage(new Uri(@"\lib\red_star-16.png", UriKind.Relative));
            }
            else
            {
                imgSudBranch.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
                imgSubmittedTo.Source = new BitmapImage(new Uri(@"\lib\star-16.png", UriKind.Relative));
            }
        }
        #endregion

        private void dtpDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
        private void dtpSubmission_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void txtClass_TextChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cmBox = (System.Windows.Controls.ComboBox)sender;
            var textBox = (cmBox.Template.FindName("PART_EditableTextBox", cmBox) as TextBox);
            if (textBox.Text.Length > 0)
            {
                textBox.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txtClass.Text.ToLower());
                textBox.SelectionStart = txtClass.Text.Length;
            }
        }

        private void txtType_TextChanged(object sender, RoutedEventArgs e)
        {
            ComboBox cmBox = (System.Windows.Controls.ComboBox)sender;
            var textBox = (cmBox.Template.FindName("PART_EditableTextBox", cmBox) as TextBox);
            if (textBox.Text.Length > 0)
            {
                textBox.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txtType.Text.ToLower());
                textBox.SelectionStart = txtType.Text.Length;
            }
        }

        private void txtDocNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDocNo.Text))
                imgDocNumber.Source = new BitmapImage(new Uri(@"\lib\asd.png", UriKind.Relative));
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            filepath();
        }
        public void filepath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Word Files(*.docx;*.doc)|*.docx;*.doc|PDF File(*.pdf)|*.pdf|Excel File(*.xlsx)|*.xlsx|Powerpoint File (*.pptx)|*pptx|Text File(*.txt)|*.txt|Web Pages(*.htm;*.html)|*.htm;*.html|JPEG Picture(*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Picture(*.png)|*.png|BMP Picture(*.bmp)|*.bmp|All Pictures (*.jpg;*.jpeg;*.png;*.bmp;*.thm)|*.jpg;*.jpeg;*.png;*.bmp;*.thm";
            ofd.FilterIndex = 0;
            ofd.Title = "Select File(s)";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (ofd.ShowDialog() == true)
            {
                Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(ofd.FileName);
                using (Bitmap bmp = icon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    imgFile.Source = BitmapFrame.Create(stream);
                }
            }
            selected.DigitalCopy = ofd.FileName;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            imgFile.Source = null;
            selected.DigitalCopy = string.Empty;
        }

        private void imgFile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(selected.DigitalCopy) == false)
            {
                try
                {
                    if (File.Exists(selected.DigitalCopy))
                        Process.Start(new ProcessStartInfo(selected.DigitalCopy));
                }
                catch (Exception)
                {
                }
            }
        }

        private void txtReplied_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtReplied.Text))
                imgReply.Source = new BitmapImage(new Uri(@"\lib\asd.png", UriKind.Relative));
        }
    }
}
