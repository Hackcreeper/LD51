using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class IngredientChest : Interactable
    {
        public Ingredient ingredient;
        
        public override void Interact(Player.Player player)
        {
            Debug.Log($"Picking up: {ingredient.label}");
            player.GetIngredientHolder().Pick(ingredient);
        }
    }
}