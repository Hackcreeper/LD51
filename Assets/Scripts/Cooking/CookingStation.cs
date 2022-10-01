using System.Linq;
using Cooking.Data;

namespace Cooking
{
    public class CookingStation : Interactable
    {
        public Recipe[] recipes;
        
        public override void Interact(Player.Player player)
        {
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (!ingredient)
            {
                return;
            }
            
            HandleIngredient(player, ingredient);
        }

        private void HandleIngredient(Player.Player player, Ingredient ingredient)
        {
            var recipe = recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            if (recipe == null)
            {
                return;
            }

            player.GetItemHolder().PickIngredient(recipe.output);
        }
    }
}