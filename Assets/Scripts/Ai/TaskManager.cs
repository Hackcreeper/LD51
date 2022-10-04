using System.Collections.Generic;
using System.Linq;
using Ai.Data;
using Cooking;
using Cooking.Data;
using Ui;
using Ui.Enum;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ai
{
    public class TaskManager : MonoBehaviour
    {
        public RecipePreview recipePreview;
        public Bot[] bots;

        private Meal _lockedMeal = null;
        private List<OngoingTask> _ongoingTasks = new List<OngoingTask>();
        private Plate _designatedPlate;

        public void LockCurrentMeal(InputAction.CallbackContext context)
        {
            if (!context.performed || _lockedMeal || !GameState.Started)
            {
                return;
            }

            _lockedMeal = recipePreview.GetCurrentMeal();
            recipePreview.MarkLocked();

            GiveTasks();
        }

        private void GiveTasks()
        {
            _ongoingTasks.Clear();

            var availableBots = new Queue<Bot>(bots);

            for (var i = 0; i < _lockedMeal.tasks.Length; i++)
            {
                var task = _lockedMeal.tasks[i];
                var bot = availableBots.Count > 0 ? availableBots.Dequeue() : null;

                _ongoingTasks.Add(new OngoingTask(task, bot, i));
                if (!bot)
                {
                    continue;
                }

                bot.StartTask(task, this);
                recipePreview.SetTaskStateOfLocked(i, TaskState.Bot);
            }
        }

        public Plate GetDesignatedPlate()
        {
            if (_designatedPlate)
            {
                return _designatedPlate;
            }
            
            // Search for all plates in the game
            var plates = GameObject.FindObjectsOfType<Plate>();
            foreach (var nonEmptyPlate in plates.Where(plate => !plate.IsEmpty()))
            {
                if (nonEmptyPlate.GetMeal() != _lockedMeal)
                {
                    continue;
                }
                
                _designatedPlate = nonEmptyPlate;
                return _designatedPlate;
            }
            
            foreach (var nonEmptyPlate in plates.Where(plate => !plate.IsEmpty()))
            {
                if (!nonEmptyPlate.CanBuildMeal(_lockedMeal))
                {
                    continue;
                }
                
                _designatedPlate = nonEmptyPlate;
                return _designatedPlate;
            }

            _designatedPlate = plates.First(plate => plate.IsEmpty());
            return _designatedPlate;
        }

        private void Update()
        {
            if (_designatedPlate == null || _ongoingTasks.Count == 0)
            {
                return;
            }

            var pickableMeal = _designatedPlate.GetPickableMeal();
            if (!pickableMeal)
            {
                return;
            }

            if (pickableMeal.IsComplete())
            {
                // UNLOCK IN UI
                recipePreview.MarkUnlocked();
                
                // Clear ongoing tasks
                _ongoingTasks.Clear();
                
                // Clear designated plate
                _designatedPlate = null;
                
                // Clear locked meal
                _lockedMeal = null;
            }
        }

        public void MarkCompleted(Bot bot)
        {
            var task = _ongoingTasks.FirstOrDefault(task => task.GetBot() == bot);
            if (task == null)
            {
                return;
            }

            task.MarkCompleted();
            recipePreview.SetTaskStateOfLocked(task.GetUiIndex(), TaskState.Done);
        }
        
        public void MarkKilled(Bot bot)
        {
            var task = _ongoingTasks.FirstOrDefault(task => task.GetBot() == bot);
            if (task == null || task.IsCompleted())
            {
                return;
            }
            
            // Mark in UI, that task is no longer managed by bot
            recipePreview.SetTaskStateOfLocked(task.GetUiIndex(), TaskState.Normal);
        }
    }
}