using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    private LevelData level;

    public void Select() {
        LevelManager.LoadScene(level);
    }
}
