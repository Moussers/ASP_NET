
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class GroupsController : Controller
{
    private readonly AcademyContext _context;

    public GroupsController(AcademyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string searchString)    
    {
        ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DaysSortParam"] = String.IsNullOrEmpty(sortOrder) ? "day_desc" : "";
        ViewData["TimeSortParam"] = sortOrder == "Time" ? "time_desc" : "Time";
        ViewData["DateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
        ViewData["CurrentFilter"] = searchString;

        IQueryable<Group> groups = from gr in _context.Groups select gr;

        if (!String.IsNullOrEmpty(searchString)) 
        {
            groups = groups.Where
                (
                    g =>
                    g.group_name.Contains(searchString)
                );
        }

        switch (sortOrder) 
        {
            case "name_desc":   groups = groups.OrderByDescending(g => g.group_name);       break;
            case "day_desc":    groups = groups.OrderByDescending(g => g.learning_days);    break;
            case "time_desc":   groups = groups.OrderByDescending(g => g.start_time);       break;
            case "Time":        groups = groups.OrderBy(g => g.start_time);                 break;
            case "date_desc":   groups = groups.OrderByDescending(g => g.start_date);       break;
            case "Date":        groups = groups.OrderBy(g => g.start_date);                 break;
            default:            groups = groups.OrderBy(g => g.group_name);                 break;
        }

        return View(await groups.AsNoTracking().ToListAsync());
        //return View(await _context.Groups.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .FirstOrDefaultAsync(m => m.group_id == id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    // GET: GROUPS/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("group_id,group_name,direction,learning_days,start_time,start_date,Direction,Students")] Group group)
    {
        if (ModelState.IsValid)
        {
            _context.Add(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(group);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }
        return View(group);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("group_id,group_name,direction,learning_days,start_time,start_date,Direction,Students")] Group group)
    {
        if (id != group.group_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(group);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(group.group_id))
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
        return View(group);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var group = await _context.Groups
            .FirstOrDefaultAsync(m => m.group_id == id);
        if (group == null)
        {
            return NotFound();
        }

        return View(group);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group != null)
        {
            _context.Groups.Remove(group);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GroupExists(int? id)
    {
        return _context.Groups.Any(e => e.group_id == id);
    }
}
