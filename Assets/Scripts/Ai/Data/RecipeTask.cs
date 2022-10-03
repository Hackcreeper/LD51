using System;
using Cooking.Data;
using Cooking.Enum;

namespace Ai.Data
{
    [Serializable]
    public class RecipeTask
    {
        public Ingredient ingredient;
        public StationType station;
    }
}