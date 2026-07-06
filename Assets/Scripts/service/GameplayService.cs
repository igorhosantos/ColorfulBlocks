using ColorfulBlocks.Model;
using Random = UnityEngine.Random;


namespace ColorfulBlocks.Service
{
    public class GameplayService
    {
        private GameplayDataSettings _dataSettings;

        public int Scores { get; private set; }
        public int Movements { get; private set; }
        
        public bool SessionIsFinished { get; private set; }

        public GridPiece[,] Grid;

        public GameplayService(GameplayDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
            Movements = dataSettings.Movements;
            SessionIsFinished = false;
            Grid = new GridPiece[dataSettings.GridLine,  dataSettings.GridColumn];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < _dataSettings.GridLine; i++)
            {
                for (int j = 0; j < _dataSettings.GridColumn; j++)
                {
                    var randomBlockId = Random.Range(0, _dataSettings.BlockTypes.Capacity);
                    Grid[i,j] = new GridPiece(i,j, _dataSettings.BlockTypes[randomBlockId].Id);
                }
            }
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


    /// <summary>
    /// Represents one square of the grid that contains the coordination
    /// and the current block id there
    /// </summary>
    public class GridPiece
    {
        public int PosX { get; private set; }
        public int PosY  { get; private set; }
        public string BlockId  { get; private set; }

        public GridPiece(int posX, int posY, string blockId)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.BlockId = blockId;
        }

        public void UpdatePos(int posX, int posY)
        {
            this.PosX = posX;
            this.PosY = posY;
        }
        
        public void UpdateBlockId(string blockId)
        {
            this.BlockId = blockId;
        }
    }
}