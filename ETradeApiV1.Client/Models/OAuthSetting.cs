using System;
using System.Collections.Generic;
using System.Text;

namespace ETradeApiV1.Client.Models
{
   public class OAuthSetting
    {
        public string BaseUrl { get; set; }
        public string AuthorizeUrl { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }
}
