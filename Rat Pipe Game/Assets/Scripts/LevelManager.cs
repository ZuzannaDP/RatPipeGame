using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{

    public static LevelData levelData;

    /// <summary>
    /// Creates a game level from the stored level data.
    /// </summary>
    /// <param name="levelData"></param>
    public static Game LoadLevel(LevelData levelData) {
        Pipe[,,] grid = new Pipe[levelData.length,levelData.width,levelData.height];
        int count = 0;

         for (int x = 0; x < levelData.length; x++) {
            for (int y = 0; y < levelData.width; y++) {
                for (int z = 0; z < levelData.height; z++) {
                    if (levelData.grid[count].exits.Length == 0) {
                        grid[x,y,z] = null;
                    } else {
                        grid[x,y,z] = new Pipe(levelData.grid[count]);
                    }
                    count++;
                }
            }
        }

        Game game = new Game(
            grid, 
            new Position(levelData.startPoint[0], levelData.startPoint[1], levelData.startPoint[2]), 
            new Position(levelData.endPoint[0], levelData.endPoint[1], levelData.endPoint[2]), 
            new Direction(levelData.startingDirection[0], levelData.startingDirection[1], levelData.startingDirection[2]),
            new Direction(levelData.endPointExitDirection[0], levelData.endPointExitDirection[1], levelData.endPointExitDirection[2]),
            new Direction(levelData.startPointExitDirection[0], levelData.startPointExitDirection[1], levelData.startPointExitDirection[2])
        );

        return game;
    }

    public static void LoadScene(LevelData newLevelData) {
        levelData = newLevelData;
        SceneManager.LoadScene("Level");
    }
}