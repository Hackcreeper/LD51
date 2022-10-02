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
        
        protected static readonly int Working = Animator.StringToHash("working");

        public override void Interact(Player.Player player)
        {
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (!ingredient)
            {
                return;
            }
            
            HandleIngredient(player, ingredient);
        }

        protected void HandleIngredient(Player.Player player, Ingredient ingredient)
        {
            var recipe = recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            if (recipe == null)
            {
                return;
            }

            TmpPlayer = player;
            TmpOutput = recipe.output;
            
            var pickable = player.GetItemHolder().MoveItem(null, itemTarget.transform.position);
            PlacedItem = ((MonoBehaviour)pickable).transform;

            if (freezePlayer)
            {
                player.GetPlayerMovement().Freeze();
            }

            animator.SetBool(Working, true);
        }

        public virtual void OnAnimationFinish()
        {
            Destroy(PlacedItem.gameObject);
            TmpPlayer.GetItemHolder().PickIngredient(TmpOutput, true);
            
            if (freezePlayer)
            {
                TmpPlayer.GetPlayerMovement().UnFreeze();
            }

            TmpPlayer = null;
            TmpOutput = null;
        }
    }
}