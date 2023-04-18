using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tcomp1.Areas.Identity.Data;

namespace tcomp1.Models
{
    public class Ademic
    {
        [Key]
        public string IdAdemic { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Enddate { get; set; }
        public virtual ICollection<Idea> Ideas { get; set; }
        public string UserId { get; set; }
        public tcomp1User tcomp1User { get; set; }
        
    }
}