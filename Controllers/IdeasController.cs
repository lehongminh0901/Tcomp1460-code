using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tcomp1.Areas.Identity.Data;
using tcomp1.Data;
using tcomp1.Models;
using Ionic.Zip;
using ZipFile = Ionic.Zip.ZipFile;
using ZipOutputStream = ICSharpCode.SharpZipLib.Zip.ZipOutputStream;
using Microsoft.Extensions.FileProviders;
using ZipEntry = Ionic.Zip.ZipEntry;
using Org.BouncyCastle.Crypto;

namespace tcomp1.Controllers
{
    [Authorize(Roles = "QA,User,Admin")]
    public class IdeasController : Controller
    {
        private readonly tcomp1Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<tcomp1User> _userManager;
        private readonly IEmailSender _emailSender;

        public IdeasController(tcomp1Context context, IWebHostEnvironment hostEnvironment, UserManager<tcomp1User> userManager, IEmailSender emailSender)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Ideas
        [Authorize(Roles = "QA,User,Admin")]
        public async Task<IActionResult> Index()
        {
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            
            var tcomp1Context = _context.ideas.Where(i => i.UserId==user.Id);
            return View(tcomp1Context);
        }

        // GET: Ideas/Details/5
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

        // GET: Ideas/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["IdAdemic"] = new SelectList(_context.Ademics, "IdAdemic", "Name");
            ViewData["IdCategory"] = new SelectList(_context.categories, "Id", "Name");

            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Descripstion,DateTime,Img,Accep,Incognito,File,Rules,CloseTime,IdAdemic,IdCategory")] Idea idea)
        {
            if (idea.Img == null)
            {
                idea.ImgUrl = "null";
            }
            else
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(idea.Img.FileName);

                string extension = Path.GetExtension(idea.Img.FileName);
                idea.ImgUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await idea.Img.CopyToAsync(fileStream);
                }
            }
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();
            idea.DisLike = 0;
            idea.Like = 0;
            
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            idea.Id = myRandomNumber;
            idea.UserId = user.Id;
            
            
            if (idea.File == null)
            {
                idea.Fileurl = "null";
            }
            else
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                string fileName = Path.GetFileNameWithoutExtension(idea.File.FileName);

