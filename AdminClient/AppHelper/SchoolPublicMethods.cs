using AdminClient.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdminClient.AppHelper
{
    public class SchoolPublicMethods
    {
        private readonly string _apiBaseUrl;
        private readonly IConfiguration _configuration;
        string tokenTxt = "TokenTxt";
        public SchoolPublicMethods(string apiBaseUrl, IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        public async Task GetAllClass(int? schoolId)
        {
            var class_url = _apiBaseUrl + $"/api/ClassMasters/GetClassMasters/{schoolId}";
            var request = new HttpRequestMessage(HttpMethod.Get, class_url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenTxt);
            // request.Content = contentData;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string returnString = await response.Content.ReadAsStringAsync();
                    var classDT = AllChildren(JObject.Parse(returnString.ToString().Replace("&#xD;&#xA;", Environment.NewLine)))
                                            .First(c => c.Type == JTokenType.Array && c.Path.Contains("data"))
                                            .Children<JObject>();
                }
            }

        }

        // recursively yield all children of json
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}
