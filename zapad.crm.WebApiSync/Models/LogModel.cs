namespace zapad.crm.WebApiSync.Models
{
    public class LogModel
    {
        public int WebAccountId { get; set; } = 0;

        public string RequestInfo { get; set; } = "";

        public string RequestStart { get; set; } = "";

        public string RequestFinished { get; set; } = "";

        public int AuthorizationCause { get; set; } = -1;

        public int IsException { get; set; } = 0;

        public string Answer { get; set; } = "";

        public string AuditInfo { get; set; } = "";

        public bool ReadyToDB { get; set; } = false;

        public LogModel()
        { }

        public LogModel(int webAccountId, string requestInfo, string requestStart)
        {
            WebAccountId = webAccountId;
            RequestInfo = requestInfo;
            RequestStart = requestStart;
        }

        public void SuccessResponse(string requestFinished, string answer)
        {
            RequestFinished = requestFinished;
            Answer = answer;
            ReadyToDB = true;
        }

        public void ExceptionResponse(string requestFinished, string auditInfo)
        {
            RequestFinished = requestFinished;
            IsException = 1;
            AuditInfo = auditInfo;
            ReadyToDB = true;
        }

        public void ErrorResponse(string requestFinished, string answer)
        {
            RequestFinished = requestFinished;
            Answer = answer;
        }
    }
}
