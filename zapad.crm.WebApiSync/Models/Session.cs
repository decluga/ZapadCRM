using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zapad.crm.WebApiSync.Helpers;

namespace zapad.crm.WebApiSync.Models
{
    public class Session
    {
        public Session(DateTime lifeTime, string ipAdress)
        {
            this.IsAuthentificated = false;
            SetIpAdress(ipAdress);
            ExpirationTime = lifeTime;

            var config = SelfHostConfigRetriever.GetHostConfig();
            LastQueries = new Queue<string>(Convert.ToInt32(config.GetElementByName("sizeQueueOfLastQueries").Value));
        }

        public bool IsAuthentificated { get; private set; }

        public long IpAdress { get; set; }

        public long FormattingIpAdress(string ip)
        {
            return Convert.ToInt64(new string(ip.Where(Char.IsDigit).ToArray()));
        }

        public void SetIpAdress(string ip)
        {
            IpAdress = FormattingIpAdress(ip);
        }

        public bool CompareIpAdress(string ip)
        {
            return IpAdress == FormattingIpAdress(ip);
        }

        public DateTime ExpirationTime { get; private set; }

        public DateTime LastActivity { get; set; }

        public Queue<string> LastQueries { get; set; }

        public string hashOfSms { get; set; }
    }
}
