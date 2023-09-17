using UnityEngine;
using System.Collections;

/// <summary>
/// All gameplay logic (seperate from UI).
/// </summary> 
public class Game { 
    private Pipe[,,] grid;
    public Pipe[,,] Grid => grid;
    private string name;
    public string Name => name;
    private Selection selected = null;
    public Selection Selected => selected;
    private int[] startPoint;
    private int[] endPoint;
    private Player player;
    public Player GetPlayer => player;

    public Game(Pipe[,,] grid, int[] startPoint, int[] endPoint, int[] dir) {
        this.grid = grid;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.player = new Player(startPoint, dir);
    }

    public bool InPipe() {
        return false;
    }















    /// <summary>
    /// 
    /// </summary>
    /// <param name="coordMoveTo"></param>
    /// <returns></returns>
    public bool MoveSelectedPipe(int[] coordMoveTo) {
        if (MovePipe(selected.GetPipe, coordMoveTo)) {
            this.selected = null;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pipe"></param>
    /// <param name="coordMoveTo"></param>
    /// <returns></returns>
    public bool MovePipe(Pipe pipe, int[] coordMoveTo) {
        if (ValidPipeMove(pipe, coordMoveTo)) {
            grid[coordMoveTo[0], coordMoveTo[1], coordMoveTo[2]] = pipe;
            return true;
        }

        return false;
    }

    /// <summary>
    /// A move is valid if at least one of the exits in a pipe conntects to
    /// an exit in another pipe; there isn't already a pipe in the space;
    /// </summary>
    /// <param name="coordsPipeMoving"></param>
    /// <param name="coordMoveTo"></param>
    /// <returns></returns>
    public bool ValidPipeMove(Pipe pipe, int[] coordMoveTo) {
        // Check empty
        if (grid[coordMoveTo[0], coordMoveTo[1], coordMoveTo[2]] != null) {
            return false;
        }

        // Check connections
        int[] exits = pipe.Exits;
        bool foundConnection = false;

        for (int e = 0; e < exits.Length; e++) {
            if (exits[e] == 1) {
                int[] direction = Pipe.directions[e];
                int[] adjacent = new int[] {coordMoveTo[0] + direction[0], coordMoveTo[1] + direction[1], coordMoveTo[2] + direction[2]};
                
                // Check pipe exists here
                if (ValidCoords(adjacent)) {
                    Pipe adjpipe = grid[adjacent[0], adjacent[1], adjacent[2]];

                    if (adjpipe != null && adjpipe.Exits[5 - e] == 1) {
                        foundConnection = true;
                    }
                }
            }
        }

        return foundConnection;
    }










    public bool ValidCoord(int[] coord, int axis) {
        return coord[axis] >= 0 && coord[axis] < grid.GetLength(axis);
    }

    public bool ValidCoords(int[] coord) {
        return ValidCoord(coord, (int) Axis.Xaxis) && ValidCoord(coord, (int) Axis.Yaxis) && ValidCoord(coord, (int) Axis.Zaxis);
    }

    public bool Select(int[] coords) {
        if (grid[coords[0], coords[1], coords[2]] != null && !IsSelected()) {
            this.selected = new Selection (grid[coords[0], coords[1], coords[2]], coords);
            grid[coords[0], coords[1], coords[2]] = null;
            return true;
        }

        return false;
    }

    public bool PutBack() {
        if (IsSelected()) {
            this.selected.Reset();
            grid[selected.OrigCoords[0], selected.OrigCoords[1], selected.OrigCoords[2]] = selected.GetPipe;
            this.selected = null;
            return true;
        }

        return false;
    }

    public bool IsSelected() {
        return selected != null;
    }

    public int[] RotateSelected(int axis, int direction) {
        return selected.GetPipe.Rotate(axis, direction);
    }
}








public class Selection {
    private Pipe pipe;
    public Pipe GetPipe => pipe;
    private int[] originalCoordinates;
    public int[] OrigCoords => originalCoordinates;
    private int[] originalExits;
    public int[] OrigExits => originalExits;

    public Selection(Pipe pipe, int[] originalCoordinates) {
        this.pipe = pipe;
        this.originalCoordinates = originalCoordinates;
        originalExits = pipe.Exits;
    }

    public void Reset() {
        pipe.SetExits(originalExits);
    }
}