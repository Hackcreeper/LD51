using System.Linq;
using Cooking;
using Cooking.Data;
using Feeding.Data;
using UnityEngine;

namespace Feeding
{
    public class MonsterFeeder : MonoBehaviour
    {
        public MealHappiness[] meals;

        private void OnTriggerEnter(Collider other)
        {
            var plate = other.GetComponent<Plate>();
            if (!plate)
            {
                return;
            }
            
            Debug.Log($"Feeding: {plate.GetMeal().label}");
            
            
            
            // Update happiness meters
            foreach (var meal in meals)
            {
                var happiness = meal.meal == plate.GetMeal()
                    ? meal.happiness - 1
                    : meal.happiness + 1;
             
                meal.happiness = Mathf.Clamp(happiness,0, 4);
            }
            
            // Remove plate and respawn it
            Destroy(other.gameObject);
            // TODO: Respawn
        }

        private MealHappiness GetMealHappinessByMeal(Meal meal)
        {
            return meals.First(m => m.meal == meal);
        }
    }
}