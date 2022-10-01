using System;
using UnityEngine;

namespace Cooking.Data
{
    [Serializable]
    public class MealIngredient
    {
        public Ingredient ingredient;
        public Transform unityObject;
    }
}