using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleModernVideoPlayer.Dao
{
    class Request
    {  /// <summary>
       /// http请求操作
       /// </summary>
       /// <param name="o">要序列化的object</param>
       /// <param name="method">方法 POST PUT PATCH DELETE等等</param>
       /// <param name="param">URI地址</param>
       /// <returns>一个object</returns>
        public static object HttpRequest(object o, string method, string param)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://39.99.164.175/RestService/" + param);
            string responsevalue = string.Empty;
            request.Method = method;
            request.ContentLength = 0;
            request.ContentType = "application/json";
            string usr2json = JsonConvert.SerializeObject(o);
            Console.WriteLine(usr2json);
            var bytes = Encoding.UTF8.GetBytes(usr2json);
            request.ContentLength = bytes.Length;

            using (var writeStream = request.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
            }
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("请求失败，原因" + response.StatusCode.ToString());
                }
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responsevalue = reader.ReadToEnd();
                        }
                    }
                }
                Console.WriteLine("response:" + responsevalue);
                return responsevalue;
            }
        }
    }
}
