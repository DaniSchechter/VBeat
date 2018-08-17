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
        private static string PAGE_ACCESS_TOKEN = "EAAECTyQpBgQBABTcBleKZBEsz7FVRrLOUCR7kFQPwBVKEVi7VRJ4ZAkZB4QFemwwCEA9EIZBU6Ll3I3MN1BhRQDBdSXMFxDpjx0LCc2ZBT4thamN9JEt61aVG3xPt2M95ADK5ZCJR2GWkdRuC1pXR4GnpysmQlsTtMYqQhFT3DyG1X6wZA3EOoCf482cUgGyYZBFDTHVDkYElWQbf8WJ6q9KnJZAkhkwbeUynrFCW8osrbQZDZD";
        HttpClient httpClient;
        public FacebookModel()
        {
            httpClient = new HttpClient();
        }

        public async Task<bool> Post(string message)
        {
            try
            {
                var response = await httpClient.PostAsync("https://graph.facebook.com/3.1/vbeatcollegeproject/feed?access_token=" + WebUtility.UrlEncode(PAGE_ACCESS_TOKEN)
                    + "&message=" + WebUtility.UrlEncode(message), new StringContent(null));
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
