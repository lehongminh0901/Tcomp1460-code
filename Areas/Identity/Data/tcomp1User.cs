using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using tcomp1.Models;

namespace tcomp1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the tcomp1User class
public class tcomp1User : IdentityUser
{
    public DateTime? DoB { get; internal set; }
    public string? Address { get; set; }
    public string? NameofUser { get; set; }
   /* public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }*/


    public virtual ICollection<Like_Idea> Like_Idea { get; set; }
    public virtual ICollection<Like_Comment> Like_Comment { get; set; }
    public virtual ICollection<Idea> Idea { get; set; }
    public virtual ICollection<Comment> Comment { get; set; }
    

}

