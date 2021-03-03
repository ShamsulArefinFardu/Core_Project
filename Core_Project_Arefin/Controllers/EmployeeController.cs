using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core_Project_Arefin.Data;
using Core_Project_Arefin.Models;
using Core_Project_Arefin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Core_Project_Arefin.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment webHostEnvironment;
        public EmployeeController(ApplicationDbContext context, IHostingEnvironment hostEnvironment)
        {
            db = context;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(
        string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var employees = from s in db.Employees
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.EmployeeName.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.EmployeeName);
                    break;
                case "Date":
                    employees = employees.OrderBy(s => s.JoiningDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(s => s.JoiningDate);
                    break;
                default:
                    employees = employees.OrderBy(s => s.EmployeeName);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        //public IActionResult Index()
        //{
        //    return View(db.Employees.ToList());
        //}
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = db.Employees.FirstOrDefault(m => m.Id == id);

            var employeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                EmployeeName = employee.EmployeeName,
                JoiningDate = employee.JoiningDate,
                ResignDate = employee.ResignDate,
                Address = employee.Address,
                ExistingImage = employee.ProfilePicture
            };

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                Employee employee = new Employee
                {
                    EmployeeName = model.EmployeeName,
                    JoiningDate = model.JoiningDate,
                    ResignDate = model.ResignDate,
                    Address = model.Address,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    ProfilePicture = uniqueFileName
                };

                db.Add(employee);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = db.Employees.Find(id);
            var employeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                EmployeeName = employee.EmployeeName,
                JoiningDate = employee.JoiningDate,
                ResignDate = employee.ResignDate,
                Address = employee.Address,
                Password = employee.Password,
                ConfirmPassword = employee.ConfirmPassword,

                ExistingImage = employee.ProfilePicture
            };

            if (employee == null)
            {
                return NotFound();
            }
            return View(employeeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = db.Employees.Find(model.Id);
                employee.EmployeeName = model.EmployeeName;
                employee.JoiningDate = model.JoiningDate;
                employee.ResignDate = model.ResignDate;
                employee.Address = model.Address;
                employee.Password = model.Password;
                employee.ConfirmPassword = model.ConfirmPassword;


                if (model.EmployeePicture != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }

                    employee.ProfilePicture = ProcessUploadedFile(model);
                }
                db.Update(employee);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = db.Employees.FirstOrDefault(m => m.Id == id);

            var employeeViewModel = new EmployeeViewModel()
            {
                Id = employee.Id,
                EmployeeName = employee.EmployeeName,
                JoiningDate = employee.JoiningDate,
                ResignDate = employee.ResignDate,
                Address = employee.Address,
                ExistingImage = employee.ProfilePicture
            };
            if (employee == null)
            {
                return NotFound();
            }

            return View(employeeViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = db.Employees.Find(id);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", employee.ProfilePicture);
            db.Employees.Remove(employee);
            if (db.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return db.Employees.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile(EmployeeViewModel model)
        {
            string uniqueFileName = null;

            if (model.EmployeePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.EmployeePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.EmployeePicture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

    }
}

