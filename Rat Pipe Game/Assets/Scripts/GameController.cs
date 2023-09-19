using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // TEST REMOVE LATER
    public LevelData testLevelData;

    // Tilemap for pipes
    [SerializeField]
    private Tilemap pipes;

    // Game logic and progresss
    private Game game;
    private PipeController[,,] pipeGrid;

    // Prefabs
    public GameObject pipePrefab;
    public GameObject spacePrefab;
    public GameObject layerPrefab;
    public GameObject indicatorPrefab;

    // Player
    [SerializeField]
    private GameObject playerObject;
    private PlayerController playerController;

    [SerializeField]
    private Cursor cursor;
    private int selectedLayer = 0;
    private GameObject[] layers;
    private int rotationAxis = (int) Axis.Xaxis;

    void Awake() {
        game = LevelManager.LoadLevel(LevelManager.levelData != null ? LevelManager.levelData : testLevelData);
        SpriteManager.LoadSprites();

        DisplayGame();

        DisplaySpaces();

        DisplayPlayer();
    }

    public void DisplayPlayer() {
        Position playerPos = game.GetPlayer.position;
        Vector3 coords = Position.WorldPosition(playerPos);

        // if using prefab return this
        // GameObject newPlayer = Instantiate(playerPrefab);
        // newPlayer.transform.SetParent(transform, false);
        playerObject.transform.position = playerObject.transform.position + coords;

        playerController = playerObject.GetComponent<PlayerController>();
        playerController.UpdateDirection(game.GetPlayer.Facing);
    }

    /// <summary>
    /// Display the grid in isometric coordinates.
    /// </summary>
    public void DisplayGame() {
        pipeGrid = new PipeController[game.Grid.GetLength(0),game.Grid.GetLength(1),game.Grid.GetLength(2)];

        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // Calculate the isometric coordinates
                    Position pos = new Position(x, y, z);
                    Vector3 coords = Position.WorldPosition(new Position(x, y, z));

                    pipeGrid[x, y, z] = CreatePipe(game.Grid[x, y, z], coords, pos);
                }
            }
        }

        // Add start and end indicators
        Vector3 startPos = Position.WorldPosition(game.StartPoint.GetPosition(game.StartPointExitDir));
        CreateIndicator(startPos, game.StartPoint, game.StartPointExitDir.Opposite());

        Vector3 endPos = Position.WorldPosition(game.EndPoint.GetPosition(game.EndPointExitDir));
        CreateIndicator(endPos, game.EndPoint, game.EndPointExitDir.Opposite());
    }

    public void DisplaySpaces() {
        // Create layers at different heights
        GameObject[] layers = CreateLayers(game.Grid.GetLength(2));

        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // Calculate the isometric coordinates
                    Position pos = new Position(x, y, z);
                    Vector3 coords = Position.WorldPosition(pos);

                    CreateSpace(layers[z], coords, pos, game.Grid.GetLength(2));
                }
            }
        }

        this.layers = layers;
    }

    public GameObject[] CreateLayers(int zheight) {
        GameObject[] layers = new GameObject[zheight];

        for (int z = 0; z < zheight; z++) {
            GameObject newLayer = Instantiate(layerPrefab);
            newLayer.transform.SetParent(transform, false);
            newLayer.SetActive(false);
            layers[z] = newLayer;
        }

        return layers;
    }

    public void CreateSpace(GameObject layer, Vector3 vecpos, Position pos, int zheight) {
        vecpos.z = (float) vecpos.z - zheight;
        GameObject newSpace = Instantiate(spacePrefab);
        newSpace.transform.SetParent(layer.transform, false);
        newSpace.transform.position = newSpace.transform.position + vecpos;

        newSpace.GetComponent<SortingGroup>().sortingOrder = pos.z + zheight;
        SpaceController spaceController = newSpace.GetComponent<SpaceController>();

        spaceController.UpdateCoordinates(pos);
        spaceController.gameController = this;
        
    }

    /// <summary>
    /// Create a pipe game object in the world.
    /// </summary>
    /// <param name="pipe"></param>
    /// <param name="pos"></param>
    public PipeController CreatePipe(Pipe pipe, Vector3 vecpos, Position pos) {
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.transform.SetParent(transform, false);
        newPipe.transform.position = newPipe.transform.position + vecpos;

        newPipe.GetComponent<SortingGroup>().sortingOrder = pos.z;
        
        PipeController pipeController = newPipe.GetComponent<PipeController>();
        pipeController.DrawPipe(pipe);
        pipeController.UpdateCoordinates(pos);
        pipeController.gameController = this;

        return pipeController;
        
    }

    public void CreateIndicator(Vector3 vecpos, Position pos, Direction dir) {
        GameObject newStartIndicator = Instantiate(indicatorPrefab);
        newStartIndicator.transform.SetParent(transform, false);
        newStartIndicator.transform.position = newStartIndicator.transform.position + vecpos;

        newStartIndicator.GetComponent<SortingGroup>().sortingOrder = pos.z;
        newStartIndicator.GetComponent<SpriteRenderer>().sprite = SpriteManager.IndicatorSprites[dir.Print()];
    }

    public bool MoveRat(Vector3 vecPos) {
        return game.MovePlayer(Position.IsometricPosition(vecPos, game.GetPlayer.position.z));
    }

    public bool JumpRat() {
        return game.MovePlayer(game.GetPlayer.position.IncrementAxis(Axis.Zaxis, 1));
    }

    public bool FallRat() {
        return game.MovePlayer(game.GetPlayer.position.IncrementAxis(Axis.Zaxis, -1));
    }

    public Position GetPlayerPosition() {
        return game.GetPlayer.position;
    }

    public bool HasPlayerWon() {
        return game.HasWon();
    }

    public void PlayerWon() {
        SceneManager.LoadScene("Won");
    }









    ///////////// Actions

    public void Selected(PipeController pipeController, Position coordinates) {
        if (game.Select(coordinates)) {
            selectedLayer = coordinates.z;
            cursor.EnableCursor(pipeController);
            layers[selectedLayer].SetActive(true);
        }
    }

    public void SelectedSpace(Position coords) {
        if (game.MoveSelectedPipe(coords)) {
            pipeGrid[coords.x, coords.y, coords.z].DrawPipe(game.Grid[coords.x, coords.y, coords.z]);
            cursor.DisableCursor();
            layers[selectedLayer].SetActive(false);
        }
    }

    public void OnDeselect(InputAction.CallbackContext context) {
        if (context.started) {
            if (game.PutBack()) {
                layers[selectedLayer].SetActive(false);
                cursor.OnDeselect();
            }
        }
    }

    public void OnRaise(InputAction.CallbackContext context) {
        if (context.started) {
            if (selectedLayer < game.Grid.GetLength(2) - 1) {
                layers[selectedLayer].SetActive(false);
                selectedLayer ++;
                cursor.IncreaseSortOrder();
                layers[selectedLayer].SetActive(true);
            }
        }
    }

    public void OnLower(InputAction.CallbackContext context) {
        if (context.started) {
            if (selectedLayer > 0) {
                layers[selectedLayer].SetActive(false);
                selectedLayer --;
                cursor.DecreaseSortOrder();
                layers[selectedLayer].SetActive(true);
            }
        }
    }

    public void OnChangeRotationAxis(InputAction.CallbackContext context) {
        if (context.started) {
            this.rotationAxis = (this.rotationAxis + 1) % 3;
            Debug.Log(rotationAxis);
        }
    }

    public void OnRotateForward(InputAction.CallbackContext context) {
        if (context.started) {
            int[] newExits = game.RotateSelected(this.rotationAxis, (int) Dir.Forward);
            cursor.Rotate(newExits);
        }
    }

    public void OnRotateBackward(InputAction.CallbackContext context) {
        if (context.started) {
            int[] newExits = game.RotateSelected(this.rotationAxis, (int) Dir.Backward);
            cursor.Rotate(newExits);
        }
    }

    public bool IsSelected() {
        return game.IsSelected();
    }
}
