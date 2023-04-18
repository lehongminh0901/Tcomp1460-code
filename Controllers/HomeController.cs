using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using tcomp1.Data;
using tcomp1.Models;
using PagedList.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using DocumentFormat.OpenXml.InkML;

namespace tcomp1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly tcomp1Context _context;

        public HomeController(ILogger<HomeController> logger, tcomp1Context context)
        {
            _logger = logger;
            _context = context;
            

        }

        public async Task<IActionResult> Index(int id)
        {
            int pageSize = 5;
            int numberOfRecords = await _context.ideas.CountAsync();     //Count SQL
            int numberOfPages = (int)Math.Ceiling((double)numberOfRecords / pageSize);
            ViewBag.numberOfPages = numberOfPages;
            ViewBag.currentPage = id;
            var category = _context.categories.ToList();
            ViewBag.category = category;
            /*ViewData["category"] = new SelectList(_context.categories, "Id", "Name");*/

            var like_idea = _context.Like_Ideas.Where(li => li.IdeaId == li.Idea.Id);
            var tcomp1Context = _context.ideas
                .Include(i => i.tcomp1User)
                .Include(i => i.Comments)
                .Include(i => i.Like_Ideas)
                .Where(i => i.Accep == true );
            

            return View(await tcomp1Context.Skip(id * pageSize).Take(pageSize).ToListAsync());
        }
        public async Task<IActionResult> Category(string id )
        {
            var category = _context.ideas
                .Include(i => i.tcomp1User)
                .Include(i => i.Comments)
                .Include(i => i.Like_Ideas)
                .Where(i=>i.IdCategory==id && i.Accep==true )
                .OrderByDescending(i=>i.Like);
            var categorys = _context.categories.ToList();
            ViewBag.categorys = categorys;
            return View(await category.ToListAsync());
        }



        public async Task<IActionResult> Privacy(string id)
        {
            var commnent =  _context.Comments.Include(c => c.Idea).Include(c => c.tcomp1User).Where(c => c.IdeaId == id);
            ViewBag.comment = commnent;
            var idea = _context.ideas.Include(i => i.tcomp1User)
                .Include(i => i.Comments)
                .FirstOrDefault(i=>i.Id==id);
            ViewBag.idea = idea;
            var c = _context.Comments.Where(c=>c.IdeaId==id);
            
                ViewBag.c = c;
            
            
            return View(await commnent.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}