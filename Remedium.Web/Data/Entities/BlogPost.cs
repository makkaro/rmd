using System;

namespace Remedium.Web.Data.Entities
{
    public sealed class BlogPost
    {
        public Int32 Id { get; set; }
        public DateTime Timestamp { get; set; }
        public ApplicationUser Author { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
    }
}