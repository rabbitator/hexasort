using UnityEngine;

namespace HexaSort.Game.Level
{
    [CreateAssetMenu(fileName = "HexMap", menuName = "HexMap")]
    public class HexMapAsset : ScriptableObject
    {
        public Vector2Int size = new(10, 10);
        public bool[] cells = new bool[100];
    }
}