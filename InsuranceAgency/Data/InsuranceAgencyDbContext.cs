using InsuranceAgency.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceAgency.Data
{
    public class InsuranceAgencyDbContext:DbContext
    {
        public InsuranceAgencyDbContext(DbContextOptions<InsuranceAgencyDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Accountant> Accountants { get; set; }
        public DbSet<InsuranceAgent> InsuranceAgents { get; set; }
        public DbSet<InsuranceObject> InsuranceObjects { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PolicyClaim> PolicyClaims { get; set; }
        public DbSet<PaymentClaim> PaymentClaims { get; set; }
        public DbSet<Policy> Policies { get; set; }
    }
}
