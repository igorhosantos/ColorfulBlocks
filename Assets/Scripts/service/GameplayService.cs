using System.Collections.Generic;
using ColorfulBlocks.Model;
using UnityEngine;
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

        private UserMovementFeed _lastUserMovementFeed;

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
            
            Debug.Log(LogGrid());
        }

        public UserMovementFeed RequestMovement(GridPiece gridPiece)
        {
            _lastUserMovementFeed = new UserMovementFeed(SearchAllAffectedBlocks(gridPiece));
            
            Movements--;
            Scores +=  _dataSettings.Score;

            if (Movements == 0)
            {
                SessionIsFinished = true;
            }

            return _lastUserMovementFeed;
        }

        private List<GridPiece> SearchAllAffectedBlocks(GridPiece gridPiece)
        {
            List<GridPiece> affectedPieces = new List<GridPiece>();
            affectedPieces.Add(gridPiece);
            
            //recursive finding of the affected blocks
            SearchAllAffectedBlocksRecursive(gridPiece, affectedPieces);

            //filtering the unique pieces affected to avoid duplications
            List<GridPiece> uniqueAffectedPieces = new List<GridPiece>();
            foreach (var piece in affectedPieces)
            {
                if (!uniqueAffectedPieces.Exists(existing => existing.AreEquals(piece)))
                {
                    piece.SetAsDirty();
                    uniqueAffectedPieces.Add(piece);
                }
            }

            return uniqueAffectedPieces;
        }

        private void SearchAllAffectedBlocksRecursive(GridPiece gridPiece, List<GridPiece> affectedPieces)
        {
            var searchAround = SearchAroundBlocks(gridPiece);

            if (searchAround.Count == 0)
            {
                return;
            }

            foreach (var piece in searchAround)
            {
                if (affectedPieces.Exists(existing => existing.AreEquals(piece)))
                {
                    continue;
                }

                affectedPieces.Add(piece);
                SearchAllAffectedBlocksRecursive(piece, affectedPieces);
            }
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

        private GridPiece GetNextValidPiecePerColumn(int x, int y)
        {
            for (int i = x; i < Grid.GetLength(1); i++)
            {
                var foundPiece = GetPiece(i+1, y);
                if (foundPiece is { IsDirty: false })
                {
                    return foundPiece;
                }
            }
            
            return null;
        }
        
        public GridPiece[,] UpdateGrid()
        {
            var oldBlocksCollected = _lastUserMovementFeed.BlocksCollected;
            
            //fall down the remaining pieces
            foreach (var oldPiece in oldBlocksCollected)
            {
                var oldPieceX = oldPiece.PosX;
                var oldPieceY = oldPiece.PosY;
                
                //access the grid and update the column changes 
                for (int i = oldPieceX; i < Grid.GetLength(1); i++)
                {
                    var previousPiece = GetPiece(i, oldPieceY);
                    var nextValidPiece = GetNextValidPiecePerColumn(i, oldPieceY);

                    if (previousPiece != null && nextValidPiece != null)
                    {
                        previousPiece.UpdateBlockId(nextValidPiece.BlockId);
                        nextValidPiece.SetAsDirty();
                    }
                }
            }
            
            Debug.Log(LogGrid());
            
            //update the grid with the new ones
            return Grid;
        }
        
        private string LogGrid()
        {
            var str = $"Grid Log: \n";

            for (int i = Grid.GetLength(0)-1; i >= 0; i--)
            {
                for (int j = Grid.GetLength(1)-1; j >= 0 ; j--)
                {
                    str += Grid[i, j].BlockId + ",";
                }

                str += '\n';
            }
            return str;
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
        public bool IsDirty  { get; private set; }

        public GridPiece(int posX, int posY, string blockId)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.BlockId = blockId;
        }

        public void SetAsDirty()
        {
            IsDirty = true;
        }
        
        public void UpdateBlockId(string blockId)
        {
            this.BlockId = blockId;
            IsDirty = false; 
        }

        public bool AreEquals(GridPiece obj)
        {
            return BlockId == obj.BlockId && PosX == obj.PosX && PosY == obj.PosY;
        }

        public override string ToString()
        {
            return $"{BlockId} - {PosX},{PosY}";
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