namespace Ai.Data
{
    public class OngoingTask
    {
        private RecipeTask _task;
        private bool _completed;
        private Bot _bot;
        private int _uiIndex;

        public OngoingTask(RecipeTask task, Bot bot, int uiIndex)
        {
            _task = task;
            _bot = bot;
            _uiIndex = uiIndex;
        }

        public Bot GetBot() => _bot;
        public bool IsCompleted() => _completed;
        public int GetUiIndex() => _uiIndex;

        public void MarkCompleted()
        {
            _completed = true;
        }
    }
}