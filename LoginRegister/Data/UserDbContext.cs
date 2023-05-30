

using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> LoginDetail { get; set; }
    }
}
