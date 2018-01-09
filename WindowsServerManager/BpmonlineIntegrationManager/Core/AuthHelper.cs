using System;
using System.IO;
using System.Net;
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

            // Вспомогательный объект, в который будут десериализованы данные HTTP-ответа.
            ResponseStatus status = null;
            // Получение ответа от сервера. Если аутентификация проходит успешно, в свойство AuthCookie будут
            // помещены cookie, которые могут быть использованы для последующих запросов.
            using (var response = (HttpWebResponse)authRequest.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    // Десериализация HTTP-ответа во вспомогательный объект.
                    string responseText = reader.ReadToEnd();
                    status = JsonConvert.DeserializeObject<ResponseStatus>(responseText);
                }

            }

            // Проверка статуса аутентификации.
            if (status != null)
            {
                // Успешная аутентификация.
                if (status.Code == 0)
                {
                    return true;
                }
                // Сообщение о неудачной аутентификации.
                Console.WriteLine(status.Message);
            }
            return false;
        }
    }
}