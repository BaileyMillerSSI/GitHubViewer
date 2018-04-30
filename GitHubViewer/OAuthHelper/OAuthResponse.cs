using System;
using System.Collections.Generic;
using System.Text;

namespace OAuthHelper
{
    public class OAuthResponse
    {
        public String AccessToken { get; set; }
        public List<String> Scopes { get; set; } = new List<string>();
        public String TokenType { get; set; }
    }
}
