using YarimKalanlar.Models;
using Microsoft.EntityFrameworkCore;

namespace YarimKalanlar.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Haber> Haberler { get; set; }
}
