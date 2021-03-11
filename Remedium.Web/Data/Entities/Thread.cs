using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Remedium.Web.Data.Entities
{
    public sealed record Thread
    {
        public Int32 Id { get; init; }
        public DateTime LastUpdateTimestamp { get; set; }
        public String AuthorId { get; init; }
        public ApplicationUser Author { get; set; }
        public ICollection<Post> Posts { get; init; }
        [Required, MaxLength(64)] public String Title { get; init; }
    }
}