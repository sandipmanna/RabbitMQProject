using SenderApplication.Interface;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;

namespace SenderApplication.BusinessLogic
{

    public class CSharpPythonRestfulAPI : ICSharpPythonRESTfulAPI
    {
        public readonly string uriWebAPI;
        public readonly string MethodSignature;
        public CSharpPythonRestfulAPI( string url, string methodSignature)
        {
            uriWebAPI = url;
            MethodSignature = methodSignature;
        }
        /// <summary>
        /// C# test to call Python HttpWeb RESTful API
        /// </summary>
        /// <param name="uirWebAPI">UIR web api link</param>
        /// <param name="exceptionMessage">Returned exception message</param>
        /// <returns>Web response string</returns>
        public bool CSharpPythonRestfulApiSimpleTest(object inputData, out dynamic _Result, out string exceptionMessage)
        {
            exceptionMessage = string.Empty;
            bool ReturnValue = false;
            _Result = null;
            string webResponse = string.Empty;
            try
            {
                /*String url = String.Concat
                        (
                            uriWebAPI,
                            MethodSignature,
                            "?",
                            "inputData=", inputData.ToString()
                        );
                Uri uri = new Uri(url);
                WebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    webResponse = streamReader.ReadToEnd();
                }*/

                using (HttpClient client = new HttpClient())
                {
                    {
                        String url = String.Concat
                        (
                            uriWebAPI,
                            MethodSignature,
                            "?",
                            "inputData=", inputData.ToString()
                        );
                        client.Timeout = TimeSpan.FromSeconds(300000);
                        client.BaseAddress = new Uri(url);
                        //HTTP GET
                        System.Threading.Tasks.Task<HttpResponseMessage> responseTask = client.GetAsync(MethodSignature, HttpCompletionOption.ResponseHeadersRead);
                        responseTask.Wait();

                        HttpResponseMessage result = responseTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            _Result = result.Content.ReadAsStringAsync();
                            _Result.Wait();
                            if (_Result != null)
                            {
                                ReturnValue = true;
                            }
                            else
                            {
                                ReturnValue = false;
                            }
                        }
                        else
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionMessage = $"An error occurred. {ex.Message}";
            }
            return ReturnValue;
        }

    }
}