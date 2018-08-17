using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace VBeat.Models.Facebook
{
    public class FacebookModel
    {
        private static string PAGE_ACCESS_TOKEN = "EAAECTyQpBgQBABAxEkzVbqO3DzwIZAaCZB4qx6WQMe4sRL47eEZAtJVkAwyqp8HTluqfuwLevvzU2kSHZBTXf1VOuqy7btqo6TzxYykb9zHKPuHIGYUAWUCMDmkds30L6aYTsVE5ZBamBzVwvQlNxid7RDoOWpMbZCGi1DZAuGvxAfk1mZAxpC4k11Ey7pZB1CsQZD";
        HttpClient httpClient;
        public FacebookModel()
        {
            httpClient = new HttpClient();
        }

        public async Task<bool> Post(string message)
        {
            try
            {
                var response = await httpClient.PostAsync("https://graph.facebook.com/274273156721225/feed" +
                   "?message=" + WebUtility.UrlEncode(message), new FormUrlEncodedContent(new Dictionary<string, string>()
                   {
                       {"access_token", PAGE_ACCESS_TOKEN }
                   }));
                string contents = await response.Content.ReadAsStringAsync();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
