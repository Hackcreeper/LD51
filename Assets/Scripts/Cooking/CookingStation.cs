using System.Linq;
using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class CookingStation : Interactable
    {
        public Recipe[] recipes;
        public Animator animator;
        public bool freezePlayer;
        public Transform itemTarget;

        protected Player.Player TmpPlayer;
        protected Ingredient TmpOutput;
        protected Transform PlacedItem;
        protected bool IsWorking;
        
        protected static readonly int Working = Animator.StringToHash("working");

        public override void Interact(Player.Player player)
        {
            if (IsWorking)
            {
                return;
            }
            
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (!ingredient)
            {
                if (TmpOutput == null)
                {
                    return;
                }
                
                player.GetItemHolder().PickIngredient(TmpOutput);
                Destroy(PlacedItem.gameObject);
                TmpOutput = null;

                return;
            }
            
            HandleIngredient(player, ingredient);
        }

        protected virtual void HandleIngredient(Player.Player player, Ingredient ingredient)
        {
            var recipe = recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            if (recipe == null)
            {
                return;
            }

            var oldOutput = TmpOutput;
            var oldPlacedItem = PlacedItem;
            
            TmpPlayer = player;
            TmpOutput = recipe.output;
            
            var pickable = player.GetItemHolder().MoveItem(null, itemTarget.transform.position);
            PlacedItem = ((MonoBehaviour)pickable).transform;

            if (oldOutput != null)
            {
                player.GetItemHolder().PickIngredient(oldOutput);
                Destroy(oldPlacedItem.gameObject);
            }
            
            if (freezePlayer)
            {
                player.GetPlayerMovement().Freeze();
            }

            animator.SetBool(Working, true);
            IsWorking = true;
        }

        public virtual void OnAnimationFinish()
        {
            IsWorking = false;
            
            if (freezePlayer)
            {
                Destroy(PlacedItem.gameObject);
                TmpPlayer.GetItemHolder().PickIngredient(TmpOutput, true);
                TmpPlayer.GetPlayerMovement().UnFreeze();
                TmpOutput = null;
                TmpPlayer = null;

                return;
            }
            
            TmpPlayer = null;
            
            var instance = Instantiate(TmpOutput.pickupPrefab);
            instance.transform.position = PlacedItem.position;
            instance.transform.localScale = PlacedItem.localScale;
            instance.transform.rotation = PlacedItem.rotation;
            
            Destroy(PlacedItem.gameObject);
            PlacedItem = instance.transform;
        }
    }
}