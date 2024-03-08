
using UnityEngine;
[CreateAssetMenu(menuName = "Level State")]
public class LevelState : ScriptableObject
{
    public int level;
    public int amout;
    public TileInfomation[] tileInfomation;
    [System.Serializable]
    public class TileInfomation
    {
        public int id;
        public int x_Pos;
        public int y_Pos;
    }
}
