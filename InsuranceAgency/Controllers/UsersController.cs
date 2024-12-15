using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using InsuranceAgency.Models;
using InsuranceAgency.Data;

namespace InsuranceAgency.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly InsuranceAgencyDbContext _context;

        public UsersController(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        [HttpGet("AboutMe")]
        [Authorize]
        public IActionResult AboutMe()
        {
            // Получение текущего авторизованного пользователя
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(new { Message = "Пользователь не авторизован." });
            }

            User user = _context.Clients.FirstOrDefault(u => u.Login == userName);

            if (user == null)
            {
                user = _context.Accountants.FirstOrDefault(u => u.Login == userName);
                if (user == null)
                {
                    user = _context.InsuranceAgents.FirstOrDefault(u => u.Login == userName);
                    if (user == null)
                    {
                        user = _context.Administrators.FirstOrDefault(u => u.Login == userName);
                        if (user == null)
                        {
                            return NotFound(new { Message = "Пользователь не найден." });
                        }
                    }
                }
                
            }

            var response = new
            {
                user.Id,
                user.Login,
                user.Surname,
                user.Name,
                user.Patronymic,
                user.Email,
                user.PhoneNumber,
                user.Role
                
            };


            return View(response);
        }
    }
}
