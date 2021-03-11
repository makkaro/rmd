using System;
using System.ComponentModel.DataAnnotations;

namespace Remedium.Web.Data.Entities
{
    public sealed record Post
    {
        public Int32 Id { get; init; }
        public DateTime Timestamp { get; init; }
        public Int32 ThreadId { get; init; }
        public Thread Thread { get; init; }
        public String AuthorId { get; init; }
        public ApplicationUser Author { get; init; }
        [Required, MaxLength(8192)] public String Content { get; init; }
    }
}