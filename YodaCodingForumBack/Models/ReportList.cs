using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class ReportList
    {
        public string ReportId { get; set; }
        public string UserId { get; set; }
        public string UserAccount { get; set; }
        public string ReportTargetId { get; set; }
        public string ReportTargetType { get; set; }
        public string ReasonCode { get; set; }
        public string ReportRemarks { get; set; }
        public string ReportContents { get; set; }
        public string ReportStatus { get; set; }
        public string ReportVerifyPerson { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string LastupdateUser { get; set; }
        public DateTime LastupdateDate { get; set; }
        public int Version { get; set; }
        public string UserName { get; set; }
    }
}
