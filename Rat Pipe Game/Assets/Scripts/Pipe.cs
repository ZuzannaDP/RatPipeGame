using UnityEngine;
using System.Collections;

public class Pipe {
    public static int[] zaxis = new int[] {1, 4, 6, 3};
    public static int[] staticzaxis = new int[] {2, 5};
    public static int[] xaxis = new int[] {1, 5, 6, 2};
    public static int[] staticxaxis = new int[] {3, 4};
    public static int[] yaxis = new int[] {3, 2, 4, 5};
    public static int[] staticyaxis = new int[] {1, 6};
    public static int[][] directions = new int[6][] {
        new int[3] {0,-1,0}, 
        new int[3] {0,0,-1}, 
        new int[3] {1,0,0}, 
        new int[3] {-1,0,0}, 
        new int[3] {0,0,1}, 
        new int[3] {0,1,0}
    };

    private int[] exits;
    public int[] Exits => exits;
    public bool movable;

    // for longer pipes
    // public dictionary<int, Pipe> connectedPipes;

    public Pipe(PipeData pipeData) {
        this.exits = pipeData.exits;
        this.movable = pipeData.movable;
    }

    public bool IsEmpty() {
        return exits.Length == 0;
    }

    /// <summary>
    /// Finds the rotation given the axis and direction the rotation is occuring.
    /// For example, a clockwise spin on the zaxis would have parameters 0, 0, 1.
    /// </summary>
    /// <param name="xaxis"></param>
    /// <param name="yaxis"></param>
    /// <param name="zaxis"></param>
    public static int[] Rotation(int[] exits, int axis, int direction) {
        if (axis == (int) Axis.Xaxis) { 
            return RotationOnAxis(Pipe.xaxis, direction, exits, Pipe.staticxaxis); 
        } else if (axis == (int) Axis.Yaxis) { 
            return RotationOnAxis(Pipe.yaxis, direction, exits, Pipe.staticyaxis); 
        } else if (axis == (int) Axis.Zaxis) { 
            return RotationOnAxis(Pipe.zaxis, direction, exits, Pipe.staticzaxis); 
        }

        return exits;
    }

    public int[] Rotate(int axis, int direction) {
        this.exits = Pipe.Rotation(exits, axis, direction);
        return this.exits;
    }

    private static int[] RotationOnAxis(int[] axis, int direction, int[] exits, int[] statics) {
        int[] newExits = new int[exits.Length];

        for (int r = 0; r < axis.Length; r++) {
            if (exits[axis[r] - 1] == 1) {
                newExits[axis[MyMath.Mod(r + direction, 4)] - 1] = 1;
            }
        } 

        for (int p = 0; p < statics.Length; p++) {
            newExits[statics[p] - 1] = exits[statics[p] - 1];
        }

        return newExits;
    }

    public void SetExits(int[] newExits) {
        if (newExits.Length == 6) {
            exits = newExits;
        }
    }
}

public static class MyMath {
    public static int Mod(int x, int m) {
        return (x%m + m)%m;
    }
}