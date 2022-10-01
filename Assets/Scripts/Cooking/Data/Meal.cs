using UnityEngine;

namespace Cooking.Data
{
    [CreateAssetMenu(fileName = "Meal", menuName = "Custom/Meal", order = 1)]
    public class Meal : ScriptableObject
    {
        public string label;
        public Ingredient[] ingredients;
        public GameObject platePrefab;
    }
}