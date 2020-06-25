using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    /// <summary>
    /// Состояние аутентификации пользователя.
    /// </summary>
    public class AuthState
    {
        /// <summary>
        /// Идентификационное имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Флаг, показывающий аутентифицирован пользователь или нет.
        /// </summary>
        public bool IsAuthenticated { get; set; }
    }
}
