using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TechnologicalRunPG
{
    public class WsdlAuthorizationService
    {
        private ChannelFactory<EntityService.IEntityServiceChannel> _entityServiceFactory;

        private ChannelFactory<AuthorizationService.IAuthorizationServiceChannel> _authServiceFactory;

        private ChannelFactory<FilesService.IFilesServiceChannel> _filesServiceFactory;

        private ChannelFactory<WorkflowService.WorkflowChannel> _workflowServiceFactory;

        public string userName;

        public string userPwd;

        public string userId;

        public bool connected = false;

        private string sessionToken;

        private Guid authToken;

        public Bitmap TestConnection()
        {
            _entityServiceFactory =

                new ChannelFactory<EntityService.IEntityServiceChannel>(

                    new BasicHttpsBinding() { MaxReceivedMessageSize = Int32.MaxValue }, "https://elma.eriskip.com/API/Entity");

            _authServiceFactory =

                new ChannelFactory<AuthorizationService.IAuthorizationServiceChannel>(new BasicHttpsBinding(),

                    "https://elma.eriskip.com/API/Authorization");

            _filesServiceFactory =

                new ChannelFactory<FilesService.IFilesServiceChannel>(new BasicHttpsBinding(),

                    "https://elma.eriskip.com/API/Files");

            _workflowServiceFactory =

               new ChannelFactory<WorkflowService.WorkflowChannel>(new BasicHttpsBinding(),

                   "https://elma.eriskip.com/API/Workflow");

            return GetAuthToken();
        }

        public Bitmap GetAuthToken()
        {
            var authorizationService = _authServiceFactory.CreateChannel();
            using (new OperationContextScope((IContextChannel)authorizationService))
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("ApplicationToken",

                "null");
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                    var token = authorizationService.LoginWithUserName(userName, userPwd);
                    authToken = token.AuthToken;
                    var auth = authorizationService.CheckToken(authToken);
                    sessionToken = auth.SessionToken;

                    userId = auth.CurrentUserId;

                    string SQL = "select Photo from [user]" +
                                 " where id like '" + userId + "'";
                    List<object> result = ElmaConnect.SqlQuery(SQL);
                    if (result[0].ToString() != "")
                    {
                        string url1 = "https://elma.eriskip.com/API/REST/Files/Download?uid=" + result[0];
                        //параметры запроса передаём в самом url, к которому будем обращаться
                        var url = string.Format(url1, "null");

                        //генерация запроса
                        HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                        req.Method = "GET";
                        req.Headers.Add("AuthToken", authToken.ToString());
                        req.Timeout = 10000;

                        //получение ответа
                        var res = req.GetResponse() as HttpWebResponse;
                        var resStream = res.GetResponseStream();
                        var sr = new StreamReader(resStream, Encoding.UTF8);

                        Bitmap loadedBitmap = null;
                        using (var responseStream = res.GetResponseStream())
                        {
                            loadedBitmap = new Bitmap(responseStream);
                        }
                        return loadedBitmap;
                    }
                    else
                    {
                        return Properties.Resources.defaultImg;
                    }
                }
                catch
                {
                    //иначе выбрасываем ошибку или дальше её обрабатываем                           
                    connected = false;
                    userId = "";
                }
            }
            return Properties.Resources.defaultImg;
        }
    }
}


