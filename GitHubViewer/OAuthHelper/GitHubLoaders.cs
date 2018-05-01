using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHelper
{
    public static class GitHubLoaders
    {
        const string BaseEndPoint = "https://api.github.com";
        private static OAuthResponse GitHubAccessData
        {
            get
            {
                return GitHubOAuthHelpers.response;
            }
        }

        public static async Task<GitHubUser> GetUser()
        {
            using (var web = new HttpClient())
            {
                web.AddAuthorization();

                web.BaseAddress = new Uri(BaseEndPoint);

                var result = await web.GetAsync("/user");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<GitHubUser>(content);
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }


        public static async Task<List<RepoResponse>> GetPublicRepos()
        {
            using (var web = new HttpClient())
            {
                web.AddAuthorization();
                web.BaseAddress = new Uri(BaseEndPoint);
                var result = await web.GetAsync("/user/repos?visibility=public");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();

                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RepoResponse>>(content);
                    return data;
                }else
                {
                    return null;
                }

            }
        }

        public static async Task<List<RepoResponse>> GetPrivateRepos()
        {
            using (var web = new HttpClient())
            {
                web.AddAuthorization();
                web.BaseAddress = new Uri(BaseEndPoint);
                var result = await web.GetAsync("/user/repos?visibility=private");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();

                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RepoResponse>>(content);
                    return data;
                }
                else
                {
                    return null;
                }

            }
        }


        private static HttpClient AddAuthorization(this HttpClient me)
        {

            me.DefaultRequestHeaders.Add("Authorization", $"token {GitHubAccessData.AccessToken}");
            return me;
        }
    }
}
