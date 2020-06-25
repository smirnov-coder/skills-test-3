using ApplicationCore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebUI.Development;
using WebUI.Helpers;

namespace WebUI.Tests.Functional
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>, IDisposable
    {
        private SqliteInMemoryDatabase _sqliteDatabase;
        
        public CustomWebApplicationFactory()
        {
            _sqliteDatabase = new SqliteInMemoryDatabase();
        }

        protected override void Dispose(bool disposing)
        {
            _sqliteDatabase?.Dispose();
            base.Dispose(disposing);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<BooksDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<BooksDbContext>(options => options.UseSqlite(_sqliteDatabase.Connection));
                CreateTestData(services.BuildServiceProvider());
            });
        }

        private void CreateTestData(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BooksDbContext>();
                db.Database.EnsureCreated();
                var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
                new FakeDataInitializer(userManager, db).Initialize();
            }
        }

        /// <summary>
        /// Создаёт HttpClient для TestHost'а с заголовком 'Authorization: Bearer <valid_access_token>' для каждого
        /// запроса.
        /// </summary>
        public async Task<HttpClient> CreateClientWithAccessTokenAsync(string userName, 
            WebApplicationFactoryClientOptions options = null)
        {
            var httpClient = options == null ? CreateClient() : CreateClient(options);
            using (var scope = Server.Services.CreateScope())
            {
                var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<IdentityUser>>();
                var user = await signInManager.UserManager.FindByNameAsync(userName);
                signInManager.Context = new DefaultHttpContext
                {
                    RequestServices = scope.ServiceProvider
                };
                if (await signInManager.CanSignInAsync(user))
                {
                    string token = new JwtHelper().GenerateJwtToken(user);
                    httpClient.DefaultRequestHeaders.Authorization =
                        AuthenticationHeaderValue.Parse($"{JwtBearerDefaults.AuthenticationScheme} {token}");
                }
            }
            return httpClient;
        }
    }
}
