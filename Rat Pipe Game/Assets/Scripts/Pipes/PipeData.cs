using UnityEngine;

[CreateAssetMenu(fileName="Pipe", menuName = "ScriptableObjects/PipeData")]
public class PipeData : ScriptableObject
{
    public int[] exits;
    public bool movable;
}