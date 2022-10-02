using System;
using Cooking.Data;
using UnityEngine;

namespace Feeding.Data
{
    [Serializable]
    public class MealHappiness
    {
        public Meal meal;
        [Range(0, 4)] public int happiness = 4;
    }
}