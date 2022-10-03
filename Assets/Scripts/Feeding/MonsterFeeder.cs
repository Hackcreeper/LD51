using System.Linq;
using Cooking;
using Cooking.Data;
using Feeding.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Feeding
{
    public class MonsterFeeder : MonoBehaviour
    {
        public MealHappiness[] meals;
        public DeathLayer[] deathLayers;
        public Sprite[] happinessIcons;
        [Range(0, 100)] public int rageMeter = 100;
        public GameObject platePrefab;
        public RectTransform rageMeterBar;
        public RectTransform deathMask;
        public int rageMeterMaxWidth = 310;
        public Color[] timerColors;
        public RectTransform timerNeedle;
        public Image timerBackground;

        private float _timeLeft = 10f;
        private bool _gotFed = false;

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
                meal.smileyIcon.sprite = happinessIcons[meal.happiness];
            }
            
            // Remove plate and respawn it
            var newPlate = Instantiate(platePrefab);
            newPlate.transform.position = plate.initialSlot.transform.position;
            plate.initialSlot.itemOnSlot = newPlate.transform;
            newPlate.GetComponent<Plate>().initialSlot = plate.initialSlot;
            plate.initialSlot.UnfreeSlot();
            
            Destroy(other.gameObject);
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
        
        private void Update()
        {
            var size = rageMeterBar.sizeDelta;

            rageMeterBar.sizeDelta = new Vector2(
                Mathf.Lerp(size.x, rageMeterMaxWidth / 100f * rageMeter, 10f * Time.deltaTime),
                size.y
            );

            var layer = GetCurrentDeathLayer();
            var deathMaskSize = deathMask.sizeDelta;

            deathMask.sizeDelta = new Vector2(
                Mathf.Lerp(deathMaskSize.x, layer.deathMeterWidth, 10f * Time.deltaTime),
                deathMaskSize.y
            );

            _timeLeft -= Time.deltaTime;
            var index = 0;
            if (_timeLeft > 2f) index++;
            if (_timeLeft > 4f) index++;
            if (_timeLeft > 6f) index++;
            if (_timeLeft > 8f) index++;

            timerBackground.color = Color.Lerp(
                timerBackground.color,
                timerColors[index],
                10f * Time.deltaTime
            );

            var rotationAngle = 359f - (359f / 10f * (10f - _timeLeft));
            timerNeedle.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);
        }

        private DeathLayer GetCurrentDeathLayer()
        {
            return deathLayers.Last(layer => rageMeter <= layer.rageBelow);
        }
    }
}