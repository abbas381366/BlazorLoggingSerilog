using Microsoft.EntityFrameworkCore;

namespace BlazorLoggingSeriLog.DbModels
{
    public class MyDbcontext:DbContext
    {
        public MyDbcontext(DbContextOptions<MyDbcontext> options):base(options) { }
        public DbSet<DIMPersonelType> DIMPersonelTypes { get; set; }
        public DbSet<Personel> Personels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DIMPersonelType>()
                .HasData(new DIMPersonelType { ID = 1, Title = "حقیقی" },
                new DIMPersonelType { ID = 2, Title = "حقوقی" });
        }

    }
}
