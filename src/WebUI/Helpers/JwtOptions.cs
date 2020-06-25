using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Helpers
{
    /// <summary>
    /// Объект параметров для класса <see cref="JwtHelper"/>.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Ключ для подписания JWT (Client Secret).
        /// </summary>
        public virtual string SigningKey { get; set; }

        /// <summary>
        /// Срок действия JWT.
        /// </summary>
        public virtual TimeSpan Lifetime { get; set; }

        /// <summary>
        /// Значения по умолчанию.
        /// </summary>
        public static JwtOptions Default { get; } = new JwtOptions
        {
            // Вообще, подобные вещи следует хранить в защищённом хранилище, но для простоты захардкодим секретный ключ
            // здесь.
            SigningKey = "mysupersecret_secretkey!123",

            // Установим срок действия JWT равным 10 дней.
            Lifetime = TimeSpan.FromDays(10)
        };

        /// <summary>
        /// Возвращает симметричный секретный ключ на основе Client Secret, используемый для подписания JWT.
        /// </summary>
        public virtual SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SigningKey));
        }
    }
}
