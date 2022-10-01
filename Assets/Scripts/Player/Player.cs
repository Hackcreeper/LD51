using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(IngredientHolder))]
    public class Player : MonoBehaviour
    {
        private IngredientHolder _ingredientHolder;

        private void Awake()
        {
            _ingredientHolder = GetComponent<IngredientHolder>();
        }

        public IngredientHolder GetIngredientHolder() => _ingredientHolder;
    }
}