
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;

public class InstructorsController : Controller
{
    private readonly ContosoUniversityContext _context;

    public InstructorsController(ContosoUniversityContext context)
    {
        _context = context;
    }

    // GET: INSTRUCTORS
    public async Task<IActionResult> Index(int? id, int? courseID)    
    {
        ContosoUniversity.Models.ViewModels.InstructorIndexData viewModel = new ContosoUniversity.Models.ViewModels.InstructorIndexData();
        viewModel.Instructors = await _context
            .Instructors
            .Include(i => i.OfficeAssignments)
            .Include(i => i.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                    .ThenInclude(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
            .Include(i => i.CourseAssignments)
                .ThenInclude(ca => ca.Course)
                    .ThenInclude(c => c.Department)
            .AsNoTracking()
            .OrderBy(i => i.LastName)
            .ToListAsync();

        if (id != null) 
        {
            ViewData["InstructorID"] = id.Value;
            Instructor instructor = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
            viewModel.Courses = instructor.CourseAssignments.Select(ca => ca.Course);
        }

        if (courseID != null) 
        {
            ViewData["CourseID"] = courseID.Value;
            viewModel.Enrollments = viewModel.Courses.Where(c => c.CourseID == courseID)
                .Single().Enrollments;
        }

        //return View(await _context.Instructors.ToListAsync());
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(i => i.OfficeAssignments)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,HireDate,FullName,CourseAssignments,OfficeAssignments")] Instructor instructor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(instructor);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context
                                .Instructors
                                .Include(i => i.OfficeAssignments)
                                .Include(i => i.CourseAssignments)
                                    .ThenInclude(ca => ca.Course)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(i => i.ID == id);
        if (instructor == null)
        {
            return NotFound();
        }
        PopulateAssignmentCourseData(instructor);
        return View(instructor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    //public async Task<IActionResult> Edit(int? id, [Bind("ID,LastName,FirstName,HireDate,FullName,CourseAssignments,OfficeAssignments")] Instructor instructor)
    public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
    {
        if (id != null)
        {
            return NotFound();
        }

        Instructor instructor = await _context.Instructors
                                    .Include(i => i.OfficeAssignments)
                                    .FirstOrDefaultAsync(i => i.ID == id);

        Task<bool> ok = TryUpdateModelAsync<Instructor>
            (
                instructor,
                "",
                i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignments
            );
        if (ok.IsCompleted) 
        {
            if (String.IsNullOrWhiteSpace(instructor.OfficeAssignments?.Location))
                instructor.OfficeAssignments = null;

            UpdateInstructorCourses(selectedCourses, instructor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) 
            {
                ModelState.AddModelError("", "Невозможно обновить данные");
                return View(instructor);
            }
        }
        UpdateInstructorCourses (selectedCourses, instructor);
        PopulateAssignmentCourseData(instructor);
        return Redirect($"/instructors/Details/{instructor.ID}");
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .FirstOrDefaultAsync(m => m.ID == id);
        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool InstructorExists(int? id)
    {
        return _context.Instructors.Any(e => e.ID == id);
    }

    void PopulateAssignmentCourseData(Instructor instructor)
    {
        DbSet<Course> courses = _context.Courses;
        HashSet<int> assignedCourses = new HashSet<int>
            (
                instructor.CourseAssignments.Select(c => c.CourseID)
            );
        List<AssignedCourseData> viewModel = new List<AssignedCourseData>();
        foreach (Course course in courses)
        {
            viewModel.Add
            (
                new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = assignedCourses.Contains(course.CourseID)
                }
            );
        }
        ViewData["Courses"] = viewModel;
    }

    void UpdateInstructorCourses(string[] selectedCourses, Instructor instructor)
    {
        if (selectedCourses == null)
        {
            instructor.CourseAssignments = new List<CourseAssignment>();
            return;
        }

        HashSet<string> selectedCoursesHS = new HashSet<string>(selectedCourses);
        HashSet<int> instructorCoursesHS = new HashSet<int>
        (
            instructor.CourseAssignments.Select(c => c.Course.CourseID)
        );

        foreach (Course course in _context.Courses)
        {
            if (selectedCourses.Contains(course.CourseID.ToString()))
            {
                if (!instructorCoursesHS.Contains(course.CourseID))
                {
                    instructor.CourseAssignments.Add
                    (
                            new CourseAssignment
                            {
                                InstructorID = instructor.ID
                            }
                    );
                }
                else
                {
                    if (instructorCoursesHS.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove =
                            instructor
                            .CourseAssignments
                            .FirstOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }
    }

}
