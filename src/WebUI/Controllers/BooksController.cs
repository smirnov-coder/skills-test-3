using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Data;
using ApplicationCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    /// <summary>
    /// Контроллер для работы с книга библиотеки (RESTful).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        /// <summary>
        /// Асинхронно возвращает книгу с заданным значением идентификатора.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetBook")]
        public Task<Book> GetBookAsync(int id)
        {
            return _bookRepository.GetBookByIdAsync(id);
        }

        /// <summary>
        /// Асинхронно возращает коллекцию всех книг.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public Task<IList<Book>> GetBooksAsync()
        {
            return _bookRepository.GetBooksAsync();
        }

        /// <summary>
        /// Асинхронно создаёт новую книгу и добавляет её в коллекцию книг библиотеки.
        /// </summary>
        /// <returns>Ответ HTTP 201 с заголовком Location, по которому доступен созданный ресурс.</returns>
        [HttpPost]
        public Task<IActionResult> CreateBookAsync(BookBindingModel model)
        {
            return PerformAddOrUpdateAsync(model, book => CreatedAtRoute("GetBook", new { id = book.Id }, book));
        }

        // Метод содержит разделяемую логику методов CreateBook() и UpdateBook() (принцип DRY).
        private async Task<IActionResult> PerformAddOrUpdateAsync(BookBindingModel model,
            Func<Book, IActionResult> returnAction)
        {
            var bookData = new Book
            {
                Id = model.Id,
                Title = model.Title,
                Author = model.Author,
                Genre = model.Genre,
                Year = model.Year
            };
            var bookEntity = _bookRepository.AddOrUpdateBook(bookData);
            await _bookRepository.SaveChangesAsync();
            return returnAction.Invoke(bookEntity);
        }

        /// <summary>
        /// Асинхронно обновляет данные существующей книги.
        /// </summary>
        [HttpPut]
        public Task<IActionResult> UpdateBookAsync(BookBindingModel model)
        {
            return PerformAddOrUpdateAsync(model, book => Ok(book));
        }

        /// <summary>
        /// Асинхронно удаляет книги с заданными значениями идентификаторов из коллекции книг.
        /// </summary>
        [HttpDelete]
        public async Task<OkResult> DeleteBooksAsync(int[] ids)
        {
            _bookRepository.RemoveBooks(ids);
            await _bookRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
