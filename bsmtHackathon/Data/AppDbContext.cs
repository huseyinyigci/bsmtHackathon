using Microsoft.EntityFrameworkCore;
using bsmtHackathon.Models;

namespace bsmtHackathon.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanındaki tablolarımız bunlar olacak
        public DbSet<User> Users { get; set; }
        public DbSet<OgrenciProfili> OgrenciProfilleri { get; set; }
        public DbSet<YemekPlaniDb> YemekPlanlari { get; set; }
    }
}