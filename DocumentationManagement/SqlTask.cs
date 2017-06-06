using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationManagement
{
    public class SqlTask
    {
        //OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\Users\x\Documents\Visual Studio 2015\Projects\DocumentationManagement\DocumentationManagement\document.accdb;Persist Security Info=False;");
        OleDbConnection conn = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\document.accdb;Persist Security Info=False;");
        DataTable dt = new DataTable();
        public List<Details> ListDocuments()
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT Details.Id, Details.Datee, Details.Title, Details.Clas, Details.Type, Details.State, Details.DocNumber, Details.DigitalCopy, Location.Borrower FROM Details, Location WHERE Details.Id=Location.Id", conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            List<Details> documentlist = new List<Details>();

            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Details doc = new Details()
                    {
                        Id = (int)dr["Id"],
                        Datee = (DateTime)dr["Datee"],
                        Title = dr["Title"].ToString(),
                        Clas = dr["Clas"].ToString(),
                        Type = dr["Type"].ToString(),
                        State = dr["State"].ToString(),
                        DocNumber = dr["DocNumber"].ToString(),
                        Borrower = dr["Borrower"].ToString(),
                    };
                    if (string.IsNullOrEmpty(dr["DigitalCopy"].ToString()) == false)
                    {
                        doc.DigitalCopy = "✔";
                        //●
                    }

                    documentlist.Add(doc);
                }
            }
            conn.Close();
            return documentlist;

            /* OleDbDataReader reader = cmd.ExecuteReader();
            List<string> lst = new List<string>();
            ListViewItem item = new ListViewItem();
            //ArrayList lst = new ArrayList(); 
            
            while (reader.Read())
            {
                lst.Add(reader["Title"].ToString() + "\t" + reader["Datee"].ToString() + " " + reader["Class"].ToString());
            }
            foreach (string detail in lst)
            {
                listdoc.Items.Add(detail);
            }
            conn.Close(); */
        }
        public Details AllDetails(int id)
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Details,Location,Submission WHERE Details.Id=@Id AND Submission.Id=@Id AND Location.Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            Details doc = new Details();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    {
                        doc.Datee = (DateTime)dr["Datee"];
                        doc.Title = dr["Title"].ToString();
                        doc.Clas = dr["Clas"].ToString();
                        doc.Type = dr["Type"].ToString();
                        doc.State = dr["State"].ToString();
                        doc.DocNumber = dr["DocNumber"].ToString();
                        doc.Author = dr["Author"].ToString();
                        doc.Status = dr["Status"].ToString();
                        doc.RepliedNumber = dr["RepliedNumber"].ToString();
                        doc.Description = dr["Description"].ToString();
                        doc.RoomNo = dr["RoomNo"].ToString();
                        doc.ShelfNo = dr["ShelfNo"].ToString();
                        doc.FileNo = dr["FileNo"].ToString();
                        doc.Borrower = dr["Borrower"].ToString();
                        doc.SubDatee = (DateTime)dr["SubDatee"];
                        doc.SubmittedBy = dr["SubmittedBy"].ToString();
                        doc.SubmittedTo = dr["SubmittedTo"].ToString();
                        doc.SubmittedBranch = dr["SubmittedBranch"].ToString();
                        doc.SubmitterBranch = dr["SubmitterBranch"].ToString();
                        doc.DigitalCopy = dr["DigitalCopy"].ToString();
                    }
                }
            }
            conn.Close();
            return doc;
        }
        public void DeleteDocument(int Id)
        {
            conn.Open();
            OleDbCommand delete = new OleDbCommand("DELETE FROM Details WHERE Id=@Id", conn);
            delete.Parameters.AddWithValue("Id", Id);
            delete.ExecuteNonQuery();
            conn.Close();
        }
        public bool CheckDocNo(Details doo)
        {
            OleDbCommand checkdocno = new OleDbCommand("SELECT DocNumber,Datee,Title,Type,Clas,State,Status,Author FROM Details", conn);
            OleDbDataAdapter docadapter = new OleDbDataAdapter(checkdocno);
            dt.Clear();
            docadapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if ((string.IsNullOrEmpty(dr["DocNumber"].ToString()) == false && doo.DocNumber == dr["DocNumber"].ToString()) || (dr["Clas"].ToString() == doo.Clas && dr["Datee"].ToString() == doo.Datee.ToString() && dr["Title"].ToString() == doo.Title && dr["Type"].ToString() == doo.Type && dr["Status"].ToString() == doo.Status && dr["State"].ToString() == doo.State && dr["Author"].ToString() == doo.Author && doo.DocNumber == dr["DocNumber"].ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool EditCheckDocNo(Details doo, Details selected)
        {
            if (selected.DocNumber == doo.DocNumber && selected.Clas == doo.Clas && selected.Datee == doo.Datee && selected.Title == doo.Title && selected.Type == doo.Type && selected.Status == doo.Status && selected.State == doo.State && selected.Author == doo.Author)
                return true;
            OleDbCommand checkdocno = new OleDbCommand("SELECT DocNumber,Datee,Title,Type,Clas,State,Status,Author FROM Details", conn);
            OleDbDataAdapter docadapter = new OleDbDataAdapter(checkdocno);
            dt.Clear();
            docadapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                        if ((string.IsNullOrEmpty(dr["DocNumber"].ToString()) == false && doo.DocNumber == dr["DocNumber"].ToString() && doo.DocNumber != selected.DocNumber) || (dr["Clas"].ToString() == doo.Clas && dr["Datee"].ToString() == doo.Datee.ToString() && dr["Title"].ToString() == doo.Title && dr["Type"].ToString() == doo.Type && dr["Status"].ToString() == doo.Status && dr["State"].ToString() == doo.State && dr["Author"].ToString() == doo.Author && doo.DocNumber == dr["DocNumber"].ToString()))
                            return false;
                }
            }
            return true;
        }
        public List<Details> Search(string where, string what)
        {
            conn.Open();
            OleDbCommand search = new OleDbCommand("SELECT Details.[Id], Details.[Datee], Details.[Title], Details.[Clas], Details.[Type], Details.[State], Details.[DocNumber] FROM Details WHERE " + where + " LIKE '%" + what + "%'", conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(search);
            dt.Clear();
            adapter.Fill(dt);
            List<Details> searchlist = new List<Details>();
            
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Details doc = new Details()
                    {
                        Id = (int)dr["Id"],
                        Datee = (DateTime)dr["Datee"],
                        Title = dr["Title"].ToString(),
                        Clas = dr["Clas"].ToString(),
                        Type = dr["Type"].ToString(),
                        State = dr["State"].ToString(),
                        DocNumber = dr["DocNumber"].ToString(),
                    };
                    searchlist.Add(doc);
                }
            }
            conn.Close();
            return searchlist;

        }
        public List<Details> SearchAll(string what)
        {
            conn.Open();
            OleDbCommand search = new OleDbCommand("SELECT Details.[Id], Details.[Datee], Details.[Title], Details.[Clas], Details.[Type], Details.[State], Details.[DocNumber] FROM Details WHERE Datee LIKE '%" + what + "%' OR Title LIKE '%" + what + "%' OR DocNumber LIKE '%" + what + "%' OR Clas LIKE '%" + what + "%' OR State LIKE '%" + what + "%' OR Type LIKE '%" + what + "%' OR Author LIKE '%" + what + "%' OR Status LIKE '%" + what + "%' OR Description LIKE '%" + what + "%'", conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(search);
            dt.Clear();
            adapter.Fill(dt);
            List<Details> searchlist = new List<Details>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Details doc = new Details()
                    {
                        Id = (int)dr["Id"],
                        Datee = (DateTime)dr["Datee"],
                        Title = dr["Title"].ToString(),
                        Clas = dr["Clas"].ToString(),
                        Type = dr["Type"].ToString(),
                        State = dr["State"].ToString(),
                        DocNumber = dr["DocNumber"].ToString(),
                    };
                    searchlist.Add(doc);
                }
            }
            conn.Close();
            return searchlist;
        }
        public DataRowCollection comboboxes()
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT Clas, Type FROM Details GROUP BY Clas, Type", conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            conn.Close();
            return dt.Rows;
        }
        public Details LastAdded()
        {
            Details lastadded = new Details();
            conn.Open();
            OleDbCommand last = new OleDbCommand("SELECT TOP 1 * FROM Submission, Location WHERE Submission.Id=Location.Id ORDER BY Submission.Id DESC, Location.Id DESC", conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter(last);
            dt.Clear();
            adapter.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    lastadded.SubmittedBy = dr["SubmittedBy"].ToString();
                    lastadded.SubmittedTo = dr["SubmittedTo"].ToString();
                    lastadded.SubmittedBranch = dr["SubmittedBranch"].ToString();
                    lastadded.SubmitterBranch = dr["SubmitterBranch"].ToString();
                    lastadded.RoomNo = dr["RoomNo"].ToString();
                    lastadded.ShelfNo = dr["ShelfNo"].ToString();
                }
            }
            conn.Close();
            return lastadded;
        }
        public List<Details> BorrowList()
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT Details.Id, Details.Title, Details.Clas, Details.Type, Location.Borrower FROM Details, Location WHERE Borrower Is Not NULL AND Borrower <> '' AND Details.Id = Location.Id; ", conn);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            List<Details> documentlist = new List<Details>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                        Details doc = new Details()
                        {
                            Id = (int)dr["Id"],
                            Title = dr["Title"].ToString(),
                            Clas = dr["Clas"].ToString(),
                            Type = dr["Type"].ToString(),
                            Borrower = dr["Borrower"].ToString(),
                        };
                        documentlist.Add(doc);
                    }
            }
            conn.Close();
            return documentlist;
        }
        public void DeleteBorrow(int Id)
        {
            conn.Open();
            OleDbCommand delete = new OleDbCommand("UPDATE Location SET Borrower = null WHERE Id = @Id", conn);
            delete.Parameters.AddWithValue("Id", Id);
            delete.ExecuteNonQuery();
            conn.Close();
        }
        public bool CheckReply(Details doo)
        {
            OleDbCommand checkdocno = new OleDbCommand("SELECT DocNumber FROM Details", conn);
            OleDbDataAdapter docadapter = new OleDbDataAdapter(checkdocno);
            dt.Clear();
            docadapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr["DocNumber"].ToString()) == false && doo.RepliedNumber == dr["DocNumber"].ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Details GetReplyDocument(string RepliedNumber)
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Details,Location,Submission WHERE Details.DocNumber=@RepliedNumber", conn);
            cmd.Parameters.AddWithValue("@RepliedNumber", RepliedNumber);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            Details doc = new Details();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    {
                        doc.Datee = (DateTime)dr["Datee"];
                        doc.Title = dr["Title"].ToString();
                        doc.Clas = dr["Clas"].ToString();
                        doc.Type = dr["Type"].ToString();
                        doc.State = dr["State"].ToString();
                        doc.DocNumber = dr["DocNumber"].ToString();
                        doc.Author = dr["Author"].ToString();
                        doc.Status = dr["Status"].ToString();
                        doc.RepliedNumber = dr["RepliedNumber"].ToString();
                        doc.Description = dr["Description"].ToString();
                        doc.RoomNo = dr["RoomNo"].ToString();
                        doc.ShelfNo = dr["ShelfNo"].ToString();
                        doc.FileNo = dr["FileNo"].ToString();
                        doc.Borrower = dr["Borrower"].ToString();
                        doc.SubDatee = (DateTime)dr["SubDatee"];
                        doc.SubmittedBy = dr["SubmittedBy"].ToString();
                        doc.SubmittedTo = dr["SubmittedTo"].ToString();
                        doc.SubmittedBranch = dr["SubmittedBranch"].ToString();
                        doc.SubmitterBranch = dr["SubmitterBranch"].ToString();
                        doc.DigitalCopy = dr["DigitalCopy"].ToString();
                    }
                }
            }
            conn.Close();
            return doc;
        }
        public string GetDigitalCopy(string DocNumber)
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT DigitalCopy FROM Details WHERE Details.DocNumber=@Docnumber", conn);
            cmd.Parameters.AddWithValue("@DocNumber", DocNumber);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            dt.Clear();
            adapter.Fill(dt);
            Details doc = new Details();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    {
                        doc.DigitalCopy = dr["DigitalCopy"].ToString();
                    }
                }
            }
            conn.Close();
            return doc.DigitalCopy;
        }
    }
}
