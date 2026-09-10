using Microsoft.EntityFrameworkCore;

public class AcademyString 
{
    public async Task<List<Academy.Models.Student>> GetStudentsAsync()
    {
        using (var context = new AcademyContext())
        {
            var students = await context.Students.ToListAsync();
            return students;
        }
    }
    public async Task<List<Academy.Models.Teacher>> GetTeachersAsync()
    {
        using (var context = new AcademyContext())
        {
            var teachers = await context.Teachers.ToListAsync();
            return teachers;
        }
    }
    public async Task<List<Academy.Models.Discipline>> GetDisciplinesAsync()
    {
        using (var context = new AcademyContext())
        {
            var disciplines = await context.Disciplines.ToListAsync();
            return disciplines;
        }
    }
    public async Task<List<Academy.Models.Group>> GetGroupsAsync()
    {
        using (var context = new AcademyContext())
        {
            var groups = await context.Groups.ToListAsync();
            return groups;
        }
    }
}

public class AcademyContext : DbContext
{
    public AcademyContext() 
    {}

    public DbSet<Academy.Models.Student> Students { get; set; }
    public DbSet<Academy.Models.Teacher> Teachers { get; set; }
    public DbSet<Academy.Models.Group> Groups { get; set; }
    public DbSet<Academy.Models.Discipline> Disciplines { get; set; }

    public AcademyContext(DbContextOptions<AcademyContext> options) : base(options) 
    {
        _ = Database.EnsureCreated();
        // _ - discard C# 7.0
    }
}
