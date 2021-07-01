using GherkinManager.Model;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GherkinManager
{
    public class GherkinClient
    {
        private readonly HttpClient _httpClient;

        public GherkinClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<string> GetSampleFeature()
        {
            string content;

            using (var response = await (_httpClient.GetAsync("samplefeature")))
            {
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FeatureContent>(content);

                return result.Content;
            }
        }

        public async Task<bool> ValidateFeature(string content)
        {
            var stringContent = new StringContent(content, Encoding.UTF8, "text/plain");
            stringContent.Headers.ContentType.CharSet = null;
            using (var response = await _httpClient.PostAsync("validate", stringContent))
            {
                if (!(response.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}