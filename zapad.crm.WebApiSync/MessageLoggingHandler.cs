using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zapad.crm.WebApiSync.Helpers;
using zapad.crm.WebApiSync.Models;

namespace zapad.crm.WebApiSync
{
    public class MessageLoggingHandler : DelegatingHandler
    {
        /// <summary>
        /// Срабатывает при получении запроса.
        /// Заполняет логи, работает с куками и сессиями.
        /// </summary>
        /// <param name="request">Пришедший запрос</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestId = new RequestId(Guid.NewGuid()); //установка уникального номера запроса чтобы можно было идентифицировать его при обработке ответа
            request.Properties.Add(Constants.RequestIdKey, requestId);
            LogWriter.AddRequestToLog(ref request, requestId);

            var response = await base.SendAsync(request, cancellationToken);
            byte[] responseMessage = await response.Content.ReadAsByteArrayAsync();
            var responseText = Encoding.UTF8.GetString(responseMessage);

            //обработка куки и сессий
            Guid sessionGuid = Guid.NewGuid();
            string ipRemoteUser = request.GetOwinContext().Request.RemoteIpAddress;
            var cookie = CookieWorker.GetSessionGuidFromCookies(request.Headers);
            if (cookie != null)
            {
                try
                {
                    sessionGuid = Guid.Parse(cookie[Constants.SessionIdKeyInCookies].Value);
                }
                catch
                {
                    Debug.WriteLine("Не удалось распарсить Guid");
                }
            }

            if (!SessionStore.ContainsKey(sessionGuid))
            {   //возможна ситуация - если получили guid из кук и его не оказалось в коллекции,то guid остаётся тем же и перезаписывается.Очень маловероятная ситуация если отдавать куки со сроком действия
                CookieWorker.AddSessionIdToCookies(sessionGuid, response);

                SessionStore.AddSession(sessionGuid, new Session(
                    SessionStore.GetTimeOutdateOfSession(),
                    ipRemoteUser));
            }

            SessionStore.UpdateLastActivity(sessionGuid);
            SessionStore.AddQueryToLastQueries(sessionGuid, request.RequestUri.ToString());

            if (!SessionStore.IsTheSameIp(sessionGuid, ipRemoteUser))
            {
                throw new Exception("Сменился ip на " + ipRemoteUser);
            }

            LogWriter.AddResponseToLogs(requestId.GUID, response.IsSuccessStatusCode, response.StatusCode, responseText);
            //LogStore.WriteContentToOutput();

            return response;
        }
    }
}
