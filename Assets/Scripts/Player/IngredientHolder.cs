using Cooking;
using Cooking.Data;
using UnityEngine;

namespace Player
{
    public class IngredientHolder : MonoBehaviour
    {
        private PickableIngredient _currentIngredient;

        public void Pick(Ingredient ingredient)
        {
            if (_currentIngredient != null)
            {
                Destroy(_currentIngredient.gameObject);
                _currentIngredient = null;
            }
            
            var instance = Instantiate(
                ingredient.pickupPrefab,
                transform
            );

            instance.transform.localPosition = new Vector3(0, 2f, 0);
            _currentIngredient = instance.GetComponent<PickableIngredient>();
        }

        public Ingredient GetCurrentIngredient() => _currentIngredient.ingredient ? _currentIngredient.ingredient : null;
    }
}