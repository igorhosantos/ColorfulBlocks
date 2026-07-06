using System.Collections.Generic;
using ColorfulBlocks.Service;
using UnityEngine;
using UnityEngine.Events;

namespace ColorfulBlocks.View
{
    public class GridView : MonoBehaviour
    {
        [SerializeField] private BlockView blockPrefab;
        
        private readonly List<BlockView> _blocks = new List<BlockView>();
        public UnityAction<GridPiece> BlockClickedRequested;
        
        public void InitializeGrid(GridPiece[,] grid)
        {
            ResetGrid();
            
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var block = Instantiate(blockPrefab, transform);
                    block.SetBlock(grid[i, j]);
                    block.BlockClickedRequested += OnBlockClickedRequested;
                    _blocks.Add(block);
                }
            }
        }

        private void OnBlockClickedRequested(GridPiece gridPiece)
        {
            BlockClickedRequested?.Invoke(gridPiece);
        }
        
        public void UpdateGrid(GridPiece[,] grid)
        {
            
        }
        
        private void ResetGrid()
        {
            foreach (var blockView in _blocks)
            {
                Destroy(blockView.gameObject);
            }
        }
    }
}