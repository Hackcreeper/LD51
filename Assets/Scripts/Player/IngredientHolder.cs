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
            RemoveCurrent();
            
            var instance = Instantiate(
                ingredient.pickupPrefab,
                transform
            );

            instance.transform.localPosition = new Vector3(0, 2f, 0);
            _currentIngredient = instance.GetComponent<PickableIngredient>();
        }

        public Ingredient GetCurrentIngredient() => _currentIngredient.ingredient ? _currentIngredient.ingredient : null;

        public PickableIngredient MoveIngredient(Transform target, Vector3 offset)
        {
            var ingredient = _currentIngredient;
            
            if (_currentIngredient)
            {
                _currentIngredient.PlaceOn(target, offset);
            }

            _currentIngredient = null;

            return ingredient;
        }

        public void RemoveCurrent()
        {
            if (_currentIngredient == null)
            {
                return;
            }
            
            Destroy(_currentIngredient.gameObject);
            _currentIngredient = null;
        }
    }
}