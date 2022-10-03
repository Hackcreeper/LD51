using System;
using Cooking.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Data
{
    [Serializable]
    public class MealRecipeUi
    {
        public Meal meal;
        public GameObject blockerElement;
        public GameObject recipeListElement;
        public Image[] colorizeWhenLocked;
        public TaskStep[] steps;
    }
}