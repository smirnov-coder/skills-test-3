using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Data
{
    /// <inheritdoc cref="IBookRepository"/>
    public class BookRepository : IBookRepository
    {
        private BooksDbContext _context;

        /// <summary>
        /// Создаёт новый экземпляр класса <see cref="BookRepository"/>.
        /// </summary>
        /// <param name="context">Объект контекста EntityFrameworkCore.</param>
        public BookRepository(BooksDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task<IList<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public void RemoveBooks(params int[] bookIds)
        {
            var deletedBooks = _context.Books.Where(book => bookIds.Contains(book.Id));
            _context.Books.RemoveRange(deletedBooks);
        }

        public Book AddOrUpdateBook(Book book)
        {
            return _context.Books.Update(book).Entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
