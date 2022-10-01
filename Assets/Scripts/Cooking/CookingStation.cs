using System.Linq;
using Cooking.Data;

namespace Cooking
{
    public class CookingStation : Interactable
    {
        public Recipe[] recipes;
        
        public override void Interact(Player.Player player)
        {
            // Get the current ingredient the player is holding
            // Then check if there is a matching recipe
            // if so, replace the ingredient
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (!ingredient)
            {
                return;
            }

            var recipe = recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            if (recipe == null)
            {
                return;
            }
            
            player.GetItemHolder().PickIngredient(recipe.output);
        }
    }
}