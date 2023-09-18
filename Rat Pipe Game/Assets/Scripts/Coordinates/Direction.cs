using UnityEngine;
using System;

public class Direction {
    public int x;
    public int y;
    public int z;

    public static Direction UP = new Direction(0, 0, 1);
    public static Direction DOWN = new Direction(0, 0, -1);
    public static Direction NE = new Direction(0, 1, 0);
    public static Direction SE = new Direction(1, 0, 0);
    public static Direction SW = new Direction(0, -1, 0);
    public static Direction NW = new Direction(-1, 0, 0);

    public Direction(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public bool IsAdjacent() {
        if (x > 1 || x < -1 || 
            y > 1 || y < -1 ||
            z > 1 || z < -1) {
                return false;
        }

        return true;
    }

    public bool Equals(Direction dir2) {
        if (dir2 == null) {
            return false;
        }

        return x == dir2.x && y == dir2.y && z == dir2.z;
    }

    public Direction Rotate(Axis axis, int dir) {
        int a = -1 * dir;
        int b = 1 * dir;

        if (axis.Equals(Axis.Zaxis)) {
            return new Direction(
                x == 0 ? y * b : 0,
                y == 0 ? x * a: 0,
                z
            );
        }
        
        // TODO: complete this
        return new Direction(x, y, z);
    }

    public string Print() {
        return x + ", " + y + ", " + z;
    }

    public Direction Opposite() {
        return new Direction(x * -1, y * -1, z * -1);
    }
}