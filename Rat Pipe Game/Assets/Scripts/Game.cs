using UnityEngine;

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
    private Position startPoint;
    private Position endPoint;
    private Player player;
    public Player GetPlayer => player;

    public Game(Pipe[,,] grid, Position startPoint, Position endPoint, Direction dir) {
        this.grid = grid;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        player = new Player(startPoint, dir);
    }

    /// <summary>
    /// Move Player to the new position if there is a pipe in that
    /// space and the pipe in the curent space connects.
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    public bool MovePlayer(Position newPos) {
        // Check Player is already in this position
        if (player.position.Equals(newPos)) {
            Debug.Log("Already here");
            return true;
        }

        // Check position is not outside grid
        if (!ValidCoords(newPos)) {
            Debug.Log("Outside grid!");
            return false;
        }

        // Check pipe exists
        Pipe pipeMoveTo = grid[newPos.x, newPos.y, newPos.z];
        if (pipeMoveTo == null) {
            Debug.Log("Pipe doesn't exist!");
            return false;
        }

        // Check pipe is adjacent
        Direction pipeDir = player.position.Adjacent(newPos);
        Debug.Log("current position: " + player.position.Print());
        Debug.Log("new position: " + newPos.Print());
        if (pipeDir == null) {
            Debug.Log("Pipe is not adjacent!");
            return false;
        }
        Debug.Log("pipe direction: " + pipeDir.Print());

        // Check exits connect
        if (grid[player.position.x, player.position.y, player.position.z].HasExit(pipeDir) && 
            pipeMoveTo.HasExit(pipeDir.Opposite())) {
                Debug.Log("Connection");
                player.position = newPos;
                return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="coordMoveTo"></param>
    /// <returns></returns>
    public bool MoveSelectedPipe(Position coordMoveTo) {
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
    public bool MovePipe(Pipe pipe, Position coordMoveTo) {
        if (ValidPipeMove(pipe, coordMoveTo)) {
            grid[coordMoveTo.x, coordMoveTo.y, coordMoveTo.z] = pipe;
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
    public bool ValidPipeMove(Pipe pipe, Position coordMoveTo) {
        // Check empty
        if (grid[coordMoveTo.x, coordMoveTo.y, coordMoveTo.z] != null) {
            return false;
        }

        // Check connections
        int[] exits = pipe.Exits;
        bool foundConnection = false;

        for (int e = 0; e < exits.Length; e++) {
            if (exits[e] == 1) {
                Direction direction = Pipe.directions[e];
                Position adjacent = new Position (coordMoveTo.x + direction.x, coordMoveTo.y + direction.y, coordMoveTo.z + direction.z);
                
                // Check pipe exists here
                if (ValidCoords(adjacent)) {
                    Pipe adjpipe = grid[adjacent.x, adjacent.y, adjacent.z];

                    if (adjpipe != null && adjpipe.Exits[5 - e] == 1) {
                        foundConnection = true;
                    }
                }
            }
        }

        return foundConnection;
    }

    public bool ValidCoord(Position coord, Axis axis) {
        return coord.GetAxis(axis) >= 0 && coord.GetAxis(axis) < grid.GetLength((int) axis);
    }

    public bool ValidCoords(Position coord) {
        return ValidCoord(coord, Axis.Xaxis) && ValidCoord(coord, Axis.Yaxis) && ValidCoord(coord, Axis.Zaxis);
    }

    public bool Select(Position coords) {
        if (player.position.Equals(coords)) {
            return false;
        }

        if (grid[coords.x, coords.y, coords.z] != null && !IsSelected()) {
            selected = new Selection (grid[coords.x, coords.y, coords.z], coords);
            grid[coords.x, coords.y, coords.z] = null;
            return true;
        }

        return false;
    }

    public bool PutBack() {
        if (IsSelected()) {
            selected.Reset();
            grid[selected.OrigCoords.x, selected.OrigCoords.y, selected.OrigCoords.z] = selected.GetPipe;
            selected = null;
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
    private Position originalCoordinates;
    public Position OrigCoords => originalCoordinates;
    private int[] originalExits;
    public int[] OrigExits => originalExits;

    public Selection(Pipe pipe, Position originalCoordinates) {
        this.pipe = pipe;
        this.originalCoordinates = originalCoordinates;
        originalExits = pipe.Exits;
    }

    public void Reset() {
        pipe.SetExits(originalExits);
    }
}