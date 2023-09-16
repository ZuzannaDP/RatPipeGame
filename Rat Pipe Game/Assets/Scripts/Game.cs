using UnityEngine;
using System.Collections;

public class Game { 
    private Pipe[,,] grid;
    public Pipe[,,] Grid => grid;
    private string name;
    public string Name => name;

    public Game(Pipe[,,] grid) {
        this.grid = grid;
    }
}