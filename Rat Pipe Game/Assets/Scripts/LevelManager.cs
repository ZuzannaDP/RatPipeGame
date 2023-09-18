
public static class LevelManager
{
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
            levelData.startPoint, 
            levelData.endPoint, 
            levelData.startingDirection
        );

        return game;
    }
}