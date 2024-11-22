namespace InsuranceAgency.Controllers
{
    using InsuranceAgency.Data;
    using InsuranceAgency.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;

    public class AccountController : Controller
    {
        private readonly Services.AuthenticationService _authService;
        private readonly InsuranceAgencyDbContext _context;

        public AccountController(Services.AuthenticationService authService, InsuranceAgencyDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            var (role, isAuthenticated) = await _authService.AuthenticateAsync(login, password);

            if (!isAuthenticated)
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                return View();
            }

            // Создаем куки
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, login),
                new System.Security.Claims.Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        

        

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Client client)
        {
            if (ModelState.IsValid)
            {
                // Хэшируем пароль перед сохранением (например, с использованием BCrypt)
                //client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);

                // Устанавливаем роль "Client"
                client.Role = "Client";

                // Сохраняем клиента в базу данных
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                // Перенаправляем на страницу входа
                return RedirectToAction("Login", "Account");
            }
            return View(client);
        }
    }

}
