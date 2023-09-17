using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
}