using System;
using Cooking;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Ai
{
    [RequireComponent(typeof(Player.Player))]
    public class Bot : Interactable
    {
        public CookingStation assignedStation;
        public Transform navMeshTarget;
        
        private Player.Player _player;

        private void Awake()
        {
            _player = GetComponent<Player.Player>();
        }

        private void Start()
        {
            // assignedStation.AttachBot();
        }

        private void Update()
        {
            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.destination = navMeshTarget.position; 

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
            
            // ((BotMovement)_player.GetPlayerMovement()).RotateToStation();
            
            assignedStation.Interact(_player);
        }
    }
}