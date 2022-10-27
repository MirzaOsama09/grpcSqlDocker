using Microsoft.EntityFrameworkCore;

namespace GrpcGreeter.Models
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

    }
}

