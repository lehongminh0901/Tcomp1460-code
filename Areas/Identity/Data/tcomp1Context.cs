using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tcomp1.Areas.Identity.Data;
using tcomp1.Models;

namespace tcomp1.Data;

public class tcomp1Context : IdentityDbContext<tcomp1User>
{
    public tcomp1Context(DbContextOptions<tcomp1Context> options)
        : base(options) { }


    public DbSet<Idea> ideas { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like_Idea> Like_Ideas { get; set; }
    public DbSet<Like_Comment> Like_Comments { get; set; }
    public DbSet<Ademic> Ademics { get; set; }
    public DbSet<Category> categories { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    builder.Entity<tcomp1User>()
           .HasMany<Idea>(au => au.Idea)
           .WithOne(i => i.tcomp1User)
           .HasForeignKey(au => au.UserId);
    builder.Entity<Comment>()
        .HasOne<tcomp1User>(c => c.tcomp1User)
        .WithMany(au => au.Comment)
        .HasForeignKey(c => c.UserId);
    builder.Entity<Comment>()
        .HasOne<Idea>(i => i.Idea)
        .WithMany(c => c.Comments)
        .HasForeignKey(c => c.IdeaId);
    builder.Entity<Idea>()
        .HasOne<tcomp1User>(i => i.tcomp1User)
        .WithMany(au => au.Idea)
        .HasForeignKey(i => i.UserId);
    
    builder.Entity<Like_Idea>()
        .HasOne<Idea>(li => li.Idea)
        .WithMany(i => i.Like_Ideas)
        .HasForeignKey(li => li.IdeaId);
    builder.Entity<Like_Idea>()
        .HasOne<tcomp1User>(li => li.tcomp1User)
        .WithMany(au => au.Like_Idea)
        .HasForeignKey(li => li.UserId);
    //

    builder.Entity<Like_Comment>()
        .HasOne<Comment>(lc => lc.Comment)
        .WithMany(c => c.Like_Comment)
        .HasForeignKey(li => li.CommentId);
    builder.Entity<Like_Comment>()
        .HasOne<tcomp1User>(lc => lc.tcomp1User)
        .WithMany(au => au.Like_Comment)
        .HasForeignKey(li => li.UserId);
    builder.Entity<Idea>()
            .HasOne<Category>(i=>i.Categorys)
            .WithMany(c=>c.Ideas)
            .HasForeignKey(i=>i.IdCategory);
        builder.Entity<Idea>()
            .HasOne<Ademic>(i => i.Ademics)
            .WithMany(a => a.Ideas)
            .HasForeignKey(i => i.IdAdemic);
/*    builder.Entity<Ademic>()
                .HasMany<Idea>(a => a.Ideas)
                .WithMany(i => i.Ademics)
                ;*/
    // Customize the ASP.NET Identity model and override the defaults if needed.
    // For example, you can rename the ASP.NET Identity table names and more.
    // Add your customizations after calling base.OnModelCreating(builder);
    }
   

    
}
