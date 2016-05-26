using System;

namespace zapad.crm.WebHostCache.Models
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
