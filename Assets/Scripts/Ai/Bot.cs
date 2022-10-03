using System;
using Cooking;
using Player;
using UnityEngine;

namespace Ai
{
    [RequireComponent(typeof(Player.Player))]
    public class Bot : Interactable
    {
        public CookingStation assignedStation;
        
        private Player.Player _player;

        private void Awake()
        {
            _player = GetComponent<Player.Player>();
        }

        private void Start()
        {
            assignedStation.AttachBot();
        }

        public override void Interact(Player.Player player)
        {
            var ingredient = player.GetItemHolder().GetCurrentIngredient();
            if (!ingredient)
            {
                return;
            }

            if (assignedStation.GetRecipeByInput(ingredient) == null)
            {
                return;
            }
            
            _player.GetItemHolder().PickIngredient(player.GetItemHolder().GetCurrentIngredient());
            player.GetItemHolder().RemoveCurrent();
            
            ((BotMovement)_player.GetPlayerMovement()).RotateToStation();
            
            assignedStation.Interact(_player);
        }
    }
}