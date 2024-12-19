using InsuranceAgency.Data;
using InsuranceAgency.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace InsuranceAgency.Controllers
{
    
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

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
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
            // Получаем пользователя по роли
            int userId = role switch
            {
                "Admin" => (await _context.Administrators.FirstOrDefaultAsync(a => a.Login == login))?.Id ?? 0,
                "Accountant" => (await _context.Accountants.FirstOrDefaultAsync(a => a.Login == login))?.Id ?? 0,
                "InsuranceAgent" => (await _context.InsuranceAgents.FirstOrDefaultAsync(a => a.Login == login))?.Id ?? 0,
                "Client" => (await _context.Clients.FirstOrDefaultAsync(c => c.Login == login))?.Id ?? 0,
                _ => 0
            };
            // Создаем куки
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(ClaimTypes.Name, login),
                new System.Security.Claims.Claim(ClaimTypes.Role, role),
                new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, userId.ToString()) // Добавляем идентификатор
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
            // Хэшируем пароль перед сохранением (например, с использованием BCrypt)
            client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);

            // Устанавливаем роль "Client"
            client.Role = "Client";
            if (ModelState.IsValid)
            {
               
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
