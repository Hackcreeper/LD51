using System.Collections.Generic;
using Ai.Data;
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

        public void LockCurrentMeal(InputAction.CallbackContext context)
        {
            if (!context.performed || _lockedMeal)
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

                _ongoingTasks.Add(new OngoingTask(task, bot));
                if (!bot)
                {
                    continue;
                }

                bot.StartTask(task, this);
                recipePreview.SetTaskStateOfLocked(i, TaskState.Bot);
            }
        }
    }
}