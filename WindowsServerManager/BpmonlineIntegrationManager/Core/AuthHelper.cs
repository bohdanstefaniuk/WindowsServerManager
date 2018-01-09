using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using Newtonsoft.Json;

namespace BpmonlineIntegrationManager.Core
{
    public class AuthHelper
    {
        private readonly string _url;
        public static CookieContainer AuthCookie = new CookieContainer();

        public AuthHelper(string baseUrl)
        {
            _url = $"{baseUrl}/0/ServiceModel/AuthService.svc/Login";
        }

        public bool TryLogin(string userName, string userPassword)
        {
            var authRequest =
                WebRequest.Create(
                    @"http://localhost/WORK_D/0/ServiceModel/AuthService.svc/Login") as HttpWebRequest;

            authRequest.Method = "POST";
            authRequest.ContentType = "application/json";
            authRequest.CookieContainer = AuthCookie;

            using (var requestStream = authRequest.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(@"{
                    ""UserName"":""" + userName + @""",
                    ""UserPassword"":""" + userPassword + @"""
                    }");
                }
            }

            ResponseStatus status;
            using (var response = (HttpWebResponse)authRequest.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    status = JsonConvert.DeserializeObject<ResponseStatus>(responseText);
                }

            }
            
            if (status != null)
            {
                if (status.Code == 0)
                {
                    return true;
                }
                
                throw new AuthenticationException(status.Message);
            }
            return false;
        }
    }
}