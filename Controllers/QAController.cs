using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcomp1.Areas.Identity.Data;
using tcomp1.Data;
using tcomp1.Models;

namespace tcomp1.Controllers
{
    [Authorize(Roles = "QA,Admin")]
    public class QAController : Controller
    {
        private readonly tcomp1Context _context;

        public QAController(tcomp1Context context)
        {
            _context = context;
        }

        // GET: QA
        
        public async Task<IActionResult> Index()
        {
            var tcomp1Context = _context.ideas.Include(i => i.tcomp1User).Where(i=>i.Accep==false);
            return View(await tcomp1Context.ToListAsync());
        }


        // GET: QA/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.ideas
                .Include(i => i.tcomp1User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: QA/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: QA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Descripstion,DateTime,ImgUrl,Category,Accep,Incognito,Like,DisLike,File,UserId")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // GET: QA/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // POST: QA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Descripstion,DateTime,ImgUrl,Category,Accep,Incognito,File,UserId")] Idea idea)
        {
            if (id != idea.Id)
            {
                return NotFound();
            }
            
            

            
            if (id == idea.Id)
            {
                try
                {
                    _context.Update(idea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaExists(idea.Id))
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

            return View(idea);
        }

        // GET: QA/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ideas == null)
            {
                return NotFound();
            }

            var idea = await _context.ideas
                .Include(i => i.tcomp1User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // POST: QA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ideas == null)
            {
                return Problem("Entity set 'tcomp1Context.ideas'  is null.");
            }
            var idea = await _context.ideas.FindAsync(id);
            if (idea != null)
            {
                _context.ideas.Remove(idea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(string id)
        {
          return _context.ideas.Any(e => e.Id == id);
        }
    }
}
