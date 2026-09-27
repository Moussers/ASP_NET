using Microsoft.EntityFrameworkCore;

public class AcademyContext(DbContextOptions<AcademyContext> options) : DbContext(options)
{
    public DbSet<Academy.Models.Discipline> Disciplines { get; set; } = default!;
    //DbSet/DbSet<TEntity>: представляет набор объектов (список объектов), которые
    //хранятся в базе данных;
    //DbContext: определяет контекст данных, используемый для взаимодействия с базой
    //данных.
    public DbSet<Academy.Models.Student> Students { get; set; } = default!;
    public DbSet<Academy.Models.Teacher> Teachers { get; set; } = default!;
    public DbSet<Academy.Models.Group> Groups { get; set; } = default!;
    public DbSet<Academy.Models.Direction> Directions { get; set; } = default!;

    public DbSet<Academy.Models.TeachersDisciplinesRelation> TeachersDisciplinesRelation { get; set; } = default!;
}
