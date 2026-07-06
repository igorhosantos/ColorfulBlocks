using ColorfulBlocks.Model;


namespace ColorfulBlocks.Service
{
    public class GameplayService
    {
        private GameplayDataSettings _dataSettings;

        public int Scores { get; private set; }
        public int Movements { get; private set; }
        
        public bool SessionIsFinished { get; private set; }

        public GameplayService(GameplayDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
            Movements = dataSettings.Movements;
            SessionIsFinished = false;
        }

        public void RequestMovement()
        {
            Movements--;
            Scores +=  _dataSettings.Score;

            if (Movements == 0)
            {
                SessionIsFinished = true;
            }
        }
    }
}