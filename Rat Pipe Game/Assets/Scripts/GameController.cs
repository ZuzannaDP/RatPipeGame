using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    // Tilemap for pipes
    [SerializeField]
    private Tilemap pipes;

    // Game logic and progresss
    private Game game;

    // Test level
    public LevelData currentLevel;

    void Awake() {
        game = LevelManager.LoadLevel(currentLevel);

        DisplayGame();
    }

    void DisplayGame() {
        // Starting position
        Vector3Int position = pipes.WorldToCell(transform.position);

        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // if (grid[x,y,z] == 1) {
                    //     Vector3Int currentPipePosition = position;
                    //     currentPipePosition.x+=x;
                    //     currentPipePosition.y+=y;
                    //     currentPipePosition.z+=z*2;
                    //     pipes.SetTile(currentPipePosition, block);
                    // }
                }
            }
        }

        pipes.RefreshAllTiles();
    }
}
