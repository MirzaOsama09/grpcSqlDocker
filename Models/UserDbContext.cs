using Microsoft.EntityFrameworkCore;

namespace GrpcGreeter.Models
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(GetUsers());
        }

        private List<User> GetUsers()
        {
            List<User> users = new List<User>() { 
                new User()
                {
                    id= 1, username="mirza", password="abc", role="admin",
                },
                new User()
                {
                    id= 2, username="cheeti", password="123", role="serverAdmin",
                }
            };

            return users;
        }

        public DbSet<User> users { get; set; }



        
    }
}

