using UnityEngine;
using System;
using System.Linq;

public class Tests : MonoBehaviour
{
    public bool runTests = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (runTests) {
            RunPipeTests();
            Debug.Log("");
            RunDirectionTests();
        }
    }

    public void RunPipeTests() {
        int[] newExits = Pipe.Rotation(new int[] {1,0,0,0,0,1}, (int) Axis.Zaxis, 1);
        string code = String.Join(",", newExits.Select(i => i.ToString()).ToArray());
        Debug.Log(code);

        newExits = Pipe.Rotation(new int[] {0,1,1,0,0,0}, (int) Axis.Zaxis, -1);
        code = String.Join(",", newExits.Select(i => i.ToString()).ToArray());
        Debug.Log(code);
    }

    public void RunDirectionTests() {
        Direction dir = new Direction(0,1,0);
        Debug.Log(dir.Print() + " rotate left -> " + dir.Rotate(Axis.Zaxis, -1).Print());

        dir = new Direction(-1,0,0);
        Debug.Log(dir.Print() + " rotate left -> " + dir.Rotate(Axis.Zaxis, -1).Print());

        dir = new Direction(0,-1,0);
        Debug.Log(dir.Print() + " rotate left -> " + dir.Rotate(Axis.Zaxis, -1).Print());

        dir = new Direction(1,0,0);
        Debug.Log(dir.Print() + " rotate left -> " + dir.Rotate(Axis.Zaxis, -1).Print());
    }
}