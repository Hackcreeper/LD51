﻿using Cooking.Data;
using Ui.Data;
using Ui.Enum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ui
{
    public class RecipePreview : MonoBehaviour
    {
        public MealRecipeUi[] recipes;
        public Color lockedColor = Color.yellow;

        private int _active = 0;
        private int _lastLocked = 0;

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

        public Meal GetCurrentMeal()
        {
            return recipes[_active].meal;
        }

        public void MarkLocked()
        {
            foreach (var image in recipes[_lastLocked].colorizeWhenLocked)
            {
                image.color = Color.white;
            }

            foreach (var image in recipes[_active].colorizeWhenLocked)
            {
                image.color = lockedColor;
            }

            _lastLocked = _active;
        }

        public void SetTaskStateOfLocked(int task, TaskState state)
        {
            recipes[_lastLocked].steps[task].UpdateState(state);
        }
    }
}