using Microsoft.EntityFrameworkCore;

public class AcademyContext(DbContextOptions<AcademyContext> options) : DbContext(options)
{
    public DbSet<Academy.Models.Disciplines> Disciplines { get; set; } = default!;
    //DbSet - список
    public DbSet<Academy.Models.Students> Students { get; set; } = default!;
    public DbSet<Academy.Models.Teachers> Teachers { get; set; } = default!;
    public DbSet<Academy.Models.Groups> Groups { get; set; } = default!;
    public DbSet<Academy.Models.Directions> Directions { get; set; } = default!;
}
