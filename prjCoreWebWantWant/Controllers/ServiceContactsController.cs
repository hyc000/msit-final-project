using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;

namespace prjCoreWebWantWant.Controllers
{
    public class ServiceContactsController : Controller
    {
        private readonly NewIspanProjectContext _context;

        public ServiceContactsController(NewIspanProjectContext context)
        {
            _context = context;
        }

        // GET: ServiceContacts
        public IActionResult Index()
        {           
            return View();
        }

        // GET: ServiceContacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceContacts == null)
            {
                return NotFound();
            }

            var serviceContact = await _context.ServiceContacts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.ServiceContactId == id);
            if (serviceContact == null)
            {
                return NotFound();
            }

            return View(serviceContact);
        }

        // GET: ServiceContacts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.MemberAccounts, "AccountId", "AccountId");
            return View();
        }

        // POST: ServiceContacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceContactId,AccountId,ComplaintTitle,ComplaintContent,ProcessStatus")] ServiceContact serviceContact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceContact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.MemberAccounts, "AccountId", "AccountId", serviceContact.AccountId);
            return View(serviceContact);
        }

        // GET: ServiceContacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceContacts == null)
            {
                return NotFound();
            }

            var serviceContact = await _context.ServiceContacts.FindAsync(id);
            if (serviceContact == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.MemberAccounts, "AccountId", "AccountId", serviceContact.AccountId);
            return View(serviceContact);
        }

        // POST: ServiceContacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceContactId,AccountId,ComplaintTitle,ComplaintContent,ProcessStatus")] ServiceContact serviceContact)
        {
            if (id != serviceContact.ServiceContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceContactExists(serviceContact.ServiceContactId))
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
            ViewData["AccountId"] = new SelectList(_context.MemberAccounts, "AccountId", "AccountId", serviceContact.AccountId);
            return View(serviceContact);
        }

        // GET: ServiceContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceContacts == null)
            {
                return NotFound();
            }

            var serviceContact = await _context.ServiceContacts
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.ServiceContactId == id);
            if (serviceContact == null)
            {
                return NotFound();
            }

            return View(serviceContact);
        }

        // POST: ServiceContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceContacts == null)
            {
                return Problem("Entity set 'NewIspanProjectContext.ServiceContacts'  is null.");
            }
            var serviceContact = await _context.ServiceContacts.FindAsync(id);
            if (serviceContact != null)
            {
                _context.ServiceContacts.Remove(serviceContact);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceContactExists(int id)
        {
          return (_context.ServiceContacts?.Any(e => e.ServiceContactId == id)).GetValueOrDefault();
        }
    }
}
