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
        private static string PAGE_ACCESS_TOKEN = "EAAECTyQpBgQBAGdJOxd0AYkxeBtCPHcgelWM61sVH6xktsBOUD8DCNKGcfsS1AfFhtzZAPcJ7ZC4qKmmDIgf9IjbtFJWao4z7pgg7BKhcgSFmAEDGWZA8eTNjw8FCF6LyVOroXst23dwZARooLrpoR8RmW0IGxTPesIwJoOJGp2fqGBnYmBkoTvBvKEmg6kZD";
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
