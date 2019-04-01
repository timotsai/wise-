using System;
using System.Collections.Generic;
using System.Text;

namespace wise4051irda
{
    class clswise4051device
    {
        public class WISE4051Device
        {
            public List<WISE4051DeviceValue> DIVal { get; set; }
            public string Err { get; set; }
            public string Msg { get; set; }
        }

        public class WISE4051DeviceValue
        {
            public string Ch { get; set; }
            public string Md { get; set; }
            public string Val { get; set; }
            public string Stat { get; set; }
            public string Cnting { get; set; }
            public string OvLch { get; set; }
        }
    }
}
