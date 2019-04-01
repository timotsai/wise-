using System;
using System.IO;
using System.Net.NetworkInformation;

namespace wise4051irda
{
    class clsNetwork
    {
        //2019/02/19
        public static string GeadWiseIp(ref string[] wise_id,ref string[] wise_ip,ref string[] wise_api)
        {
            StreamReader StreamReader;
            string linedata;
            try
            {
                StreamReader = new StreamReader("Setting.ini");
                do
                {
                    linedata = StreamReader.ReadLine();
                    if (linedata != null)
                    {
                        if (linedata.IndexOf("DeviceName") >= 0)
                        {
                            Array.Resize(ref wise_id, wise_id.Length + 1);
                            wise_id[wise_id.Length-1] = linedata.Split(Convert.ToChar(":"))[1].Trim();
                        }
                        if (linedata.IndexOf("IP") >= 0)
                        {
                            Array.Resize(ref wise_ip, wise_ip.Length + 1);
                            wise_ip[wise_ip.Length-1] = linedata.Split(Convert.ToChar(":"))[1].Trim();
                        }
                        if (linedata.IndexOf("API") >= 0)
                        {
                            Array.Resize(ref wise_api, wise_api.Length + 1);
                            wise_api[wise_api.Length - 1] = linedata.Split(Convert.ToChar(":"))[1].Trim();
                        }
                    }
                }
                while (linedata != null);
                StreamReader.Close();
                StreamReader.Dispose();
                return linedata;
            }
            catch(Exception ex)
            { return ""; }
            finally { }
        }
        public static bool isExitst(string ip)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            PingReply reply = pingSender.Send(ip);
            if (reply.Status == IPStatus.Success)
            { return true; }
            else
            { return false; }
        }
    }
}
