using System.Collections.Generic;
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

        public UserMovementFeed RequestMovement(GridPiece gridPiece)
        {
            var userMovementFeed = new UserMovementFeed(SearchAllAffectedBlocks(gridPiece));
            
            Movements--;
            Scores +=  _dataSettings.Score;

            if (Movements == 0)
            {
                SessionIsFinished = true;
            }

            return userMovementFeed;
        }

        private List<GridPiece> SearchAllAffectedBlocks(GridPiece gridPiece)
        {
            List<GridPiece> affectedPieces = new List<GridPiece>();
            var searchAround  = SearchAroundBlocks(gridPiece);

            if (searchAround.Count > 0)
            {
                foreach (var affectedPiece in searchAround)
                {
                    affectedPieces.AddRange(SearchAroundBlocks(affectedPiece));
                }
            }
            
            //add the one clicked
            affectedPieces.Add(gridPiece);
            return affectedPieces;
        }

        private List<GridPiece> SearchAroundBlocks(GridPiece gridPiece)
        {
            List<GridPiece> aroundPieces = new List<GridPiece>();
          
            var left = GetPiece(gridPiece.PosX, gridPiece.PosY-1);
            var right = GetPiece(gridPiece.PosX, gridPiece.PosY+1);
            var up = GetPiece(gridPiece.PosX-1, gridPiece.PosY);
            var down = GetPiece(gridPiece.PosX+1, gridPiece.PosY);

            if (left != null && gridPiece.BlockId.Equals(left.BlockId))
            {
                aroundPieces.Add(left);
            }
            if (right != null && gridPiece.BlockId.Equals(right.BlockId))
            {
                aroundPieces.Add(right);
            }
            
            if (up != null && gridPiece.BlockId.Equals(up.BlockId))
            {
                aroundPieces.Add(up);
            }
            
            if (down != null && gridPiece.BlockId.Equals(down.BlockId))
            {
                aroundPieces.Add(down);
            }
            
            return aroundPieces;
        }

        private GridPiece GetPiece(int x, int y)
        {
            //respecting the bounds
            if (x >= 0 && x < _dataSettings.GridLine && y >= 0 && y < _dataSettings.GridColumn)
            {
                return Grid[x,y];
            }
            
            return null;
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

        public bool AreEquals(GridPiece obj)
        {
            return BlockId == obj.BlockId && PosX == obj.PosX && PosY == obj.PosY;
        }
    }
    
   /// <summary>
   /// Provide the information required per user movement to guide
   /// the view how it's going to work
   /// </summary>
    public class UserMovementFeed
    {
        public List<GridPiece> BlocksCollected { get; private set; }

        public UserMovementFeed(List<GridPiece> blocksCollected)
        {
            BlocksCollected = blocksCollected;
        }
    }
}