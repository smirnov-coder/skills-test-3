using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    /// <summary>
    /// Входные данные книги.
    /// </summary>
    public class BookBindingModel
    {
        [Required, Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Year { get; set; }

        [Required, StringLength(100)]
        public string Genre { get; set; }

        [Required, StringLength(100)]
        public string Author { get; set; }
    }
}
