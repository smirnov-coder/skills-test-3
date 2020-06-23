using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string Genre { get; set; }

        public string Author { get; set; }
    }
}
