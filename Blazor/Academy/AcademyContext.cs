using Microsoft.EntityFrameworkCore;

public class AcademyContext(DbContextOptions<AcademyContext> options) : DbContext(options)
{
    public DbSet<Academy.Models.Discipline> Disciplines { get; set; } = default!;
    //DbSet - список
    public DbSet<Academy.Models.Student> Students{ get; set; } = default!;
}
