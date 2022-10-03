using UnityEngine;

namespace Cooking.Data
{
    [CreateAssetMenu(fileName = "RecipeBook", menuName = "Custom/Recipe Book", order = 2)]
    public class RecipeBook : ScriptableObject
    {
        public Recipe[] recipes;
    }
}