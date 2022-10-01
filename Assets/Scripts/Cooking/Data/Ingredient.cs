using UnityEngine;

namespace Cooking.Data
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "Custom/Ingredient", order = 0)]
    public class Ingredient : ScriptableObject
    {
        public string label;
        public GameObject pickupPrefab;
    }
}