using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OAuthHelper
{
    public delegate void AuthenticationChangedHandler(bool state);

    public static class GitHubOAuthHelpers
    {

        const string client_id = "da2a4dfdb573c4aa2ac6";
        const string secret = "10d299f4838eedc5695cd8e9d098c33ed17a5bb2";
        const string scope = "user,read:user,user:email,repo";
        const string redirect_uri = "bailey-github://code";

        public static OAuthResponse response;
        public static event AuthenticationChangedHandler OnAuthChanged;

        public static async Task<OAuthResponse> ExchangeCodeAsync(string code)
        {
            return await Task.Factory.StartNew(()=> 
            {
                return ExchangeCode(code);
            });
        }

        public static OAuthResponse ExchangeCode(string code)
        {
            if (!String.IsNullOrEmpty(code) && response == null)
            {
                try
                {
                    using (var web = new HttpClient())
                    {
                        var response = web.PostAsync($"https://github.com/login/oauth/access_token", new StringContent(Stringfy(new GitHubOAuthRequest() { client_id = client_id, client_secret = secret, redirect_uri = redirect_uri, code = GetCode(code) }), System.Text.Encoding.UTF8, "application/json")).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var content = response.Content.ReadAsStringAsync().Result;
                            GitHubOAuthHelpers.response = ParseCodeResponse(content);

                            OnAuthChanged?.Invoke(true);

                            return ParseCodeResponse(content);
                        }
                        throw new Exception("Bad Request");
                    }
                }
                catch (Exception ex)
                {
                    OnAuthChanged?.Invoke(false);
                    return null;
                }
            }
            else
            {
                OnAuthChanged?.Invoke(false);
                return null;
            }
        }

        private static OAuthResponse ParseCodeResponse(string s)
        {
            var obj = new OAuthResponse();
            var groups = s.Split('&');
            obj.AccessToken = groups[0].Split('=')[1];
            obj.TokenType = groups[2].Split('=')[1];
            
            return obj;
        }

        public static string GetOAuth()
        {
            return $"https://github.com/login/oauth/authorize?client_id={client_id}&redirect_uri={redirect_uri}&scope={scope}";
        }

        public static string Stringfy(object o)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(o);
        }

        public static T ToObject<T>(string s)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static string GetCode(string code)
        {
            if (!String.IsNullOrEmpty(code))
            {
                if (code.Contains("="))
                {
                    return code.Split('=')[1];
                }
                else
                {
                    return code;
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }

    class GitHubOAuthRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string redirect_uri { get; set; }
    }
}
