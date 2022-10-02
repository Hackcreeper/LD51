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
        [Range(0, 100)] public int rageMeter = 100;

        private void OnTriggerEnter(Collider other)
        {
            var plate = other.GetComponent<Plate>();
            if (!plate)
            {
                return;
            }
            
            Debug.Log($"Feeding: {plate.GetMeal().label}");
            
            // Determine the rage
            var eatenMeal = GetMealHappinessByMeal(plate.GetMeal());
            rageMeter = Mathf.Clamp(rageMeter + CalculateRageByHappiness(eatenMeal.happiness), 0, 100);

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

        private static int CalculateRageByHappiness(int happiness)
        {
            return happiness switch
            {
                0 => -25, // Hate
                1 => -15, // Dislike
                2 => 0, // Meh
                3 => 15, // Okay
                4 => 25, // Happy
                _ => 0
            };
        }

        private MealHappiness GetMealHappinessByMeal(Meal meal)
        {
            return meals.First(m => m.meal == meal);
        }
    }
}