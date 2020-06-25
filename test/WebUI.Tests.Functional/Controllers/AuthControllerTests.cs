using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebUI.Helpers;
using WebUI.Models;
using Xunit;

namespace WebUI.Tests.Functional.Controllers
{
    //
    // Протестируем основные положительные сценарии использования.
    //

    [Collection(nameof(FactoryCollection))]
    public class AuthControllerTests
    {
        private const string BASE_URL = "/api/auth";
        private const string USER_NAME = "admin";
        private readonly FactoryFixture _factory;

        public AuthControllerTests(FactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanLoginWithValidCredentials()
        {
            using (var httpClient = _factory.ForRead.CreateClient())
            {
                var testCredentials = new
                {
                    UserName = USER_NAME,
                    Password = "Admin-123"
                };
                string json = JsonConvert.SerializeObject(testCredentials);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{BASE_URL}/login", requestContent))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResult>(responseJson);
                    Assert.False(string.IsNullOrWhiteSpace(result.Token));
                    Assert.True(new JwtHelper().ValidateToken(result.Token));
                }
            }
        }

        [Fact]
        public async Task CanGetAuthStateWhenLoggedIn()
        {
            using (var httpClient = await _factory.ForRead.CreateClientWithAccessTokenAsync(USER_NAME))
            {
                using (var response = await httpClient.GetAsync($"{BASE_URL}/state"))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthState>(responseJson);
                    Assert.True(result.IsAuthenticated);
                    Assert.Equal(USER_NAME, result.UserName);
                }
            }
        }
    }
}
