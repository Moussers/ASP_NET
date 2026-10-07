
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity;

public class StudentsController : Controller
{
    private readonly ContosoUniversityContext _context;

    public StudentsController(ContosoUniversityContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)    
    {
        ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
        if (searchString != null) pageNumber = 1;
        ViewData["CurrentFilter"] = searchString;

        IQueryable<Student> students = from student in _context.Students select student;

        if (!String.IsNullOrEmpty(searchString)) 
        {
            students = students.Where
                (
                s => 
                s.LastName.Contains(searchString) ||
                s.FirstName.Contains(searchString)
                );
        }

        switch (sortOrder) 
        {
            case "name_desc":   students = students.OrderByDescending(s => s.LastName);         break;
            case "date_desc":   students = students.OrderByDescending(s => s.EnrollmentDate);   break;
            case "Date":        students = students.OrderBy(s => s.EnrollmentDate);             break;
            default:            students = students.OrderBy(s => s.LastName);                   break;
        }

        int pageSize = 2;
        return View
            (
                await PaginatedList<Student>.CreateAsync
                (
                    students.AsNoTracking(),
                    pageNumber ?? 1,
                    pageSize
                )
            );
        //turn View(await students.AsNoTracking().ToListAsync());
        //Чтобы данные не помещались в кэш, применяется метод AsNoTracking(). При его применении
        //возвращаемые из запроса данные не кэшируются. 
        //return View(await _context.Students.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,EnrollmentDate,Enrollments")] Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("ID,LastName,FirstName,EnrollmentDate,Enrollments")] Student student)
    {
        if (id != student.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.ID == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int? id)
    {
        return _context.Students.Any(e => e.ID == id);
    }
}
