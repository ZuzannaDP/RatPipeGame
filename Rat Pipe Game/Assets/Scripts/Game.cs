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

    public Game(Pipe[,,] grid) {
        this.grid = grid;
    }

    public bool Move(int[] coordsPipeMoving, int[] coordMoveTo) {
        if (ValidMove(coordsPipeMoving, coordMoveTo)) {
            return true;
        }

        return false;
    }

    public bool ValidMove(int[] coordsPipeMoving, int[] coordMoveTo) {
        return false;
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
        this.selected.Reset();
        grid[selected.OrigCoords[0], selected.OrigCoords[1], selected.OrigCoords[2]] = selected.GetPipe;
        this.selected = null;
        return true;
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