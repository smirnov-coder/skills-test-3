using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Data
{
    /// <summary>
    /// Контекст EntityFrameworkCore базы данных BookLibrary.
    /// </summary>
    public class BooksDbContext : IdentityDbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options): base(options)
        {
        }

        /// <summary>
        /// Коллекция книг.
        /// </summary>
        public virtual DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
