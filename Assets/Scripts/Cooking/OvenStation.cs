using System.Linq;
using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class OvenStation : CookingStation
    {
        private PickableMeal _cookingMeal;
        
        public override void Interact(Player.Player player)
        {
            if (IsWorking)
            {
                return;
            }
            
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            var plate = player.GetItemHolder().GetCurrentPlate();

            if (ingredient == null && plate == null)
            {
                if (TmpOutput == null && _cookingMeal == null)
                {
                    return;
                }

                if (TmpOutput)
                {
                    player.GetItemHolder().PickIngredient(TmpOutput);
                    Destroy(PlacedItem.gameObject);
                    TmpOutput = null;
                    return;
                }

                if (_cookingMeal)
                {
                    player.GetItemHolder().PickPlate(PlacedItem.GetComponent<Plate>(), true);
                    _cookingMeal = null;
                    return;
                }

                return;
            }
            
            if (ingredient)
            {
                HandleIngredient(player, ingredient);
                return;
            }

            if (!plate.GetMeal().needsBaking || plate.GetPickableMeal().IsBaked())
            {
                return;
            }

            var oldPlate = PlacedItem ? PlacedItem.GetComponent<Plate>() : null;
            var oldOutput = TmpOutput;
            var oldPlacedItem = PlacedItem;
            
            TmpPlayer = player;
            _cookingMeal = plate.GetPickableMeal();
            
            if (freezePlayer)
            {
                TmpPlayer.GetPlayerMovement().Freeze();
            }
            
            var pickable = player.GetItemHolder().MoveItem(null, itemTarget.transform.position);
            PlacedItem = ((MonoBehaviour)pickable).transform;

            if (oldPlate)
            {
                player.GetItemHolder().PickPlate(oldPlate);
                TmpOutput = null;
            }
            else if(oldOutput != null)
            {
                player.GetItemHolder().PickIngredient(oldOutput);
                Destroy(oldPlacedItem.gameObject);
                TmpOutput = null;
            }
            
            animator.SetBool(Working, true);
            IsWorking = true;
        }

        protected override void HandleIngredient(Player.Player player, Ingredient ingredient)
        {
            if (_cookingMeal == null)
            {
                base.HandleIngredient(player, ingredient);
                return;
            }

            var recipe = recipeBook.recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            if (recipe == null)
            {
                return;
            }
            
            var oldPlate = PlacedItem.GetComponent<Plate>();
            
            TmpPlayer = player;
            TmpOutput = recipe.output;
            
            var pickable = player.GetItemHolder().MoveItem(null, itemTarget.transform.position);
            PlacedItem = ((MonoBehaviour)pickable).transform;

            player.GetItemHolder().PickPlate(oldPlate);
            _cookingMeal = null;
            
            if (freezePlayer)
            {
                player.GetPlayerMovement().Freeze();
            }

            animator.SetBool(Working, true);
            IsWorking = true;
        }

        public override void OnAnimationFinish()
        {
            if (TmpOutput != null)
            {
                base.OnAnimationFinish();
                return;
            }

            IsWorking = false;

            _cookingMeal.Bake();

            if (freezePlayer)
            {
                TmpPlayer.GetItemHolder().PickPlate(PlacedItem.GetComponent<Plate>(), true);
                TmpPlayer.GetPlayerMovement().UnFreeze();
                _cookingMeal = null;
                TmpPlayer = null;

                return;
            }

            TmpPlayer = null;
        }
    }
}