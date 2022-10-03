using System;
using Cooking.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Feeding.Data
{
    [Serializable]
    public class MealHappiness
    {
        public Meal meal;
        public Image smileyIcon;
        [Range(0, 4)] public int happiness = 4;
    }
}