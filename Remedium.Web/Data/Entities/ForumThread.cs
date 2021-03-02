using System;
using System.Collections.Generic;

namespace Remedium.Web.Data.Entities
{
    public sealed class ForumThread
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public ICollection<ForumPost> Posts { get; set; }
    }
}