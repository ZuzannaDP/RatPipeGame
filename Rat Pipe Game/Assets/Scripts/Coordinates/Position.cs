using UnityEngine;
using System;

public class Position
{
    public int x;
    public int y;
    public int z;

    public Position(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Direction Adjacent(Position pos2) {
        Direction dir = GetDirection(pos2);

        if (dir.IsAdjacent()) {
            return dir;
        }

        return null;
    }

    public Direction GetDirection(Position pos2) {
        return new Direction (pos2.x - x, pos2.y - y, pos2.z - z);
    }

    public Position GetPosition(Direction dir) {
        return new Position (x + dir.x, y + dir.y, z + dir.z);
    }

    public int GetLevel() {
        return z;
    }

    public int GetAxis(Axis axis) {
        if (axis.Equals(Axis.Xaxis)) {
            return x;
        } else if (axis.Equals(Axis.Yaxis)) {
            return y;
        } else {
            return z;
        }
    }

    public bool Equals(Position pos) {        
        if (pos == null) {
            return false;
        }
        
        return x == pos.x && y == pos.y && z == pos.z; 
    }

    public Position IncrementAxis(Axis axis, int dir) {
        if (axis.Equals(Axis.Xaxis)) {
            return new Position(x + dir, y, z);
        } else if (axis.Equals(Axis.Yaxis)) {
            return new Position(x, y + dir, z);
        } else {
            return new Position(x, y, z + dir);
        }
    }

    public static Vector3 WorldPosition(Position pos) {
        float xCoord = (float) (pos.y * 0.5 + pos.x * 0.5);
        float yCoord = (float) (pos.y * 0.25 - pos.x * 0.25 + pos.z * 0.5);
        float zCoord = (float) (yCoord - pos.z);
        return new Vector3 (xCoord, yCoord, zCoord);
    }

    public static Position IsometricPosition(Vector3 pos, int zLayer) {
        float newX = pos.x - 2 * pos.y + zLayer;
        int xCoord = (int) Math.Round(newX);
        int yCoord = (int) Math.Round(4 * pos.y + newX - 2 * zLayer);
        int zCoord = zLayer;
        return new Position (xCoord, yCoord, zCoord);
    }

    public string Print() {
        return x + "," + y + "," + z;
    }
}