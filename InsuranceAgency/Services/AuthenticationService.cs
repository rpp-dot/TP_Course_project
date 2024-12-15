using InsuranceAgency.Data;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System;

namespace InsuranceAgency.Services
{
    public class AuthenticationService
    {
        private readonly InsuranceAgencyDbContext _context;

        public AuthenticationService(InsuranceAgencyDbContext context)
        {
            _context = context;
        }

        public async Task<(string Role, bool IsAuthenticated)> AuthenticateAsync(string login, string password)
        {
            // Проверяем администратора
            var admin = await _context.Administrators
                .FirstOrDefaultAsync(a => a.Login == login);
            if (admin != null && VerifyPassword(password, admin.Password))
                return (admin.Role, true);

            // Проверяем бухгалтера
            var accountant = await _context.Accountants
                .FirstOrDefaultAsync(a => a.Login == login);
            if (accountant != null && VerifyPassword(password, accountant.Password))
                return (accountant.Role, true);

            // Проверяем страхового агента
            var agent = await _context.InsuranceAgents
                .FirstOrDefaultAsync(a => a.Login == login);
            if (agent != null && VerifyPassword(password, agent.Password))
                return (agent.Role, true);

            // Проверяем клиента
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Login == login);
            if (client != null && VerifyPassword(password, client.Password))
                return (client.Role, true);

            return (null, false);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

}
