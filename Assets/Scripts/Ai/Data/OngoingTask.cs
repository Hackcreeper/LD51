namespace Ai.Data
{
    public class OngoingTask
    {
        private RecipeTask _task;
        private bool _completed;
        private Bot _bot;

        public OngoingTask(RecipeTask task, Bot bot)
        {
            _task = task;
            _bot = bot;
        }
    }
}