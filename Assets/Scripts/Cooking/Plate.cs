﻿using System.Collections.Generic;
using System.Linq;
using Cooking.Data;
using UnityEngine;

namespace Cooking
{
    public class Plate : Interactable, IPickable
    {
        public Meal[] meals;
        public DeskSlot initialSlot;

        private readonly List<Ingredient> _ingredients = new();
        private readonly List<IPickable> _pickableIngredients = new();
        private PickableMeal _placedMeal;

        public override void Interact(Player.Player player)
        {
            if (_placedMeal != null && _placedMeal.IsComplete())
            {
                player.GetItemHolder().PickPlate(this);
                IsInteractable = false;
                initialSlot.FreeSlot();
                return;
            }
            
            
            var playerIngredient = player.GetItemHolder().GetCurrentIngredient();
            if (!playerIngredient || _ingredients.Contains(playerIngredient) || (_placedMeal != null && _placedMeal.IsComplete()))
            {
                return;
            }

            var allIngredients = new List<Ingredient>();
            allIngredients.AddRange(_ingredients);
            allIngredients.Add(playerIngredient);

            var possibleMeals = (from meal in meals let worked = allIngredients.All(ingredient => meal.ingredients.Contains(ingredient)) where worked select meal).ToList();

            switch (possibleMeals.Count)
            {
                case 0:
                    Debug.Log("No matching meal found.");
                    return;
                
                case 1:
                {
                    Debug.Log("Found only one meal. Checking for completion..");
                
                    if (!_placedMeal)
                    {
                        _pickableIngredients.ForEach(ingredient => Destroy(((PickableIngredient)ingredient).gameObject));
                        _pickableIngredients.Clear();
                        
                        var meal = Instantiate(possibleMeals.First().platePrefab, transform);
                        _placedMeal = meal.GetComponent<PickableMeal>();
                    }

                    _ingredients.Add(playerIngredient);
                    _placedMeal.SetIngredients(_ingredients.ToArray());
                    player.GetItemHolder().RemoveCurrent();

                    if (_placedMeal.IsComplete())
                    {
                        Debug.Log("Meal finished. Someone needs to take it");
                    }
                    
                    return;
                }

                default:
                {
                    Debug.Log($"Found {possibleMeals.Count} possible meals.");
                    _pickableIngredients.Add(player.GetItemHolder().MoveIngredient(transform, Vector3.zero));
                    _ingredients.Add(playerIngredient);
                    return;
                }
            }
        }

        public Meal GetMeal() => _placedMeal? _placedMeal.meal : null;

        public PickableMeal GetPickableMeal() => _placedMeal;
    }
}