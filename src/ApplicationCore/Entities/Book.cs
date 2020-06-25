using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    /// <summary>
    /// Книга.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Год издания.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Жанр.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Автор.
        /// </summary>
        public string Author { get; set; }
    }
}
