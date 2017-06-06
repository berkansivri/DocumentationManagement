using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationManagement
{
    public class Details
    {
        public int Id { get; set; }
        public DateTime Datee { get; set; }
        public string Title { get; set; }
        public string Clas { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string DocNumber { get; set; }
        public string RepliedNumber { get; set; }
        public string Description { get; set; }

        public string RoomNo { get; set; }
        public string ShelfNo { get; set; }
        public string FileNo { get; set; }
        public string Borrower { get; set; }

        public DateTime SubDatee { get; set; }
        public string SubmittedBy { get; set; }
        public string SubmittedTo { get; set; }
        public string SubmitterBranch { get; set; }
        public string SubmittedBranch { get; set; }

        public string DigitalCopy { get; set; }
    }
}
