using KweetService.API.Models.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Test
{
    public class KweetIntegrationTest
    {
        HttpClient? _client;

        public KweetIntegrationTest()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _client = webApplicationFactory.CreateDefaultClient();
        }

        [Fact]
        public async void GetFeedIntegration()
        {
            // Act
            var response = await _client.GetAsync("/api/kweet/testfeed");

            // Assert
            Assert.True(response.IsSuccessStatusCode);

        }

        [Fact]
        public async void CreateKweetIntegration()
        {
            KweetCreatedDTO dto = new()
            {
                Text = "test kweet integration",
                CustomerId = 1
            };

            var jSonData = JsonConvert.SerializeObject(dto);

            // Act
            var response = await _client.PostAsync("/api/kweet/create", new StringContent(jSonData, Encoding.UTF8, "application/json"));

            // Assert
            Assert.True(response.IsSuccessStatusCode);

        }
    }
}
