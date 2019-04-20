using Cardin.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cardin.Service
{
    public class HttpClientServices
    {
        static HttpClient client = new HttpClient();

        public HttpClientServices()
        {

        }

        public async Task<string> CreateAsync(string url,Object body,string auth)
        {
            try
            {
                string jsonObject = JsonConvert.SerializeObject(body, Formatting.Indented);
                var httpContentObject = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                if (!string.IsNullOrEmpty(auth))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                }
                var response = client.PostAsync(url, httpContentObject);
                string serializeObject = await response.Result.Content.ReadAsStringAsync();
                return serializeObject;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<string> UpdateAsync(string url,string id, Object body,string auth)
        {
            try
            {
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                var response = await client.PutAsJsonAsync(url+"/"+id, body);
                response.EnsureSuccessStatusCode();
                var serializeResponseObject = await response.Content.ReadAsStringAsync();
                return serializeResponseObject;
            }
            catch (Exception ex)
            {
                string u = ex.Message;
                return "";
            }
        }

        public async Task<string> GetAsync(string url, string auth)
        {
            try
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string serializeResponseObject = await response.Content.ReadAsStringAsync();
                    return serializeResponseObject;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PatchAsync(string url, string auth,dynamic body,bool contentWithImg)
        {
            try
            {
                if (contentWithImg)
                {
                    string jsonObject = JsonConvert.SerializeObject(body, Formatting.Indented);
                    var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                    var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                    httpRequestMessage.Content = content;
                    var response = client.SendAsync(httpRequestMessage);
                    string serializeObject = await response.Result.Content.ReadAsStringAsync();
                    return serializeObject;
                }
                else
                {
                    var parameters = new List<KeyValuePair<string, string>>
                        {

                            new KeyValuePair<string,string>("email", body.email),
                            new KeyValuePair<string,string>("firstName", body.firstName),
                            new KeyValuePair<string,string>("lastName",body.lastName),
                            new KeyValuePair<string ,string>("phone",body.phone),
                            new KeyValuePair<string, string>("post", body.post),
                            new KeyValuePair<string,string>("profileImage", body.profileImage),
                            //new KeyValuePair<string,string>("signature", body.signature),
                            new KeyValuePair<string,string>("facilityId", body.facilityId),
                            new KeyValuePair<string ,string>("engagementType",body.engagementType),
                            new KeyValuePair<string, string>("extentionNo", body.extentionNo)
                        };
                    HttpContent content = new FormUrlEncodedContent(parameters);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                    var httpRequestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                    httpRequestMessage.Content = content;
                    var response = client.SendAsync(httpRequestMessage);
                    string serializeObject = await response.Result.Content.ReadAsStringAsync();
                    return serializeObject;
                }
            }
            catch (Exception ex)
            {
                string g = ex.Message;
                return "";
            }
        }

        public async Task<string> DeleteAsync(string url, string id, string auth)
        {
            try
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth);
                var response = await client.DeleteAsync(url + "/" + id);
                response.EnsureSuccessStatusCode();
                var serializeResponseObject = await response.Content.ReadAsStringAsync();
                return serializeResponseObject;
            }
            catch (Exception ex)
            {
                string u = ex.Message;
                return "";
            }
        }
    }    
}
