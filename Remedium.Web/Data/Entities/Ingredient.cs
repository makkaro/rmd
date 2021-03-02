using System;
using Remedium.Web.Data.Types;

namespace Remedium.Web.Data.Entities
{
    public sealed class Ingredient
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public IngredientType Type { get; set; }
    }
}