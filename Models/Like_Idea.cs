using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tcomp1.Areas.Identity.Data;

namespace tcomp1.Models
{
    public class Like_Idea
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string IdSTT { get; set; }
        public string? IdeaId { get; set; }
        
        public Idea Idea { get; set; }
        
        public string UserId { get; set; }
        public tcomp1User tcomp1User { get; set; }
        
    }
}