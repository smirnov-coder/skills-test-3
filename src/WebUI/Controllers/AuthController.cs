using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.Helpers;
using WebUI.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebUI.Controllers
{
    /// <summary>
    /// Контроллер аутентификации и авторизации (RESTful).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private SignInManager<IdentityUser> _signInManager;
        private IJwtHelper _jwtHelper;

        public AuthController(SignInManager<IdentityUser> signInManager, IJwtHelper jwtHelper)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(jwtHelper));
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        }

        /// <summary>
        /// Асинхронно выполняет вход на сайт.
        /// </summary>
        /// <param name="model">Учётные данные пользователя.</param>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserCredentials model)
        {
            var user = await _signInManager.UserManager.FindByNameAsync(model.UserName);
            if (user == null)
                return NotFound(new { error = "Пользователь не найден." });

            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (signInResult.Succeeded)
            {
                string token = _jwtHelper.GenerateJwtToken(user);
                return new JsonResult(new LoginResult { Token = token });
            }
            return BadRequest(new { error = "Неверный пароль." });
        }

        /// <summary>
        /// Проверяет и возвращает состояние аутентификации пользователя.
        /// </summary>
        [HttpGet("state")]
        public AuthState GetAuthState()
        {
            string userName = User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return new AuthState
                {
                    IsAuthenticated = false,
                };
            }
            else
            {
                return new AuthState
                {
                    IsAuthenticated = true,
                    UserName = userName
                };
            }
        }
    }
}