                string extension = Path.GetExtension(idea.File.FileName);
                idea.Fileurl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/doc/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await idea.File.CopyToAsync(fileStream);
                    
                }
                Response.Clear();
                
                var Ziplink = $"{idea.Fileurl}.zip";
                var destinationPath = Path.Combine(wwwRootPath + "/doc/", Ziplink);

                using (ZipFile zip = new ZipFile())
                { 
                    zip.AddFile(path);
                    zip.Save(destinationPath);
                }
            }

           
            idea.DateTime= DateTime.Now;
            idea.Accep = false;
            var ademic = _context.Ademics.Find(idea.IdAdemic);
            if (ademic.StartDate < idea.DateTime && idea.DateTime < ademic.Enddate)
            {
                _context.Add(idea);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.mess = "quá thời hạn";
                return View(idea);
            }
            await _emailSender.SendEmailAsync(user.Email, "Idea just created",
                       idea.Descripstion);
            return RedirectToAction(nameof(Index));
            ViewData["IdCategory"] = new SelectList(_context.Set<Category>(), "Id", "Name", idea.IdCategory);
            ViewData["IdAdemic"] = new SelectList(_context.Set<Ademic>(), "IdAdemic", "Name", idea.IdAdemic);
        }
        public async Task<IActionResult> CreateLike(string id)
        {
            var ideas =  _context.ideas.FirstOrDefault(i=>i.Id==id);
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var like_Idea = _context.Like_Ideas.FirstOrDefault(li => li.UserId == thisUserId && li.IdeaId == ideas.Id);
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();


            if (like_Idea == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Like_Idea like_Idea1 = new Like_Idea();
                        like_Idea1.Id = myRandomNumber;
                        like_Idea1.IdSTT = "Like";
                        like_Idea1.UserId = thisUserId;
                        like_Idea1.IdeaId = id;
                        _context.Add(like_Idea1);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error" + ex);

                    }
                }
                ideas.Like = ideas.Like + 1;
                _context.Update(ideas);
                await _context.SaveChangesAsync();

            }
            if(like_Idea != null)
            {
                
                    if (like_Idea.IdSTT == "Like")
                    {
                        ideas.Like = ideas.Like - 1;
                        _context.Like_Ideas.Remove((Like_Idea)like_Idea);
                        await _context.SaveChangesAsync();
                    }
                    if (like_Idea.IdSTT == "DisLike")
                    {
                        ideas.DisLike = ideas.DisLike - 1;
                        ideas.Like = ideas.Like + 1;
                        like_Idea.IdSTT = "Like";
                        _context.Like_Ideas.Update((Like_Idea)like_Idea);
                        await _context.SaveChangesAsync();
                    }
                
                _context.Update(ideas);
                await _context.SaveChangesAsync();

            }
            
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> CreateDisLike(string id)
        {
            var ideas = _context.ideas.FirstOrDefault(i => i.Id == id);
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var like_Idea = _context.Like_Ideas.FirstOrDefault(li => li.UserId == thisUserId && li.IdeaId == ideas.Id);
            var rand = Guid.NewGuid();
            var myRandomNumber = rand.ToString();


            if (like_Idea == null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Like_Idea like_Idea1 = new Like_Idea();
                        like_Idea1.Id = myRandomNumber;
                        like_Idea1.IdSTT = "DisLike";
                        like_Idea1.UserId = thisUserId;
                        like_Idea1.IdeaId = id;
                        _context.Add(like_Idea1);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error" + ex);

                    }
                }
                ideas.DisLike = ideas.DisLike + 1;
                _context.Update(ideas);
                await _context.SaveChangesAsync();

            }
            if (like_Idea != null)
            {
                
                    if (like_Idea.IdSTT == "DisLike")
                    {
                        ideas.DisLike = ideas.DisLike - 1;
                        _context.Like_Ideas.Remove((Like_Idea)like_Idea);
                        await _context.SaveChangesAsync();
                    }
                    if (like_Idea.IdSTT == "Like")
                    {
                        ideas.DisLike = ideas.DisLike + 1;
                        ideas.Like = ideas.Like - 1;
                        like_Idea.IdSTT = "DisLike";
                        _context.Like_Ideas.Update((Like_Idea)like_Idea);
                        await _context.SaveChangesAsync();
                    }
                
                _context.Update(ideas);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Index", "Home");

        }

        // GET: Ideas/Edit/5
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
            ViewData["DateTime"] = new SelectList(_context.Users, "DateTime", "DateTime", idea.DateTime);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Descripstion,DateTime,ImgUrl,Accep,Incognito,Fileurl,UserId,CloseTime,IdAdemic,IdCategory")] Idea idea)
        {
            if (id != idea.Id)
            {
                return NotFound();
            }
            tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
            var like_Idea =  _context.Like_Ideas.Where(li => li.UserId == user.Id && li.IdeaId==id);
            
            idea.UserId = user.Id;
            
            if (id==idea.Id)
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

        // GET: Ideas/Delete/5
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

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ideas == null)
            {
                return Problem("Entity set 'tcomp1Context.ideas'  is null.");
            }
            var idea = await _context.ideas.FindAsync(id);
            var comment = _context.Comments.Where(c=>c.IdeaId == idea.Id);
            var li = _context.Like_Ideas.Where(l => l.IdeaId == idea.Id);
            if (idea != null)
            {
                if (comment.Count() !=0)
                {
                    for (int i = 0; i < comment.Count(); i++)
                    {
                        var cm = _context.Comments.FirstOrDefault(c => c.IdeaId == id);
                        _context.Comments.Remove(cm);
                        await _context.SaveChangesAsync();
                    } 
                }
                if(li.Count() != 0)
                {
                    for (int i = 0; i < li.Count(); i++)
                    {
                        var like = _context.Like_Ideas.FirstOrDefault(l => l.IdeaId == id);
                        _context.Like_Ideas.Remove(like);
                        await _context.SaveChangesAsync();
                    }
                }
                _context.ideas.Remove(idea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(string id)
        {
          return _context.ideas.Any(e => e.Id == id);
        }
        public FileResult DownloadFile(string id)
        {

            string wwwRootPath = _hostEnvironment.WebRootPath;
            var Ziplink = $"{id}.zip";

            string path = Path.Combine(wwwRootPath + "/doc/", Ziplink);
                byte[] bytes = System.IO.File.ReadAllBytes(path);
            

            return File(bytes, "application/zip", Ziplink);
        }
    }
}
