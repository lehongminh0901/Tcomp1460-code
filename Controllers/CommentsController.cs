using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tcomp1.Areas.Identity.Data;
using tcomp1.Data;
using tcomp1.Models;

namespace tcomp1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly tcomp1Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<tcomp1User> _userManager;

        public CommentsController(tcomp1Context context, IWebHostEnvironment hostEnvironment, UserManager<tcomp1User> userManager)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var tcomp1Context = _context.Comments.Include(c => c.Idea).Include(c => c.tcomp1User);
            return View(await tcomp1Context.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Idea)
                .Include(c => c.tcomp1User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["IdeaId"] = new SelectList(_context.ideas, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id,[Bind("Id,Descripstion,DateTime,Img,Incognito,IdeaId,UserId")] Comment comment)
        {
            if (comment.Img == null)
            {
                comment.ImgUrl = "null";
            }
            else
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(comment.Img.FileName);

                string extension = Path.GetExtension(comment.Img.FileName);
                comment.ImgUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await comment.Img.CopyToAsync(fileStream);
                }
            }
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();

            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            comment.Id = myRandomNumber;
            comment.UserId = user.Id;
            comment.DateTime = DateTime.Now;
            comment.IdeaId = id;
            comment.Like = 0;
            comment.DisLike = 0;

            _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Privacy", "Home",new {id=id});
            
           
        }
        public async Task<IActionResult> CreateLike(string id)
        {
            var comment = _context.Comments.FirstOrDefault(i => i.Id == id);
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var like_comment = _context.Like_Comments.FirstOrDefault(li => li.UserId == thisUserId && li.CommentId == comment.Id);
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();


            if (like_comment == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Like_Comment like_Comment = new Like_Comment();
                        like_Comment.Id = myRandomNumber;
                        like_Comment.IdSTT = "Like";
                        like_Comment.UserId = thisUserId;
                        like_Comment.CommentId = id;
                        _context.Add(like_Comment);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error" + ex);

                    }
                }
                comment.Like = comment.Like + 1;
                _context.Update(comment);
                await _context.SaveChangesAsync();

            }
            if (like_comment != null)
            {

                if (like_comment.IdSTT == "Like")
                {
                    comment.Like = comment.Like - 1;
                    _context.Like_Comments.Remove((Like_Comment)like_comment);
                    await _context.SaveChangesAsync();
                }
                if (like_comment.IdSTT == "DisLike")
                {
                    comment.DisLike = comment.DisLike - 1;
                    comment.Like = comment.Like + 1;
                    like_comment.IdSTT = "Like";
                    _context.Like_Comments.Update((Like_Comment)like_comment);
                    await _context.SaveChangesAsync();
                }

                _context.Update(comment);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> CreateDisLike(string id)
        {
            var comment = _context.Comments.FirstOrDefault(i => i.Id == id);
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var like_comment = _context.Like_Comments.FirstOrDefault(li => li.UserId == thisUserId && li.CommentId == comment.Id);
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();


            if (like_comment == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Like_Comment like_Comment = new Like_Comment();
                        like_Comment.Id = myRandomNumber;
                        like_Comment.IdSTT = "DisLike";
                        like_Comment.UserId = thisUserId;
                        like_Comment.CommentId = id;
                        _context.Add(like_Comment);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error" + ex);

                    }
                }
                comment.DisLike = comment.DisLike + 1;
                _context.Update(comment);
                await _context.SaveChangesAsync();

            }
            if (like_comment != null)
            {

                if (like_comment.IdSTT == "DisLike")
                {
                    comment.DisLike = comment.DisLike - 1;
                    _context.Like_Comments.Remove((Like_Comment)like_comment);
                    await _context.SaveChangesAsync();
                }
                if (like_comment.IdSTT == "Like")
                {
                    comment.DisLike = comment.DisLike + 1;
                    comment.Like = comment.Like - 1;
                    like_comment.IdSTT = "DisLike";
                    _context.Like_Comments.Update((Like_Comment)like_comment);
                    await _context.SaveChangesAsync();
                }

                _context.Update(comment);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index", "Home");

        }


        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["IdeaId"] = new SelectList(_context.ideas, "Id", "Id", comment.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Descripstion,DateTime,Like,Dislike,ImgUrl,Incognito,IdeaId,UserId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["IdeaId"] = new SelectList(_context.ideas, "Id", "Id", comment.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comment.UserId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Idea)
                .Include(c => c.tcomp1User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'tcomp1Context.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(string id)
        {
          return _context.Comments.Any(e => e.Id == id);
        }
    }
}
