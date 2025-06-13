using EmailChatAppIdentity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmailChatAppIdentity.Context
{
    public class EmailContext: IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=GIZEM\\SQLEXPRESS;initial Catalog=EmailChatAppIdentityDb;integrated security=true;trust server certificate=true");
        }

        public DbSet<Message> Messages { get; set; }
    }
}
