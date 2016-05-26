using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using zapad.crm.WebHostCache.Helpers;
using System.Web.Http.ExceptionHandling;

namespace zapad.crm.WebHostCache.Models
{
    /// <summary>
    /// Работает с классом LogStore - записывает напрямую логи.
    /// </summary>
    public static class LogWriter
    {
        /// <summary>
        /// Устанавливает уникальный Guid запроса,создаёт и добавляет запись лога в хранилище(неполная информация,без ответа)
        /// </summary>
        /// <param name="request"></param>
        public static void AddRequestToLog(ref HttpRequestMessage request, RequestId requestId)
        {
            var requestParameters = GetRequestParameters(request);
            int userId = GetUserIdFromReqParameters(requestParameters);
            XElement requestInfo = GetRequestInfo(request, requestParameters);

            if (!LogStore.Logs.ContainsKey(requestId.GUID))
            {
                LogStore.Logs.Add(requestId.GUID,
                new LogModel(
                    userId,
                    requestInfo.ToString(),
                    DateTime.Now.ToString(Constants.DateFormat))
                    );
            }
            else
            {
                LogStore.Logs[requestId.GUID].ExceptionResponse(
                    DateTime.Now.ToString(Constants.DateFormat),
                    "Execute exception before adding request to logs."
                    );
            }
        }

        /// <summary>
        /// Находит нужную запись в хранилище логов и заполняет данными ответа на запрос.
        /// </summary>
        /// <param name="requestGuid"></param>
        /// <param name="isSuccessStatusCode"></param>
        /// <param name="statusCode"></param>
        /// <param name="responseText"></param>
        public static void AddResponseToLogs(Guid requestGuid, bool isSuccessStatusCode, HttpStatusCode statusCode, string responseText)
        {
            if (isSuccessStatusCode)
            {
                LogStore.Logs[requestGuid].SuccessResponse(
                    DateTime.Now.ToString(Constants.DateFormat),
                    responseText
                    );
            }
            else
            {
                LogStore.Logs[requestGuid].ErrorResponse(
                    DateTime.Now.ToString(Constants.DateFormat),
                    responseText
                    );

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    LogStore.Logs[requestGuid].AuthorizationCause = 1;
                }
            }
        }

        /// <summary>
        /// Создаёт XElement из объекта запроса.
        /// </summary>
        /// <param name="request">Request to api</param>
        /// <param name="requestParameters">Параметры запроса приведенные к словарю <string,string></param>
        /// <returns></returns>
        public static XElement GetRequestInfo(HttpRequestMessage request, Dictionary<string, string> requestParameters)
        {
            XElement requestInfo = new XElement("rq",
                    new XElement("uri", request.RequestUri.AbsolutePath),
                    new XElement("fullPath", request.RequestUri.AbsoluteUri),
                    new XElement("m", request.Method.Method));
            XElement parameters = new XElement("parameters");
            foreach (var pairParameters in requestParameters)
            {
                XElement param = new XElement("param",
                    new XElement("name", pairParameters.Key),
                    new XElement("value", pairParameters.Value));
                parameters.Add(param);
            }
            requestInfo.Add(parameters);

            return requestInfo;
        }


        public static Dictionary<string, string> GetRequestParameters(HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs().
                ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        public static int GetUserIdFromReqParameters(Dictionary<string, string> requestParameters)
        {
            if (requestParameters.ContainsKey("user_id"))
            {
                return Convert.ToInt32(requestParameters["user_id"]);
            }
            else
                return 0;
        }
    }

    public class TraceExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Срабатывает при исключениях в коде.
        /// Заполняет логи информацией об исключении.
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            var requestId = (RequestId)context.Request.Properties[Constants.RequestIdKey];

            var auditInfo = context.Exception.ExceptionToXElement();

            if (LogStore.Logs.ContainsKey(requestId.GUID))
                LogStore.Logs[requestId.GUID].ExceptionResponse(
                    DateTime.Now.ToString(Constants.DateFormat),
                    auditInfo.ToString()
                    );
            else
            {
                requestId = new RequestId(Guid.NewGuid());
                var request = context.Request;
                var requestParameters = LogWriter.GetRequestParameters(request);
                int userId = LogWriter.GetUserIdFromReqParameters(requestParameters);
                XElement requestInfo = LogWriter.GetRequestInfo(request, requestParameters);
                LogStore.Logs.Add(requestId.GUID,
                new LogModel(
                    userId,
                    requestInfo.ToString(),
                    DateTime.Now.ToString(Constants.DateFormat))
                    );
                LogStore.Logs[requestId.GUID].ExceptionResponse(
                    DateTime.Now.ToString(Constants.DateFormat),
                    "Execute exception before adding request to logs." + auditInfo.ToString()
                    );
            }
        }
    }
}
