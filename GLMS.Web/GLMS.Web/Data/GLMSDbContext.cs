using Microsoft.EntityFrameworkCore;
using GLMS.Web.Models;

namespace GLMS.Web.Data
{
    public class GLMSDbContext : DbContext
    {
        public GLMSDbContext(DbContextOptions<GLMSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}