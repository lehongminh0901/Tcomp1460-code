using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using tcomp1.Areas.Identity.Data;

namespace tcomp1.Models
{
    public class Idea
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Descripstion { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CloseTime { get; set; }
        
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile Img { get; set; }
        public string Fileurl { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        
        public bool Accep { get; set; }
        public bool Rules { get; set; }
        public string Incognito { get; set; }
        public int Like { get; set; }
        public int DisLike { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        
        public string UserId { get; set; }
        public tcomp1User tcomp1User { get; set; }
        public virtual ICollection<Like_Idea> Like_Ideas { get; set; }
        public string IdAdemic { get; set; }
        public Ademic Ademics { get; set; }
        public string IdCategory { get; set; }
        public Category Categorys { get; set; }


    }
}