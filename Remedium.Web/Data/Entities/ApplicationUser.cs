using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Remedium.Web.Data.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public ICollection<Ingredient> Ingredients { get; set; }
        public String ForumFooter { get; set; }
    }
}