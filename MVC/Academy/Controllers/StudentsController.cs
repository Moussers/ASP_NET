
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class StudentsController : Controller
{
    private readonly AcademyContext _context;

    public StudentsController(AcademyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder)    
    {
        ViewData["LNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
        ViewData["FNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
        ViewData["MNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "middle_name_desc" : "";
        ViewData["GroupSortParam"] = String.IsNullOrEmpty(sortOrder) ? "group_desc" : "";
        ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";

        IQueryable<Student> students = from student in _context.Students select student;
        switch (sortOrder) 
        {
            case "last_name_desc":   students = students.OrderByDescending(s => s.last_name);           break;
            case "first_name_desc":   students = students.OrderByDescending(s => s.first_name);         break;
            case "middle_name_desc":   students = students.OrderByDescending(s => s.middle_name);       break;
            case "date_desc":   students = students.OrderByDescending(s => s.birth_date);               break;
            //OrderByDescending - сортировка по убыванию
            case "group_desc":   students = students.OrderByDescending(s => s.group);                   break;
            case "Date":        students = students.OrderBy(s => s.birth_date);                         break;
            //OrderBy - сортировка по возрастанию
            default: students = students.OrderBy(s => s.last_name);                                     break;
            
        }

        return View(await students.AsNoTracking().ToListAsync());
        //return View(await _context.Students.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.stud_id == id);
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
    public async Task<IActionResult> Create([Bind("stud_id,group,Group,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Student student)
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
        //в Edit теряется stud_id
        //Решение проблемы: заменить на примере страницы Students заменить stud_id на просто id,
        //*На примере Students. Система переводит stud_id в просто в id, в результате id
        //теряется и stud_id всегда будет равен null
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
    public async Task<IActionResult> Edit(int? id, [Bind("stud_id,group,Group,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Student student)
    {
        if (id != student.stud_id)
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
                if (!StudentExists(student.stud_id))
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
            .FirstOrDefaultAsync(m => m.stud_id == id);
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
        return _context.Students.Any(e => e.stud_id == id);
    }
}
