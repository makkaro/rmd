using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remedium.Web.Data.Entities 
{
    public sealed record Article
    {
        public Int32 Id { get; init; }
        public DateTime Timestamp { get; init; }
        public DateTime LastUpdateTimestamp { get; init; }
        public String AuthorId { get; init; }
        public String LastUpdateAuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]public ApplicationUser Author { get; init; }
        [ForeignKey(nameof(LastUpdateAuthorId))] public ApplicationUser LastUpdateAuthor { get; init; }
        [Required, MaxLength(64)] public String Title { get; init; }
        [Required, MaxLength(2048)] public String Introduction { get; init; }
        [Required, MaxLength(16384)] public String Content { get; init; }
    }
}