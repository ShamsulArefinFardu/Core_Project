using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core_Project_Arefin.Data;
using Core_Project_Arefin.Models;

namespace Core_Project_Arefin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public readonly IHostingEnvironment _hostingEnvironment;

        public CategoriesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.CategoryID = new SelectList(_context.Categories, "ID", "Name");
            return View(await _context.Categories.ToListAsync());
        }
        public ActionResult GetCategoryWiseItems(long? id)
        {
            if (id == null)
            {
                NotFound();
            }

            ViewData["id"] = id;
            List<Item> items = _context.Items.Where(e => e.CategoryID == id).ToList();

            if (items == null)
            {
                NotFound();
            }

            return PartialView("CategoryWiseItems", items);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item items, Category category, IFormFile[] Image)
        {
            try
            {

                if (Image != null)
                {
                    if (category.Items.Count == Image.Count())
                    {
                        for (int i = 0; i < category.Items.Count; i++)
                        {

                            string picture = System.IO.Path.GetFileName(Image[i].FileName);
                            var file = picture;
                            var uploadFile = Path.Combine(_hostingEnvironment.WebRootPath, "images", picture);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                Image[i].CopyTo(ms);
                                category.Items[i].Image = ms.GetBuffer();
                            }
                        }
                    }
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    TempData["id"] = category.ID;
                    return RedirectToAction("Index");
                }

                return View(category);
            }
            catch (Exception)
            {
                return View(category);
            }
        }

       
        public IActionResult Delete(long id)
        {
            Category category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(long id)
        {
            Item item = _context.Items.Find(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
    }
}