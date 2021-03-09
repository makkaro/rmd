using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remedium.Web.Data.Entities 
{
    public sealed record Article
    {
        public Int32 Id { get; set; }
        
        [Required, MaxLength(64)] public String Title { get; set; }
        
        [Required, MaxLength(2048)] public String Introduction { get; set; }
        
        [Required, MaxLength(8192)] public String Content { get; set; }
        
        public DateTime Timestamp { get; set; }
        
        public DateTime LastUpdateTimestamp { get; set; }
        
        public String AuthorId { get; set; }
        
        [ForeignKey(nameof(AuthorId))]public ApplicationUser Author { get; set; }
        
        public String LastUpdateAuthorId { get; set; }
        
        [ForeignKey(nameof(LastUpdateAuthorId))] public ApplicationUser LastUpdateAuthor { get; set; }
    }
}