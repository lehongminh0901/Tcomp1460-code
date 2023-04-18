using System.ComponentModel.DataAnnotations.Schema;
using tcomp1.Areas.Identity.Data;

namespace tcomp1.Models
{
    public class Comment
    {
        public string? Id { get; set; }
        public string Descripstion { get; set; }
        public DateTime DateTime { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }
        public string Incognito { get; set; }
        public string IdeaId { get; set; }
        public Idea Idea { get; set; }
        public string UserId { get; set; }
        public tcomp1User tcomp1User { get; set; }
        public virtual ICollection<Like_Comment> Like_Comment { get; set; }
    }
}