using ApplicationCore.Data;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Development
{
    /// <summary>
    /// Вспомогательный класс для разработки и тестирования.
    /// </summary>
    public class FakeDataInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private BooksDbContext _dbContext;

        public FakeDataInitializer(UserManager<IdentityUser> userManager, BooksDbContext dbContext)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Генерирует фейовые данные.
        /// </summary>
        public void Initialize()
        {
            CreateUsers();
            CreateBooks();
        }

        private void CreateUsers()
        {
            if (_userManager.Users.Any())
                return;

            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                EmailConfirmed = true
            };
            _userManager.CreateAsync(adminUser, "Admin-123").Wait();
        }

        private void CreateBooks()
        {
            if (_dbContext.Books.Any())
                return;

            for (int i = 1; i <= 10; i++)
            {
                _dbContext.Books.Add(new Book
                {
                    Title = $"Title #{i}",
                    Author = $"Author #{i}",
                    Genre = $"Genre #{i}",
                    Year = 2000 + i
                });
            }
            _dbContext.SaveChanges();
        }
    }
}
