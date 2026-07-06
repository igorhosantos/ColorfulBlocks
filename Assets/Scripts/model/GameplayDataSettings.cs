using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorfulBlocks.Model
{
    [CreateAssetMenu(fileName = "GameplayDataSettings", menuName = "Scriptable Objects/GameplayDataSettings")]
    public class GameplayDataSettings : ScriptableObject
    {
        [SerializeField] private int movements;
        [SerializeField] private int score;
        [SerializeField] private int gridLine;
        [SerializeField] private int gridColumn;
        [SerializeField] private List<BlockType> blockTypes;
        
        public int Movements => movements;
        public int Score => score;
        public int GridLine => gridLine;
        public int GridColumn => gridColumn;
        public List<BlockType> BlockTypes => blockTypes;
    }

    [Serializable]
    public struct BlockType
    {
        public string Id;
        public Sprite Sprite;
    }
}
