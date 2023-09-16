using UnityEngine;
using System.Collections;

public class Pipe {
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
}