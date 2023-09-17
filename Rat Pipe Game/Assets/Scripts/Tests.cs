using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class Tests : MonoBehaviour
{
    public bool runTests = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (runTests) {
            RunTests();
        }
    }

    public void RunTests() {
        int[] newExits = Pipe.Rotation(new int[] {1,0,0,0,0,1}, (int) Axis.Zaxis, 1);
        string code = String.Join(",", newExits.Select(i => i.ToString()).ToArray());
        Debug.Log(code);

        newExits = Pipe.Rotation(new int[] {0,1,1,0,0,0}, (int) Axis.Zaxis, -1);
        code = String.Join(",", newExits.Select(i => i.ToString()).ToArray());
        Debug.Log(code);
    }
}