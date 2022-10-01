namespace Cooking
{
    public class OvenStation : CookingStation
    {
        public override void Interact(Player.Player player)
        {
            base.Interact(player);
            
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (ingredient)
            {
                return;
            }

            var plate = player.GetItemHolder().GetCurrentPlate();
            if (!plate.GetMeal().needsBaking || plate.GetPickableMeal().IsBaked())
            {
                return;
            }
            
            plate.GetPickableMeal().Bake();
        }
    }
}