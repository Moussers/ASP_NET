using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class DisciplinesController : Controller
{
    private readonly AcademyContext _context;

    public DisciplinesController(AcademyContext context)
    {
        _context = context;
    }

    // GET: DISCIPLINES
    public async Task<IActionResult> Index(string sortOrder)    
    {
        ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["LessonSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lesson_desc" : "";
        
        IQueryable<Discipline> disciplines = from discipline in _context.Disciplines select discipline;
        switch (sortOrder) 
        {
            case "name_desc":   disciplines = disciplines.OrderByDescending(d => d.discipline_name);    break;
            //OrderByDescending - сортировка по убыванию
            case "lesson_desc": disciplines = disciplines.OrderByDescending(d => d.number_of_lessons);  break;
            default:            disciplines = disciplines.OrderBy(d => d.discipline_name);              break;
            //OrderBy - сортировка по возрастанию
        }

        return View(await disciplines.AsNoTracking().ToListAsync());
        //return View(await _context.Disciplines.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discipline = await _context.Disciplines
            .FirstOrDefaultAsync(m => m.discipline_id == id);
        if (discipline == null)
        {
            return NotFound();
        }

        return View(discipline);
    }

    // GET: DISCIPLINES/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("discipline_id,discipline_name,number_of_lessons")] Discipline discipline)
    {
        if (ModelState.IsValid)
        {
            _context.Add(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(discipline);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discipline = await _context.Disciplines.FindAsync(id);
        if (discipline == null)
        {
            return NotFound();
        }
        return View(discipline);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("discipline_id,discipline_name,number_of_lessons")] Discipline discipline)
    {
        if (id != discipline.discipline_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(discipline);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisciplineExists(discipline.discipline_id))
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
        return View(discipline);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var discipline = await _context.Disciplines.FirstOrDefaultAsync(m => m.discipline_id == id);
        if (discipline == null)
        {
            return NotFound();
        }

        return View(discipline);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var discipline = await _context.Disciplines.FindAsync(id);
        if (discipline != null)
        {
            _context.Disciplines.Remove(discipline);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DisciplineExists(int? id)
    {
        return _context.Disciplines.Any(e => e.discipline_id == id);
    }
}
