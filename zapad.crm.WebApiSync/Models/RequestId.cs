using System;

namespace zapad.crm.WebApiSync.Models
{
    public class RequestId
    {
        private Guid guid;
        public Guid GUID
        {
            get { return guid; }
        }

        public RequestId(Guid guid)
        {
            this.guid = guid;
        }
    }
}
