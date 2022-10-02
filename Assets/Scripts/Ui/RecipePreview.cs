using Ui.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui
{
    public class RecipePreview : MonoBehaviour
    {
        public MealRecipeUi[] recipes;

        private int _active = 0;

        public void OnNextRecipe(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            
            recipes[_active].blockerElement.SetActive(false);
            recipes[_active].recipeListElement.SetActive(false);

            _active++;
            if (_active >= recipes.Length)
            {
                _active = 0;
            }
            
            recipes[_active].blockerElement.SetActive(true);
            recipes[_active].recipeListElement.SetActive(true);
        }

        public void OnPreviousRecipe(InputAction.CallbackContext context)
        {
            if (!context.performed)
            {
                return;
            }
            
            recipes[_active].blockerElement.SetActive(false);
            recipes[_active].recipeListElement.SetActive(false);

            _active--;
            if (_active < 0)
            {
                _active = recipes.Length - 1;
            }
            
            recipes[_active].blockerElement.SetActive(true);
            recipes[_active].recipeListElement.SetActive(true);
        }
    }
}