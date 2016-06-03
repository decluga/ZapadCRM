using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zapad.Public.WebInterface.Models.ViewModels
{
    public class CallDTO
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Client { get; set; }
        public string DateTime { get; set; }
        public string CallReceiver { get; set; }
        public string IsRepeatCall { get; set; }
        public string Status { get; set; }
        public string CallResultType { get; set; }
        public string DepartmentUserDepartment { get; set; }
        public string DepartmentUser { get; set; }
    }
}