using Microsoft.AspNetCore.Mvc.Testing;

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
        public async void CreateKweetIntegration()
        {
            // Act
            var response = await _client.GetAsync("/api/kweet/testfeed");

            var result = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(!string.IsNullOrEmpty(result));

        }
    }
}
