using System;
using System.Linq;
using Cooking.Data;
using Cooking.Enum;
using Ui;
using UnityEngine;

namespace Cooking
{
    [RequireComponent(typeof(AudioSource))]
    public class CookingStation : Interactable
    {
        public RecipeBook recipeBook;
        public Animator animator;
        public bool freezePlayer;
        public Transform itemTarget;
        public StationType type;
        public Transform standingTarget;
        public Tutorial tutorial;

        protected Player.Player TmpPlayer;
        protected Ingredient TmpOutput;
        protected Transform PlacedItem;
        protected bool IsWorking;
        
        protected static readonly int Working = Animator.StringToHash("working");

        private bool _blockedByBot;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

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
            var recipe = GetRecipeByInput(ingredient);
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

            if (recipe.replacementPrefab)
            {
                var replacement = Instantiate(recipe.replacementPrefab, PlacedItem.parent);
                replacement.transform.localPosition = PlacedItem.localPosition;
                replacement.transform.localScale = PlacedItem.localScale;
                replacement.transform.localRotation = PlacedItem.localRotation;
                
                Destroy(PlacedItem.gameObject);
                PlacedItem = replacement.transform;
            }

            if (oldOutput != null)
            {
                player.GetItemHolder().PickIngredient(oldOutput);
                Destroy(oldPlacedItem.gameObject);
            }
            
            if (freezePlayer)
            {
                player.GetPlayerMovement().Freeze();
            }

            if (recipe.audioClip)
            {
                _audioSource.clip = recipe.audioClip;
                _audioSource.Play();
            }
            
            animator.SetBool(Working, true);
            IsWorking = true;
        }

        public Recipe GetRecipeByInput(Ingredient ingredient)
        {
            var recipe = recipeBook.recipes.FirstOrDefault(recipe => recipe.input == ingredient);
            return recipe;
        }

        public virtual void OnAnimationFinish()
        {
            IsWorking = false;
            
            if (freezePlayer)
            {
                Destroy(PlacedItem.gameObject);
                TmpPlayer.GetItemHolder().PickIngredient(TmpOutput, true);
                TmpPlayer.GetPlayerMovement().UnFreeze();
                TmpPlayer.InformStationReady();
                TmpOutput = null;
                TmpPlayer = null;

                return;
            }
            
            var instance = Instantiate(TmpOutput.pickupPrefab);
            instance.transform.position = PlacedItem.position;
            instance.transform.localScale = PlacedItem.localScale;
            instance.transform.rotation = PlacedItem.rotation;
            
            Destroy(PlacedItem.gameObject);
            PlacedItem = instance.transform;
            
            TmpPlayer.InformStationReady();
            TmpPlayer = null;
        }

        public void BlockByBot()
        {
            _blockedByBot = true;
        }

        public void UnblockByBot()
        {
            _blockedByBot = false;
        }

        public bool IsBlockedByBot() => _blockedByBot;
    }
}