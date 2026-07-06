using System;
using ColorfulBlocks.Model;
using ColorfulBlocks.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorfulBlocks.View
{
    public class GameplayView : MonoBehaviour
    {
       [SerializeField] private GameplayDataSettings dataSettings;
       [SerializeField] private TextMeshProUGUI scoreText;
       [SerializeField] private TextMeshProUGUI movementText;
       [SerializeField] private Button makeMoveButton;
       [SerializeField] private Canvas canvasPopup;
       [SerializeField] private Button resetGameButton;
       [SerializeField] private GridView gridView;

       private GameplayService _gameplayService;
       
       private void Awake()
       {
           ResetGame();
           makeMoveButton.onClick.AddListener(RequestMovement);
           resetGameButton.onClick.AddListener(ResetGame);
       }

       private void ResetGame()
       {
           //clear UI
           canvasPopup.gameObject.SetActive(false);
           StartNewSession();
       }
       
       private void StartNewSession()
       {
           _gameplayService = new GameplayService(dataSettings);
           scoreText.text = $"{_gameplayService.Scores}";
           movementText.text = $"{_gameplayService.Movements}";
           
           //draw the grid
           gridView.InitializeGrid(_gameplayService.Grid);
           gridView.BlockClickedRequested += OnBlockClickedRequested;
       }

       private void RequestMovement()
       {
           _gameplayService.RequestMovement();
           scoreText.text = $"{_gameplayService.Scores}";
           movementText.text = $"{_gameplayService.Movements}";

           //show game over
           if (_gameplayService.SessionIsFinished)
           {
               canvasPopup.gameObject.SetActive(true);
           }
       }

       private void OnBlockClickedRequested(GridPiece  gridPiece)
       {
           Debug.Log($"OnBlockClickedRequested: {gridPiece.BlockId} - {gridPiece.PosX} - {gridPiece.PosY}");
       }
    }
}