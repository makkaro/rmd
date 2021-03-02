using System;

namespace Remedium.Web.Data.Entities
{
    public sealed class IngredientQuantity
    {
        public Int32 Id { get; set; }
        public Int32 Order { get; set; }
        public String Quantity { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public Ingredient Ingredient { get; set; }
        public Recipe Recipe { get; set; }
    }
}