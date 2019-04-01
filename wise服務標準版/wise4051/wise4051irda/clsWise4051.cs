using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using static wise4051irda.clswise4051device;


namespace wise4051irda
{
    class clsWise4051
    {
        WISE4051Device output = new WISE4051Device();

        public WISE4051Device wise4051Get(String ip,String api)
        {
            var result = string.Empty;
            var url = "http://" + ip.Trim() + "/di_value/" + api.Trim();
            var head = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("root:00000000"));

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "Get";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8;";
            request.Headers.Add("Authorization", "Basic " + head);
            request.Timeout = 10000;

            try
            {
                using (HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
                output = JsonConvert.DeserializeObject<WISE4051Device>(result);
                return output;
            }
            catch (WebException e)
            {
                return output;
            }
        }
    }
}
