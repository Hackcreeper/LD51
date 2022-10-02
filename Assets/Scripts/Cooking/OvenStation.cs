namespace Cooking
{
    public class OvenStation : CookingStation
    {
        private PickableMeal _cookingMeal;
        
        public override void Interact(Player.Player player)
        {
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (ingredient)
            {
                HandleIngredient(player, ingredient);
                return;
            }

            var plate = player.GetItemHolder().GetCurrentPlate();
            if (!plate.GetMeal().needsBaking || plate.GetPickableMeal().IsBaked())
            {
                return;
            }

            TmpPlayer = player;
            _cookingMeal = plate.GetPickableMeal();
            
            if (freezePlayer)
            {
                TmpPlayer.GetPlayerMovement().Freeze();
            }
            
            animator.SetBool(Working, true);
        }

        public override void OnAnimationFinish()
        {
            if (TmpOutput != null)
            {
                base.OnAnimationFinish();
                return;
            }
            
            _cookingMeal.Bake();
            
            if (freezePlayer)
            {
                TmpPlayer.GetPlayerMovement().UnFreeze();
            }

            TmpPlayer = null;
            _cookingMeal = null;
        }
    }
}