using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SchoolSystem.Data;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        // GET: Students
        public async Task<IActionResult> Index(string? search,string? sortOrder ,string? sortType ,int pageSize=1 ,int pageNumber=1)
        {
            IQueryable<Student> students =  _context.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search.Trim();
                ViewBag.search = search;
                students = _context.Students.Where(s => s.Name.Contains(search));
            }
            if(sortType == "Name")
            {
                if(sortOrder == "asc")
                    students = students.OrderBy(s => s.Name);
                if(sortOrder == "desc")
                    students = students.OrderByDescending(s => s.Name);
            }
            if (sortType == "Age")
            {
                if (sortOrder == "asc")
                    students = students.OrderBy(s => s.Age);
                if (sortOrder == "desc")
                    students = students.OrderByDescending(s => s.Age);
            }
            students = students.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            ViewBag.pageNumber = pageNumber;
            ViewBag.pageSize = pageSize;

            ViewBag.AllClasses = _context.Classes;
            return View(await students.Include(c => c.Class).ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(c => c.Class)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.AllClasses = _context.Classes;
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewBag.AllClasses = _context.Classes;
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AverageMark,Class,Grade,Id,Name,Age,PhoneNumber,Email,ConfirmEmail,Password,ConfirmPassword,Address,BirthDate,CreatedAt,UpdatedAt,AttendanceTime,LeavingTime,JoiningDate,ImageUrl,ClassId")] Student student ,IFormFile? imageFile)
        {
            if (ModelState.IsValid == true)
            {
                if (imageFile == null)
                {
                    student.ImageUrl = "\\images\\No_Image.png";
                }
                else
                {
                    string imgExtension = Path.GetExtension(imageFile.FileName);
                    Guid imgGuid = Guid.NewGuid();
                    string imgName = imgGuid + imgExtension;
                    string imgUrl = "\\images\\" + imgName;
                    student.ImageUrl = imgUrl;

                    string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                    FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                    imageFile.CopyTo(imgStream);
                    imgStream.Dispose();
                }
                student.Age = DateTime.Now.Year - student.BirthDate.Year;
                student.CreatedAt = DateTime.Now;
                student.UpdatedAt = DateTime.Now;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AllClasses = _context.Classes;
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.AllClasses = _context.Classes;
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Age,PhoneNumber,AverageMark,Class,Grade,Id,Email,ConfirmEmail,Password,ConfirmPassword,Address,BirthDate,CreatedAt,UpdatedAt,AttendanceTime,LeavingTime,JoiningDate,ImageUrl")] Student student , IFormFile? imageFile)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        if(student.ImageUrl != "\\images\\No_Image.png")
                        {
                            string imgOldPath = _webHostEnvironment.WebRootPath + student.ImageUrl;
                            if (System.IO.File.Exists(imgOldPath))
                            {
                                System.IO.File.Delete(imgOldPath);
                            }
                        }
                        string imgExtension = Path.GetExtension(imageFile.FileName);
                        Guid imgGuid = Guid.NewGuid();
                        string imgName = imgGuid + imgExtension;
                        string imgUrl = "\\images\\" + imgName;
                        student.ImageUrl = imgUrl;

                        string imgPath = _webHostEnvironment.WebRootPath + imgUrl;
                        FileStream imgStream = new FileStream(imgPath, FileMode.Create);
                        imageFile.CopyTo(imgStream);
                        imgStream.Dispose();
                    }

                    student.UpdatedAt = DateTime.Now;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewBag.AllClasses = _context.Classes;
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> DeleteView(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.AllClasses = _context.Classes;
            return View("Delete",student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student.ImageUrl != "\\images\\No_Image.png") {
                string imgPath = _webHostEnvironment.WebRootPath + student.ImageUrl;
                System.IO.File.Delete(imgPath);
                    }
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return _context.Students.Any(e => e.Id == id);
        }
    }
}
