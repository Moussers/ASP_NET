
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academy.Models;
using Academy;

public class DirectionsController : Controller
{
    private readonly AcademyContext _context;

    public DirectionsController(AcademyContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)    
    {
        ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        if (searchString != null) pageNumber = 1;
        ViewData["CurrentFilter"] = searchString;

        IQueryable<Direction> directions = from direction in _context.Directions select direction;
        if (!String.IsNullOrEmpty(searchString)) 
        {
            directions = directions.Where(d => d.direction_name.Contains(searchString));
        }

        switch (sortOrder) 
        {
            case "name_desc":   directions = directions.OrderByDescending(d => d.direction_name);   break;
            default:            directions = directions.OrderBy(d => d.direction_name);             break;
        }

        int pageSize = 5;
        return View
            (
                await PaginatedList<Direction>.CreateAsync
                (
                    directions.AsNoTracking(),
                    pageNumber ?? 1,
                    pageSize
                )
            );
        //return View(await directions.AsNoTracking().ToListAsync());
        //return View(await _context.Directions.AsNoTracking().ToListAsync());
        //return View(await _context.Directions.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direction = await _context.Directions
            .FirstOrDefaultAsync(m => m.direction_id == id);
        if (direction == null)
        {
            return NotFound();
        }

        return View(direction);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("direction_id,direction_name,Groups")] Direction direction)
    {
        if (ModelState.IsValid)
        {
            _context.Add(direction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(direction);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direction = await _context.Directions.FindAsync(id);
        if (direction == null)
        {
            return NotFound();
        }
        return View(direction);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("direction_id,direction_name,Groups")] Direction direction)
    {
        if (id != direction.direction_id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(direction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectionExists(direction.direction_id))
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
        return View(direction);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var direction = await _context.Directions
            .FirstOrDefaultAsync(m => m.direction_id == id);
        if (direction == null)
        {
            return NotFound();
        }

        return View(direction);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var direction = await _context.Directions.FindAsync(id);
        if (direction != null)
        {
            _context.Directions.Remove(direction);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DirectionExists(int? id)
    {
        return _context.Directions.Any(e => e.direction_id == id);
    }
}
