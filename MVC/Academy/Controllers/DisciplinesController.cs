using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;
using Academy;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Diagnostics.Contracts;

public class DisciplinesController : Controller
{
    private readonly AcademyContext _context;

    public DisciplinesController(AcademyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber, int? id, int? teacherID)    
    {
        ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["LessonSortParam"] = String.IsNullOrEmpty(sortOrder) ? "lesson_desc" : "";
        if (searchString != null) pageNumber = 1;
        ViewData["CurrentFilter"] = searchString;
        
        IQueryable<Discipline> disciplines = from discipline in _context.Disciplines select discipline;

        if (!String.IsNullOrEmpty(searchString)) 
        {
            disciplines = disciplines.Where
            (
                s => 
                s.discipline_name.Contains(searchString)
            );
        }

        switch (sortOrder) 
        {
            case "name_desc":   disciplines = disciplines.OrderByDescending(d => d.discipline_name);    break;
            //OrderByDescending - сортировка по убыванию
            case "lesson_desc": disciplines = disciplines.OrderByDescending(d => d.number_of_lessons);  break;
            default:            disciplines = disciplines.OrderBy(d => d.discipline_name);              break;
            //OrderBy - сортировка по возрастанию
        }

        int pageSize = 5;
        Academy.Models.ViewModels.DiscplineIndexData viewModel = new Academy.Models.ViewModels.DiscplineIndexData();
        viewModel.Disciplines = await _context.Disciplines
            .Include(tr => tr.TeachersRelations)
                .ThenInclude(t => t.Teacher)
            .AsNoTracking()
            .OrderBy(d => d.discipline_name)
            .ToListAsync();

        if (id != null) 
        {
            ViewData["DisciplineID"] = id.Value;
            Discipline discipline = viewModel.Disciplines.Where(i => i.discipline_id == id.Value).Single();
            viewModel.Teachers = discipline.TeachersRelations.Select(tr => tr.Teacher);
        }

        if (teacherID != null) 
        {
            ViewData["TeacherID"] = teacherID.Value;
            viewModel.Teachers = viewModel.Teachers.Where(t => t.teacher_id == teacherID);
        }

        return View(viewModel);
        //return View
        //(
        //    await PaginatedList<Discipline>.CreateAsync
        //    (
        //        disciplines.AsNoTracking(), 
        //        pageNumber ?? 1,
        //        pageSize
        //    )
        //);
        //return View(await disciplines.AsNoTracking().ToListAsync());
        //return View(await _context.Disciplines.AsNoTracking().ToListAsync());
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
