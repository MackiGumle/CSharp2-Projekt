using CSharp2Projekt;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            this.ViewBag.Schools = await DatabaseService.QueryBuilder.SelectAllAsync<School>();
            this.ViewBag.Programs = await DatabaseService.QueryBuilder.SelectAllAsync<SchoolPrograms>();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult MainForm(MainForm form)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Ok");
            }
            return View("Index", form);
        }

        [HttpGet("/SchoolProgram/{id}")]
        public async Task<ActionResult<IEnumerable<Program>>> GetSchoolPrograms(int id)
        {
            try
            {
                var allPrograms = await DatabaseService.QueryBuilder.SelectAllAsync<SchoolPrograms>();

                var programs = allPrograms.Where(p => p.School == id).ToList();
                if (!programs.Any())
                {
                    return NotFound();
                }

                return Ok(programs);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddApp(MainForm form)
        {
            if (ModelState.IsValid)
            {

                if (form.Program1 == form.Program2 || form.Program1 == form.Program3 || form.Program2 == form.Program3 && (form.Program2 != null || form.Program3 != null))
                {
                    return StatusCode(400, "Programs must be different");
                }

                var student = new Student
                {
                    FName = form.FName,
                    LName = form.LName,
                    Email = form.Email,
                    Phone = form.Phone,
                    Country = form.Country,
                    City = form.City,
                    Street = form.Street
                };

                var res = await DatabaseService.QueryBuilder.InsertAsync<Student>(student);

                if (res == null)
                {
                    return StatusCode(500, "Failed to insert student");
                }

                var program1 = await DatabaseService.QueryBuilder.SelectByIdAsync<SchoolPrograms>(form.Program1);
                if (program1 == null)
                {
                    return StatusCode(404, "Program 1 not found");
                }

                if (form.Program2 != null)
                {
                    var program2 = await DatabaseService.QueryBuilder.SelectByIdAsync<SchoolPrograms>((long)form.Program2);
                    if (form.Program2 != null && program2 == null)
                    {
                        return StatusCode(404, "Program 2 not found");
                    }
                }

                if (form.Program3 != null)
                {
                    var program3 = await DatabaseService.QueryBuilder.SelectByIdAsync<SchoolPrograms>((long)form.Program3);
                    if (form.Program3 != null && program3 == null)
                    {
                        return StatusCode(404, "Program 3 not found");
                    }
                }

                var application = new Applications
                {
                    Student = (long)res,
                    Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Program1 = form.Program1,
                    Program2 = form.Program2,
                    Program3 = form.Program3
                };
                
                var res2 = await DatabaseService.QueryBuilder.InsertAsync<Applications>(application);

                if (res2 == null)
                {
                    return StatusCode(500, "Failed to insert application");
                }

                return RedirectToAction("Ok");
            }

            return View("Index", form);
        }
    }
}
