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

        private Player.Player _player;
        private Ingredient _output;
        
        private static readonly int Working = Animator.StringToHash("working");

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

            _player = player;
            _output = recipe.output;
            
            player.GetItemHolder().RemoveCurrent();

            if (freezePlayer)
            {
                player.GetPlayerMovement().Freeze();
            }

            animator.SetBool(Working, true);
        }

        public void OnAnimationFinish()
        {
            _player.GetItemHolder().PickIngredient(_output, true);
            
            if (freezePlayer)
            {
                _player.GetPlayerMovement().UnFreeze();
            }

            _player = null;
            _output = null;
        }
    }
}