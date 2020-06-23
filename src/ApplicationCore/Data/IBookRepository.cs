using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Data
{
    public interface IBookRepository
    {
        Task SaveBookAsync(Book book);

        Task RemoveBookAsync(int bookId);
    }
}
