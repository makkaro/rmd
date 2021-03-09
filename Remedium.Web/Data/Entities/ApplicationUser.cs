using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Remedium.Web.Data.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        [InverseProperty(nameof(Article.Author))] public ICollection<Article> Articles { get; set; }
    }
}