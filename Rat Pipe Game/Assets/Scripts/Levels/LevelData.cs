using UnityEngine;

[CreateAssetMenu(fileName="Level", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public PipeData[] grid;
    public int length;
    public int width;
    public int height;
    public string name;
    public int[] startPoint;
    public int[] endPoint;
    public int[] startingDirection;
}