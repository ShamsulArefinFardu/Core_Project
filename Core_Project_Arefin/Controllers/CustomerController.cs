using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_Project_Arefin.Data;
using Core_Project_Arefin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core_Project_Arefin.Helper;

namespace Core_Project_Arefin.Controllers
{
    public class CustomerController : Controller
        {
            private readonly ApplicationDbContext _context;

            public CustomerController(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IActionResult> Index()
            {
                return View(await _context.Customers.ToListAsync());
            }
            [NoDirectAccess]
            public async Task<IActionResult> AddOrEdit(int id = 0)
            {
                if (id == 0)
                    return View(new Customer());
                else
                {
                    var transactionModel = await _context.Customers.FindAsync(id);
                    if (transactionModel == null)
                    {
                        return NotFound();
                    }
                    return View(transactionModel);
                }
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddOrEdit(int id, Customer transactionModel)
            {
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {                        
                        _context.Add(transactionModel);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        try
                        {
                            _context.Update(transactionModel);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!TransactionModelExists(transactionModel.CustomerID))
                            { return NotFound(); }
                            else
                            { throw; }
                        }
                    }
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Customers.ToList()) });
                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionModel) });
            }
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var transactionModel = await _context.Customers
                    .FirstOrDefaultAsync(m => m.CustomerID == id);
                if (transactionModel == null)
                {
                    return NotFound();
                }

                return View(transactionModel);
            }
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var transactionModel = await _context.Customers.FindAsync(id);
                _context.Customers.Remove(transactionModel);
                await _context.SaveChangesAsync();
                return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Customers.ToList()) });
            }

            private bool TransactionModelExists(int id)
            {
                return _context.Customers.Any(e => e.CustomerID == id);
            }
        }
    }