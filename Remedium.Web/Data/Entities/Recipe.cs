using System;
using System.Collections.Generic;
using Remedium.Web.Data.Types;

namespace Remedium.Web.Data.Entities
{
    public sealed class Recipe
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public RecipeType RecipeType { get; set; }
        public ICollection<IngredientQuantity> IngredientQuantities { get; set; }
    }
}