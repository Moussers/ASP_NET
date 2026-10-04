
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;
using Microsoft.Data.SqlClient;
using System.Numerics;

public class TeachersController : Controller
{
    private readonly AcademyContext _context;

    public TeachersController(AcademyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder)    
    {
        ViewData["LNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
        ViewData["FNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
        ViewData["MNameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "middle_name_desc" : "";
        ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
        
        IQueryable<Teacher> teachers = from teaher in _context.Teachers select teaher;
        switch (sortOrder) 
        {
            case "last_name_desc":      teachers = teachers.OrderByDescending(t => t.last_name);       break;
            case "first_name_desc":     teachers = teachers.OrderByDescending(t => t.first_name);      break;
            case "middle_name_desc":    teachers = teachers.OrderByDescending(t => t.middle_name);     break;
            case "date_desc":           teachers = teachers.OrderByDescending(t => t.birth_date);      break;
            case "Date":                teachers = teachers.OrderBy(t => t.birth_date);                break;
            default:                    teachers = teachers.OrderBy(t => t.last_name);                 break;
        }
        return View(await teachers.AsNoTracking().ToListAsync());
        //return View(await _context.Teachers.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("teacher_id,work_since,rate,DisplinesResations,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        //в Edit тер€етс€ teacher_id
        //–ешение проблемы: заменить на примере страницы Teachers заменить teacher_id на просто id,
        //*Ќа примере Teachers. —истема переводит teacher_id в просто в id, в результате id
        //тер€етс€ и teacher_id всегда будет равен null
        if (id == null)
            if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }
        return View(teacher);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("teacher_id,work_since,rate,DisplinesResations,last_name,first_name,middle_name,birth_date,email,phone,photo,FullName")] Teacher teacher)
    {
        if (id != teacher.teacher_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.teacher_id))
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
        return View(teacher);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.teacher_id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(int? id)
    {
        return _context.Teachers.Any(e => e.teacher_id == id);
    }
}
