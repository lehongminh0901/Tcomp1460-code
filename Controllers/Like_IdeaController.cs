using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcomp1.Areas.Identity.Data;
using tcomp1.Data;
using tcomp1.Models;
using static System.Formats.Asn1.AsnWriter;

namespace tcomp1.Controllers
{
    public class Like_IdeaController : Controller
    {
        private readonly tcomp1Context _context;
        private readonly UserManager<tcomp1User> _userManager;

        public Like_IdeaController(tcomp1Context context, UserManager<tcomp1User> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        // GET: Like_Idea
        public async Task<IActionResult> Index()
        {
            var tcomp1Context = _context.Like_Ideas.Include(l => l.Idea).Include(l => l.tcomp1User);
            return View(await tcomp1Context.ToListAsync());
        }

        // GET: Like_Idea/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Like_Ideas == null)
            {
                return NotFound();
            }

            var like_Idea = await _context.Like_Ideas
                .Include(l => l.Idea)
                .Include(l => l.tcomp1User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (like_Idea == null)
            {
                return NotFound();
            }

            return View(like_Idea);
        }

        // GET: Like_Idea/Create
        public async Task<IActionResult> Create_Like(string id, Like_Idea like_Idea)
        {
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            Like_Idea like_Idea1 = await _context.Like_Ideas.FirstOrDefaultAsync(li => li.UserId == user.Id);
            if (like_Idea1 != null)
            {
                return RedirectToAction("EditLike", "Like_Idea", new { @id = id });
            }
            if (like_Idea1 == null)
            {
                like_Idea.UserId = user.Id;
                like_Idea.IdeaId = id;
                
                var rand = Guid.NewGuid();
                var myRandomNumber = rand.ToString();
                like_Idea.Id = myRandomNumber;
                _context.Add(like_Idea);
                await _context.SaveChangesAsync();
                
            }

            return RedirectToAction("Index", "Home");
            
        }

        // POST: Like_Idea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Like(string id ,[Bind("IdeaId,Like,UserId")] Like_Idea like_Idea)
        {
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            Like_Idea like_Idea1 = await _context.Like_Ideas.FirstOrDefaultAsync(li => li.UserId == user.Id);
            if (like_Idea1 != null)
            {
                return RedirectToAction("Edit","Like_Idea", new { @id = id });
            }
            if (like_Idea1==null)
            {
                like_Idea.UserId=user.Id;
                like_Idea.IdeaId = id;
                like_Idea.Like = true;
                _context.Add(like_Idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return RedirectToAction("Index", "Home");
        }*/
        public async Task<IActionResult> Create_DisLike(string id, Like_Idea like_Idea)
        {
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.Id == like_Idea.UserId)
            {
                return RedirectToAction("EditDisLike", "Like_Idea", new { @id = id });
            }
            if (ModelState.IsValid)
            {
                like_Idea.UserId = user.Id;
                like_Idea.IdeaId = id;
                /*like_Idea.DisLike = 1;*/
                _context.Add(like_Idea);
                await _context.SaveChangesAsync();
               
            }
            
            return RedirectToAction("Index", "Home");
        }

        // GET: Like_Idea/Edit/5
        /*public async Task<IActionResult> EditLike(string id)
        {
            if (id == null )
            {
                return NotFound();
            }

            Like_Idea like_Idea = await _context.Like_Ideas.FirstOrDefaultAsync(li=>li.IdeaId == id);
            if (like_Idea.Like == 1)
            {
                like_Idea.Like = 0;
            }
            if(like_Idea.DisLike == 1)
            {
                like_Idea.DisLike= 0;
                like_Idea.Like = 1;

            }
            _context.Update(like_Idea);
            if (like_Idea.Like == 0)
            {
                _context.Like_Ideas.Remove(like_Idea);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> EditDisLike(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Like_Idea like_Idea = await _context.Like_Ideas.FirstOrDefaultAsync(li => li.IdeaId == id);
            if (like_Idea.DisLike == 1)
            {
                like_Idea.Like = 0;
            }
            if (like_Idea.Like == 1)
            {
                like_Idea.Like = 0;
                like_Idea.DisLike = 1;

            }
            _context.Update(like_Idea);
            if (like_Idea.Like == 0)
            {
                _context.Like_Ideas.Remove(like_Idea);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // POST: Like_Idea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdeaId,Like,UserId")] Like_Idea like_Idea)
        {
            if (id != like_Idea.IdeaId)
            {
                return NotFound();
            }

            if (id== like_Idea.IdeaId)
            {
                try
                {
                    like_Idea.Like = 0;
                    _context.Update(like_Idea);
                    if (like_Idea.Like == 0)
                    {
                        _context.Like_Ideas.Remove(like_Idea);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Like_IdeaExists(like_Idea.UserId))
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
            ViewData["IdeaId"] = new SelectList(_context.ideas, "Id", "Id", like_Idea.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", like_Idea.UserId);
            return View(like_Idea);
        }
*/
        // GET: Like_Idea/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null )
            {
                return NotFound();
            }

            Like_Idea like_Idea = await _context.Like_Ideas.FirstOrDefaultAsync(li => li.IdeaId == id);
            if (like_Idea == null)
            {
                return NotFound();
            }

            return View(like_Idea);
        }

        // POST: Like_Idea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Like_Ideas == null)
            {
                return Problem("Entity set 'tcomp1Context.Like_Ideas'  is null.");
            }
            Like_Idea like_Idea = await _context.Like_Ideas.FirstOrDefaultAsync(li => li.IdeaId == id);
            if (like_Idea != null)
            {
                _context.Like_Ideas.Remove(like_Idea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Like_IdeaExists(string id)
        {
          return _context.Like_Ideas.Any(e => e.UserId == id);
        }
    }
}
