using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using ICSharpCode.SharpZipLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using tcomp1.Areas.Identity.Data;
using tcomp1.Data;
using tcomp1.Models;

namespace tcomp1.Controllers
{
    
    [Authorize(Roles = "QA,Admin")]
    public class AdemicsController : Controller
    {
        [DataContract]
        public class DataPoint
        {
            public DataPoint(string label, double y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
        private readonly tcomp1Context _context;
        private readonly UserManager<tcomp1User> _userManager;

        public AdemicsController(tcomp1Context context, UserManager<tcomp1User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Ademics
        public async Task<IActionResult> Index()
        {
              return _context.Ademics != null ? 
                          View(await _context.Ademics.ToListAsync()) :
                          Problem("Entity set 'tcomp1Context.Ademics'  is null.");
        }
        public async Task<IActionResult> Chart()
        {
            var chart = _context.ideas.Include(i => i.Comments)
                .Include(i => i.Like_Ideas).ToList();
            var comment = _context.Comments.ToList();
            var like = _context.Like_Ideas.ToList();
            List<DataPoint> dataPoints = new List<DataPoint>();
            dataPoints.Add(new DataPoint("Ideas", chart.Count()));
            dataPoints.Add(new DataPoint("Comment", comment.Count()));
            dataPoints.Add(new DataPoint("Like", like.Count()));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        public async Task<IActionResult> Chartcategory()
        {
            var Categotys = _context.categories.FirstOrDefault(c => c.Name == "Business");
            var Category_Marketing = _context.categories.FirstOrDefault(c => c.Name == "Marketing");
            var Category_manager = _context.categories.FirstOrDefault(c => c.Name == "Manager");

            var Business = _context.ideas.Where(i => i.IdCategory == Categotys.Id);
            var Maketing = _context.ideas.Where(i => i.IdCategory == Category_Marketing.Id);
            var Manager = _context.ideas.Where(i => i.IdCategory == Category_manager.Id);
            List<DataPoint> dataPoints = new List<DataPoint>();
            dataPoints.Add(new DataPoint("Business", Business.Count()));
            dataPoints.Add(new DataPoint("Marketing", Maketing.Count()));
            dataPoints.Add(new DataPoint("Manager", Manager.Count()));
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        public async Task<IActionResult> Percentage_of_ideas()
        {
            var Categotys = _context.categories.FirstOrDefault(c => c.Name == "Business");
            var Category_Marketing = _context.categories.FirstOrDefault(c => c.Name == "Marketing");
            var Category_manager = _context.categories.FirstOrDefault(c => c.Name == "Manager");

            var Business = _context.ideas.Where(i => i.IdCategory == Categotys.Id);
            var Maketing = _context.ideas.Where(i => i.IdCategory == Category_Marketing.Id);
            var Manager = _context.ideas.Where(i => i.IdCategory == Category_manager.Id);
            var total = Business.Count() + Manager.Count()+Maketing.Count();
            var a = Business.Count();
            var a1 = a*100/total;
            var b = Maketing.Count();
            var b1 = b*100/total;
            var c = Manager.Count();
            var c1= 100-(a1+b1);
            List<DataPoint> dataPoints = new List<DataPoint>();
            dataPoints.Add(new DataPoint("Business", a1));
            dataPoints.Add(new DataPoint("Marketing", b1));
            dataPoints.Add(new DataPoint("Manager", c1));
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        public async Task<IActionResult> Number_of_contributors()
        {
            /*var idea = _context.ideas.Include(i => i.tcomp1User).DistinctBy(i => i.UserId);
            var comment = _context.Comments.Include(c => c.tcomp1User).DistinctBy(c => c.UserId);*/
            var Categotys = _context.categories.FirstOrDefault(c => c.Name == "Business");
            var Category_Marketing = _context.categories.FirstOrDefault(c => c.Name == "Marketing");
            var Category_manager = _context.categories.FirstOrDefault(c => c.Name == "Manager");

            var Business = _context.ideas.Where(i => i.IdCategory == Categotys.Id);
            var Maketing = _context.ideas.Where(i => i.IdCategory == Category_Marketing.Id);
            var Manager = _context.ideas.Where(i => i.IdCategory == Category_manager.Id);

            var Business1 = Business.GroupBy(i=>i.UserId);
            var Maketing1 = Maketing.GroupBy(i => i.UserId);
            var Manager1 = Manager.GroupBy(i => i.UserId);
            /* var Maketing = _context.ideas.Include(i => i.tcomp1User).Where(i => i.IdCategory == Category_Marketing.Id).DistinctBy(i => i.UserId);
             var Manager = _context.ideas.Include(i => i.tcomp1User).Where(i => i.IdCategory == Category_manager.Id).DistinctBy(i => i.UserId);*/



            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("Business", Business1.Count()));
            dataPoints.Add(new DataPoint("Maketing", Maketing1.Count()));
            dataPoints.Add(new DataPoint("Manager", Manager1.Count()));


            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }
        public async Task<IActionResult> Exception_reports()
        {
            var ideas = _context.ideas.Include(i => i.Comments);
            var commnent = _context.Comments.ToList();
            var comment1 = commnent.GroupBy(c=>c.IdeaId);
            var a = ideas.Count() - comment1.Count();
            var ideaI = _context.ideas.Where(i => i.Incognito == "Incognito");
            var commentI = _context.Comments.Where(c => c.Incognito == "Incognito");
            var b = ideaI.Count() + commentI.Count();
            List<DataPoint> dataPoints = new List<DataPoint>();
            dataPoints.Add(new DataPoint("Ideas without a comment", a));
            dataPoints.Add(new DataPoint("Anonymous ideas and comments", b));
            

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }


            /* public async Task<IActionResult> business()
             {
                 var idea = _context.ideas.Where(i => i.Category == "business");
                 return View(await idea.ToListAsync());
             }
             public async Task<IActionResult> marketing()
             {
                 var idea = _context.ideas.Where(i => i.Category == "marketing");
                 return View(await idea.ToListAsync());
             }
             public async Task<IActionResult> employee_manager()
             {
                 var idea = _context.ideas.Where(i => i.Category == "employee manager");
                 return View(await idea.ToListAsync());
             }*/
            public async Task<IActionResult> CSV()
        {
            var idea = _context.ideas.Include(i=>i.Categorys).ToList();
            var builder = new StringBuilder();
            builder.AppendLine("Id,Title,Descripstion,DateTime,ImgUrl,Fileurl,Category,UserId,Like,DisLike");
            foreach (var ideas in idea)
            {
                builder.AppendLine($"{ideas.Id},{ideas.Title},{ideas.Descripstion},{ideas.DateTime},{ideas.ImgUrl},{ideas.Fileurl},{ideas.Categorys.Name},{ideas.UserId},{ideas.Like},{ideas.DisLike}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "ideas.csv");
        }

        // GET: Ademics/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Ademics == null)
            {
                return NotFound();
            }

            var ademic = await _context.Ademics
                .FirstOrDefaultAsync(m => m.IdAdemic == id);
            var ademics = _context.Ademics.Include(a=>a.Ideas);
            var idea = _context.ideas.Where(i => i.DateTime > ademic.StartDate);
            ViewBag.Name = ademic.Name;
            ViewBag.DateStart = ademic.StartDate;
            ViewBag.Enddate = ademic.Enddate;

            return View(await idea.ToListAsync());
        }

        // GET: Ademics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ademics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,StartDate,Enddate")] Ademic ademic)
        {
           
                tcomp1User user = await _userManager.GetUserAsync(HttpContext.User);
                var rand = Guid.NewGuid();
                var myRandomNumber = rand.ToString();
                ademic.IdAdemic = myRandomNumber;
                ademic.UserId = user.Id;
                _context.Add(ademic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Index));
        }

        // GET: Ademics/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Ademics == null)
            {
                return NotFound();
            }

            var ademic = await _context.Ademics.FindAsync(id);
            if (ademic == null)
            {
                return NotFound();
            }
            return View(ademic);
        }

        // POST: Ademics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdAdemic,Name,StartDate,Enddate,UserId")] Ademic ademic)
        {
            if (id != ademic.IdAdemic)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ademic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdemicExists(ademic.IdAdemic))
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
            return View(ademic);
        }

        // GET: Ademics/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Ademics == null)
            {
                return NotFound();
            }

            var ademic = await _context.Ademics
                .FirstOrDefaultAsync(m => m.IdAdemic == id);
            if (ademic == null)
            {
                return NotFound();
            }

            return View(ademic);
        }

        // POST: Ademics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Ademics == null)
            {
                return Problem("Entity set 'tcomp1Context.Ademics'  is null.");
            }
            var ademic = await _context.Ademics.FindAsync(id);
            if (ademic != null)
            {
                _context.Ademics.Remove(ademic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdemicExists(string id)
        {
          return (_context.Ademics?.Any(e => e.IdAdemic == id)).GetValueOrDefault();
        }
    }
}
