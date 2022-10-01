using System.Linq;
using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class PickableMeal : MonoBehaviour
    {
        public Meal meal;
        public MealIngredient[] ingredients;

        private bool _isComplete;

        public void SetIngredients(Ingredient[] existing)
        {
            foreach (var mealIngredient in ingredients)
            {
                mealIngredient.unityObject.gameObject.SetActive(
                    existing.Contains(mealIngredient.ingredient)                    
                );
            }

            _isComplete = existing.Length == ingredients.Length;
        }

        public bool IsComplete() => _isComplete;
    }
}