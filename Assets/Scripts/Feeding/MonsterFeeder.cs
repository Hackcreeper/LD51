using System.Linq;
using Ai;
using Cooking;
using Cooking.Data;
using Feeding.Data;
using Ui;
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
        public int ragePenaltyIfNoFood = 25;
        public GameObject platePrefab;
        public RectTransform rageMeterBar;
        public RectTransform deathMask;
        public int rageMeterMaxWidth = 310;
        public Color[] timerColors;
        public RectTransform timerNeedle;
        public Image timerBackground;
        public Flash flash;
        public Monster monster;
        public YeetBot yeetBot;
        public Bot bot1;
        public Bot bot2;

        private float _timeLeft = 10f;
        private bool _gotFed = false;
        private int _killed = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Bot>())
            {
                Destroy(other.gameObject);
                monster.StopSuck();
                return;
            }
            
            var plate = other.GetComponent<Plate>();
            if (!plate)
            {
                return;
            }

            Debug.Log($"Feeding: {plate.GetMeal().label}");
            _gotFed = true;

            monster.Crunch();

            // Determine the rage
            var eatenMeal = GetMealHappinessByMeal(plate.GetMeal());
            ModifyRageMeter(CalculateRageByHappiness(eatenMeal.happiness));

            // Update happiness meters
            foreach (var meal in meals)
            {
                var happiness = meal.meal == plate.GetMeal()
                    ? meal.happiness - 1
                    : meal.happiness + 1;

                meal.happiness = Mathf.Clamp(happiness, 0, 4);
                meal.smileyIcon.sprite = happinessIcons[meal.happiness];
            }

            // Remove plate and respawn it
            var newPlate = Instantiate(platePrefab, null);
            newPlate.transform.position = plate.initialSlot.itemTarget.position;
            plate.initialSlot.itemOnSlot = newPlate.transform;
            var plateComponent = newPlate.GetComponent<Plate>();
            plateComponent.initialSlot = plate.initialSlot;
            plateComponent.startIngredient = plate.startIngredient;
            plateComponent.startMeal = plate.startMeal;
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
            if (!GameState.Started)
            { 
                return;
            }
            
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

            if (_timeLeft <= 0f)
            {
                if (!_gotFed)
                {
                    Debug.Log("Time is up. Monster is still hungry");
                    _timeLeft = 10f;
                    ModifyRageMeter(-ragePenaltyIfNoFood);
                    return;
                }

                Debug.Log("Time is up. Monster is not hungry");
                _timeLeft = 10f;
                _gotFed = false;
            }
        }

        private void ModifyRageMeter(int amount)
        {
            rageMeter = Mathf.Clamp(rageMeter + amount, 0, 100);

            if (amount >= 0)
            {
                return;
            }
            
            flash.StartFlash();
            monster.Play();

            if (GetCurrentDeathLayer().killed <= _killed)
            {
                return;
            }
            
            monster.StartSuck();
            _killed = GetCurrentDeathLayer().killed;

            if (_killed == 1)
            {
                yeetBot.GetSucked(transform.position);
                return;
            }

            if (_killed == 2)
            {
                bot2.GetSucked(transform.position);
                return;
            }

            if (_killed == 3)
            {
                bot1.GetSucked(transform.position);
                return;
            }
            
            Debug.Log("GAME OVER");
        }

        private DeathLayer GetCurrentDeathLayer()
        {
            return deathLayers.Last(layer => rageMeter <= layer.rageBelow);
        }

        public float HowMuchTimeLeft() => _timeLeft;
    }
}