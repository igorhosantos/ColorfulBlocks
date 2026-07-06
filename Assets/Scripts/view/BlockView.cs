using System;
using ColorfulBlocks.Model;
using ColorfulBlocks.Service;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ColorfulBlocks.View
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Image blockImage;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameplayDataSettings _dataSettings;
        
        private string Id;
        private GridPiece _piece;
        public UnityAction<GridPiece> BlockClickedRequested;
        public void SetBlock(GridPiece piece)
        {
            _piece = piece;
            this.Id = piece.BlockId;
            blockImage.sprite = GetSpriteById(Id);
            label.text = $"{_piece.PosX},{_piece.PosY}";
            button.onClick.AddListener(()=> BlockClickedRequested?.Invoke(_piece));
        }
        
        public Sprite GetSpriteById(string id)
        {
            foreach (var dataSettingsBlockType in _dataSettings.BlockTypes)
            {
                if (dataSettingsBlockType.Id == id)
                {
                    return dataSettingsBlockType.Sprite;
                }
            }
           
            Debug.LogError("Sprite not found");
            return null;
        }
    }
}