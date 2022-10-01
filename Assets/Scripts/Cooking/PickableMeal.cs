using System.Linq;
using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class PickableMeal : MonoBehaviour
    {
        public Meal meal;
        public MealIngredient[] ingredients;
        public MealIngredientBakedMaterial[] bakedMaterials;

        private bool _isComplete;
        private bool _baked;

        public void SetIngredients(Ingredient[] existing)
        {
            foreach (var mealIngredient in ingredients)
            {
                foreach (var unityObject in mealIngredient.unityObjects)
                {
                    unityObject.gameObject.SetActive(existing.Contains(mealIngredient.ingredient));
                }
            }

            _isComplete = existing.Length == ingredients.Length;
        }

        public void Bake()
        {
            _baked = true;

            foreach (var bakedMaterial in bakedMaterials)
            {
                bakedMaterial.meshRenderer.material = bakedMaterial.material;
            }
        }

        public bool IsComplete() => _isComplete;

        public bool IsBaked() => _baked;

        public bool IsReadyToDeliver() => meal.needsBaking ? _baked : _isComplete;
    }
}