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
    public class PublishersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public readonly IHostingEnvironment _hostingEnvironment;

        public PublishersController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.PublisherID = new SelectList(_context.Publishers, "ID", "Name");
            return View(await _context.Publishers.ToListAsync());
        }
        public ActionResult GetCategoryWiseItems(long? id)
        {
            if (id == null)
            {
                NotFound();
            }

            ViewData["id"] = id;
            List<Book> books = _context.Books.Where(e => e.PublisherID == id).ToList();

            if (books == null)
            {
                NotFound();
            }

            return PartialView("CategoryWiseItems", books);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book books, Publisher publisher, IFormFile[] Image)
        {
            try
            {

                if (Image != null)
                {
                    if (publisher.Books.Count == Image.Count())
                    {
                        for (int i = 0; i < publisher.Books.Count; i++)
                        {

                            string picture = System.IO.Path.GetFileName(Image[i].FileName);
                            var file = picture;
                            var uploadFile = Path.Combine(_hostingEnvironment.WebRootPath, "images", picture);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                Image[i].CopyTo(ms);
                                publisher.Books[i].Image = ms.GetBuffer();
                            }
                        }
                    }
                    _context.Publishers.Add(publisher);
                    _context.SaveChanges();
                    TempData["id"] = publisher.ID;
                    return RedirectToAction("Index");
                }

                return View(publisher);
            }
            catch (Exception)
            {
                return View(publisher);
            }
        }

       
        public IActionResult Delete(long id)
        {
            Publisher publisher = _context.Publishers.Find(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(long id)
        {
            Book book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
    }
}