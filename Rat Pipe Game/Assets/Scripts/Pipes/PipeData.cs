using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Pipe", menuName = "ScriptableObjects/PipeData")]
public class PipeData : ScriptableObject
{
    public int[] exits;
    public bool movable;
}