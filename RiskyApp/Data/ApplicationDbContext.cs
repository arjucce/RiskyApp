using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiskyApp.Models;

namespace RiskyApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<DepartmentOperation> DepartmentOperations { get; set; }
        public DbSet<OpType> OpTypes { get; set; }
        public DbSet<RiskyApp.Models.Data> Data { get; set; }
        public DbSet<Category> Categories { get; set;}
        public DbSet<Months> Months { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Year> Years { get; set; }
    }
}