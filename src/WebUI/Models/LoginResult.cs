using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    /// <summary>
    /// Результат авторизации пользователя.
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// Маркер доступа (он же access token) в виде JWT.
        /// </summary>
        public string Token { get; set; }
    }
}
