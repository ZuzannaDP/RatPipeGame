using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    // Tilemap for pipes
    [SerializeField]
    private Tilemap pipes;

    // Game logic and progresss
    private Game game;

    // Test level
    public LevelData currentLevel;

    // Pipe prefab
    public GameObject pipePrefab;

    void Awake() {
        game = LevelManager.LoadLevel(currentLevel);
        SpriteManager.LoadSprites();

        DisplayGame();
    }

    /// <summary>
    /// Display the grid in isometric coordinates.
    /// </summary>
    public void DisplayGame() {
        // Starting position
        Vector3Int position = pipes.WorldToCell(transform.position);

        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // Calculate the isometric coordinates
                    float xCoord = (float) (y * 0.5 + x * 0.5);
                    float yCoord = (float) (y * 0.25 - x * 0.25 + z * 0.5);
                    float zCoord = (float) (z);
                    if (game.Grid[x,y,z] != null) {
                        Debug.Log(xCoord + ", " + yCoord + ", " + zCoord);
                        CreatePipe(game.Grid[x, y, z], new Vector3(xCoord, yCoord, zCoord));
                    }
                }
            }
        }

        pipes.RefreshAllTiles();
    }

    /// <summary>
    /// Create a pipe game object in the world.
    /// </summary>
    /// <param name="pipe"></param>
    /// <param name="pos"></param>
    public void CreatePipe(Pipe pipe, Vector3 pos) {
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.transform.SetParent(transform, false);
        newPipe.transform.position = newPipe.transform.position + pos;

        string code = String.Join(",", pipe.Exits.Select(i => i.ToString()).ToArray());

        newPipe.GetComponent<SortingGroup>().sortingOrder = (int) pos.z;
        newPipe.GetComponent<SpriteRenderer>().sprite = SpriteManager.PipeSprites[code];
    }
}
