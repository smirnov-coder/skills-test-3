using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с JSON Web Token (JWT).
    /// </summary>
    public interface IJwtHelper
    {
        /// <summary>
        /// Создаёт JWT на основе данных пользователя Identity.
        /// </summary>
        /// <param name="user">Данные пользователя Identity.</param>
        /// <returns>JWT в виде строки <see cref="string"/>.</returns>
        string GenerateJwtToken(IdentityUser user);

        /// <summary>
        /// Возвращает параметры, используемые при создании/валидации JWT.
        /// </summary>
        TokenValidationParameters GetValidationParameters();

        /// <summary>
        /// Проверяет валидность JWT.
        /// </summary>
        /// <param name="encodedJwt">JWT в виде строки <see cref="string"/>.</param>
        /// <returns>true, если JWT валиден; иначе false.</returns>
        bool ValidateToken(string encodedJwt);
    }
}
