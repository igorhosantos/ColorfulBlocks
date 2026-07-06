using UnityEngine;

namespace ColorfulBlocks.Model
{
    [CreateAssetMenu(fileName = "GameplayDataSettings", menuName = "Scriptable Objects/GameplayDataSettings")]
    public class GameplayDataSettings : ScriptableObject
    {
        [SerializeField] private int movements;
        [SerializeField] private int score;
        
        
        public int Movements => movements;
        public int Score => score;
    }
}
