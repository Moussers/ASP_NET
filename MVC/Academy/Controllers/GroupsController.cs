
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

    public async Task<IActionResult> Index()    
    {
        return View(await _context.Groups.ToListAsync());
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
