using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace HSDraft_v2.Models
{
    public class CardAppContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=DraftDatabase;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Server=tcp:vu4hlc0ywi.database.windows.net,1433;Database=DraftDatabase;User ID=neimless@vu4hlc0ywi;Password=P6tGH7611412;Trusted_Connection=False;Encrypt=True;Connection Timeout=120;");
        }
    }
}
