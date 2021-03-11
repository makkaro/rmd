using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Remedium.Web.Data.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public ICollection<Post> Posts { get; set; }
        public ICollection<Thread> Threads { get; set; }
        [InverseProperty(nameof(Article.Author))] public ICollection<Article> Articles { get; set; }
    }
}