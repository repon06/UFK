using System;
using RestSharp;

namespace ufk.Helper
{
    class RestHelper
    {
        public static string SendRequest(/*string url*/)
        {
            //внести изменения: с сайта возвращать закодир текстовый шаблон 
            //"http://www.uniplast-kbe.ru/test/ufk.php?program=ufk&id=644521"
            try
            {
                var httpClient = new RestClient("http://www.uniplast-kbe.ru") { Timeout = 30000 };
                var request = new RestRequest("/test/ufk.php?program=ufk&id=644521", Method.POST);
                request.Timeout = 30000;
                var response = httpClient.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content.Trim();
                    return content;
                }
                else
                    return "Error: ошибка при проверке активации!";

            }
            catch (Exception e)
            {
                return "Error: ошибка при проверке активации!";
            }

        }
    }
}
