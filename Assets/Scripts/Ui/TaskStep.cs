using Ui.Enum;
using UnityEngine;

namespace Ui
{
    public class TaskStep : MonoBehaviour
    {
        public TaskState state = TaskState.Normal;
        public GameObject botImage;

        public void UpdateState(TaskState newState)
        {
            state = newState;
            
            botImage.SetActive(false);

            switch (state)
            {
                case TaskState.Bot:
                    botImage.SetActive(true);
                    break;
                
                case TaskState.Done:
                    botImage.SetActive(true);
                    // TODO: Replace with completed checkmark
                    break;
            }
        }
    }
}