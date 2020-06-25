using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Data
{
    /// <summary>
    /// Репозиторий для хранения книг.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Асинхронно извлекает из репозиторий книгу с заданным значнеием идентификатора.
        /// </summary>
        /// <param name="bookId">Значение идентификатора книги.</param>
        /// <returns>Объект класса <see cref="Book"/> или null, если книга не найдена.</returns>
        Task<Book> GetBookByIdAsync(int bookId);

        /// <summary>
        /// Асинхронно извлекает из репозитория все книги.
        /// </summary>
        Task<IList<Book>> GetBooksAsync();

        /// <summary>
        /// Добавляет в репозиторий новую книгу.
        /// </summary>
        /// <param name="book">Новая книга.</param>
        /// <returns>Объект добавленной книги.</returns>
        Book AddOrUpdateBook(Book book);

        /// <summary>
        /// Удаляет из репозитория книги по заданным значениям идентификаторов.
        /// </summary>
        /// <param name="booksIds">Значения идентификаторов удаляемых книг.</param>
        void RemoveBooks(params int[] booksIds);

        /// <summary>
        /// Асинхронно фиксирует все изменения в репозитории. Метод должен вызываться явно после каждой операции
        /// модификации репозитория.
        /// </summary>
        Task SaveChangesAsync();
    }
}
