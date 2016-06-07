using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zapad.Model.Calls.DTO;

namespace zapad.Model.ViewModel
{
    public class CallViewModel
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Client { get; set; }
        public string DateTime { get; set; }
        public string CallReceiver { get; set; }
        public string IsRepeatCall { get; set; }
        public string EventStatus { get; set; }
        public string EventResultType { get; set; }
        public string DepartmentUserDepartment { get; set; }
        public string DepartmentUser { get; set; }

        public CallViewModel()
        {

        }

        public CallViewModel(CallDTO callDTO)
        {
            this.Id = callDTO.Id;
            this.Phone = callDTO.Phone;
            this.Client = callDTO.Client;
            this.DateTime = callDTO.DateTime.ToString("dd.MM.yyyy hh:mm");
            this.CallReceiver = callDTO.CallReceiver;
            this.IsRepeatCall = callDTO.IsRepeatCall ? "Да" : "Нет";
            this.EventStatus = callDTO.EventStatus;
            this.EventResultType = callDTO.EventResultType;
            this.DepartmentUser = callDTO.DepartmentUser;
            this.DepartmentUserDepartment = callDTO.DepartmentUserDepartment;
        }
    }

    public static class CallViewModelExtensions
    {
        public static List<CallViewModel> ViewModelList(this CallDTO[] callDTOs)
        {
            var result = new List<CallViewModel>();
            foreach (var callDTO in callDTOs)
                result.Add(new CallViewModel(callDTO));
            return result;
        }
    }
}
