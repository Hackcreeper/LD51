using System.Collections.Generic;
using System.Linq;
using Cooking.Data;
using UnityEngine;
using Util;

namespace Cooking
{
    [RequireComponent(typeof(Yeeter))]
    public class Plate : Interactable, IPickable
    {
        public Meal[] meals;
        public DeskSlot initialSlot;
        public Meal startMeal;
        public Ingredient startIngredient;

        private readonly List<Ingredient> _ingredients = new();
        private readonly List<IPickable> _pickableIngredients = new();
        private PickableMeal _placedMeal;
        private Yeeter _yeeter;
        private bool _preparedForTheYeet;
        private bool _pickedUp;

        private void Awake()
        {
            _yeeter = GetComponent<Yeeter>();
        }

        private void Start()
        {
            if (startIngredient == null || startMeal == null)
            {
                return;
            }
            
            var meal = Instantiate(startMeal.platePrefab, transform);
            
            _placedMeal = meal.GetComponent<PickableMeal>();
            _ingredients.Add(startIngredient);
            _placedMeal.SetIngredients(_ingredients.ToArray());
        }

        public override void Interact(Player.Player player)
        {
            if (player.GetItemHolder().IsEmpty())
            {
                player.GetItemHolder().PickPlate(this);
                IsInteractable = false;
                initialSlot.FreeSlot();
                return;
            }
            
            if (_placedMeal != null && _placedMeal.IsComplete())
            {
                var worked = player.GetItemHolder().PickPlate(this);

                IsInteractable = false;
                initialSlot.FreeSlot();

                if (worked)
                {
                    return;
                }
                
                initialSlot.Interact(player);
                player.GetItemHolder().PickPlate(this);

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
                    _pickableIngredients.Add(player.GetItemHolder().MoveItem(transform, Vector3.zero));
                    _ingredients.Add(playerIngredient);
                    return;
                }
            }
        }

        public Meal GetMeal() => _placedMeal? _placedMeal.meal : null;

        public PickableMeal GetPickableMeal() => _placedMeal;

        public void Placed(Transform target)
        {
            if (target == null)
            {
                return;
            }
            
            IsInteractable = true;
            _pickedUp = false;
            
            var slot = target.GetComponent<DeskSlot>();
            if (!slot || slot.GetType() == typeof(YeetPlatform))
            {
                return;
            }

            initialSlot = slot;
            initialSlot.UnfreeSlot();
        }

        public void Clear()
        {
            _pickableIngredients.ForEach(ingredient => Destroy(((PickableIngredient)ingredient).gameObject));
            _pickableIngredients.Clear();
            
            _ingredients.Clear();

            if (_placedMeal)
            {
                Destroy(_placedMeal.gameObject);
                _placedMeal = null;
            }
        }

        public void Yeet(Vector3 target)
        {
            IsInteractable = false;
            _yeeter.StartTheYeet(target);
        }

        public bool IsEmpty()
        {
            return _ingredients.Count == 0;
        }

        public bool CanBuildMeal(Meal meal)
        {
            var possibleMeals = (from m in meals let worked = _ingredients.All(ingredient => m.ingredients.Contains(ingredient)) where worked select m).ToList();

            return possibleMeals.Contains(meal);
        }

        public void PrepareForYeet()
        {
            _preparedForTheYeet = true;
        }

        public bool IsPreparedForTheYeet() => _preparedForTheYeet;

        public void SetPickedUp(bool pickedUp)
        {
            _pickedUp = pickedUp;
        }

        public bool IsPickedUp() => _pickedUp;
    }
}