using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    /// <summary>
    /// Учётные данные пользователя
    /// </summary>
    public class UserCredentials
    {
        /// <summary>
        /// Идентификационное имя пользователя.
        /// </summary>
        [Required, StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Required, StringLength(20)]
        public string Password { get; set; }
    }
}
