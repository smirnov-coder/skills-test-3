using ApplicationCore.Data;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebUI.Tests.Functional.Controllers
{
    //
    // Протестируем основные положительные сценарии использования.
    //

    [Collection(nameof(FactoryCollection))]
    public class BlogControllerTests
    {
        private const string BASE_URL = "/api/books";
        private const string USER_NAME = "admin";
        private readonly FactoryFixture _factory;

        public BlogControllerTests(FactoryFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CanGetExistingBook()
        {
            using (var httpClient = _factory.ForRead.CreateClient())
            {
                using (var response = await httpClient.GetAsync($"{BASE_URL}/1"))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                    var resultJson = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Book>(resultJson);
                    Assert.NotNull(book);
                    Assert.Equal(1, book.Id);
                    Assert.Equal("Title #1", book.Title);
                    Assert.Equal("Author #1", book.Author);
                    Assert.Equal("Genre #1", book.Genre);
                    Assert.Equal(2001, book.Year);
                }
            }
        }

        [Fact]
        public async Task CanGetAllBooks()
        {
            using (var httpClient = _factory.ForRead.CreateClient())
            {
                using (var response = await httpClient.GetAsync(BASE_URL))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
                    var resultJson = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<List<Book>>(resultJson);
                    Assert.NotNull(books);
                    Assert.Equal(10, books.Count);
                    Assert.Equal(1, books.First().Id);
                    Assert.Equal(10, books.Last().Id);
                }
            }
        }

        [Fact]
        public async Task CanCreateNewBook()
        {
            var factory = _factory.ForAdd;
            using (var httpClient = await factory.CreateClientWithAccessTokenAsync(USER_NAME))
            {
                int expectedCount = 11;
                var model = new 
                {
                    Id = 0,
                    Title = "Test Title",
                    Author = "Test Author",
                    Genre = "Test Genre",
                    Year = 2020
                };
                string json = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(BASE_URL, requestContent))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    using (var scope = factory.Server.Services.CreateScope())
                    {
                        using (var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>())
                        {
                            Assert.Equal(expectedCount, db.Books.Count());
                            var book = db.Books.ToList().LastOrDefault();
                            Assert.NotNull(book);
                            Assert.NotEqual(0, book.Id);
                            Assert.Equal("Test Title", book.Title);
                            Assert.Equal("Test Author", book.Author);
                            Assert.Equal("Test Genre", book.Genre);
                            Assert.Equal(2020, book.Year);

                        }
                    }
                }
            }
        }

        [Fact]
        public async Task CanUpdateExistingBook()
        {
            var factory = _factory.ForUpdate;
            using (var httpClient = await factory.CreateClientWithAccessTokenAsync(USER_NAME))
            {
                int expectedCount = 10,
                    id = 1;
                var model = new
                {
                    Id = id,
                    Title = "New Title",
                    Author = "New Author",
                    Genre = "New Genre",
                    Year = 2020
                };
                string json = JsonConvert.SerializeObject(model);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync(BASE_URL, requestContent))
                {
                    Assert.True(response.IsSuccessStatusCode);
                    using (var scope = factory.Server.Services.CreateScope())
                    {
                        using (var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>())
                        {
                            Assert.Equal(expectedCount, db.Books.Count());
                            var book = await db.Books.FirstOrDefaultAsync(x => x.Id == id);
                            Assert.NotNull(book);
                            Assert.Equal(id, book.Id);
                            Assert.Equal("New Title", book.Title);
                            Assert.Equal("New Author", book.Author);
                            Assert.Equal("New Genre", book.Genre);
                            Assert.Equal(2020, book.Year);
                        }
                    }
                }
            }
        }

        [Fact]
        public async Task CanDeleteBooks()
        {
            var factory = _factory.ForDelete;
            using (var httpClient = await factory.CreateClientWithAccessTokenAsync(USER_NAME))
            {
                int expectedCount = 7;
                var deletedBooksIds = new[] { 1, 3, 5 };
                using (var request = new HttpRequestMessage(HttpMethod.Delete, BASE_URL))
                {
                    string json = JsonConvert.SerializeObject(deletedBooksIds);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = await httpClient.SendAsync(request))
                    {
                        Assert.True(response.IsSuccessStatusCode);
                        using (var scope = factory.Server.Services.CreateScope())
                        {
                            using (var db = scope.ServiceProvider.GetRequiredService<BooksDbContext>())
                            {
                                Assert.Equal(expectedCount, db.Books.Count());
                                Assert.True(await db.Books.AnyAsync(book => !deletedBooksIds.Contains(book.Id)));
                            }
                        }
                    }
                }
            }
        }
    }
}
