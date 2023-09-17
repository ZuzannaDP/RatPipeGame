using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public enum Axis : int {
    Xaxis = 0,
    Yaxis = 1,
    Zaxis = 2
}

public enum Direction : int {
    Forward = 1,
    Backward = -1,
    None = 0
}

public class GameController : MonoBehaviour
{
    // Tilemap for pipes
    [SerializeField]
    private Tilemap pipes;

    // Game logic and progresss
    private Game game;
    private PipeController[,,] pipeGrid;

    // Test level
    public LevelData currentLevel;

    // Prefabs
    public GameObject pipePrefab;
    public GameObject spacePrefab;
    public GameObject layerPrefab;

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
        game = LevelManager.LoadLevel(currentLevel);
        SpriteManager.LoadSprites();

        DisplayGame();

        DisplaySpaces();

        DisplayPlayer();
    }

    public void DisplayPlayer() {
        int[] playerPos = game.GetPlayer.Position;
        float[] coords = WorldPosition(playerPos[0], playerPos[1], playerPos[2]);
        Vector3 vectCoords = new Vector3(coords[0], coords[1], coords[2]);

        // if using prefab return this
        // GameObject newPlayer = Instantiate(playerPrefab);
        // newPlayer.transform.SetParent(transform, false);
        playerObject.transform.position = playerObject.transform.position + vectCoords;

        playerController = playerObject.GetComponent<PlayerController>();
        playerController.UpdateDirection(game.GetPlayer.Direction);
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
                    float[] coords = WorldPosition(x, y, z);

                    pipeGrid[x, y, z] = CreatePipe(game.Grid[x, y, z], new Vector3(coords[0], coords[1], coords[2]), x, y, z);
                }
            }
        }
    }

    public void DisplaySpaces() {
        // Create layers at different heights
        GameObject[] layers = CreateLayers(game.Grid.GetLength(2));

        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // Calculate the isometric coordinates
                    float[] coords = WorldPosition(x, y, z);

                    CreateSpace(layers[z], new Vector3(coords[0], coords[1], coords[2]), x, y, z, game.Grid.GetLength(2));
                }
            }
        }

        this.layers = layers;
    }
 
    private float[] WorldPosition(int x, int y, int z) {
        float xCoord = (float) (y * 0.5 + x * 0.5);
        float yCoord = (float) (y * 0.25 - x * 0.25 + z * 0.5);
        float zCoord = (float) (yCoord - z);
        return new float[] {xCoord, yCoord, zCoord};
    }

    private int[] IsometricPosition(float x, float y, float z) {
        float newX = x - (2 * y);
        int xCoord = (int) Math.Round(newX);
        int yCoord = (int) Math.Round(4 * y + newX);
        int zCoord = (int) z;

        // int xCoord = (int) Math.Round(x / 0.5);
        // int yCoord = (int) Math.Round((y - z * 0.5) / 0.25);
        // int zCoord = (int) z;
        return new int[] {xCoord, yCoord, zCoord};
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

    public void CreateSpace(GameObject layer, Vector3 pos, int x, int y, int z, int zheight) {
        pos.z = (float) pos.z - zheight;
        GameObject newSpace = Instantiate(spacePrefab);
        newSpace.transform.SetParent(layer.transform, false);
        newSpace.transform.position = newSpace.transform.position + pos;

        newSpace.GetComponent<SortingGroup>().sortingOrder = z + zheight;
        SpaceController spaceController = newSpace.GetComponent<SpaceController>();

        spaceController.UpdateCoordinates(new int[] {x, y, z});
        spaceController.gameController = this;
        
    }

    /// <summary>
    /// Create a pipe game object in the world.
    /// </summary>
    /// <param name="pipe"></param>
    /// <param name="pos"></param>
    public PipeController CreatePipe(Pipe pipe, Vector3 pos, int x, int y, int z) {
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.transform.SetParent(transform, false);
        newPipe.transform.position = newPipe.transform.position + pos;

        newPipe.GetComponent<SortingGroup>().sortingOrder = z;
        
        PipeController pipeController = newPipe.GetComponent<PipeController>();
        pipeController.DrawPipe(pipe);
        pipeController.UpdateCoordinates(new int[] {x, y, z});
        pipeController.gameController = this;

        return pipeController;
        
    }

    public bool MoveRat(Vector3 pos) {
        int[] isopos = IsometricPosition(pos.x, pos.y, pos.z);
        string code = String.Join(",", isopos.Select(i => i.ToString()).ToArray());
        Debug.Log(code);
        return true;
    }










    ///////////// Actions

    public void Selected(PipeController pipeController, int[] coordinates) {
        if (game.Select(coordinates)) {
            selectedLayer = coordinates[2];
            cursor.EnableCursor(pipeController);
            layers[selectedLayer].SetActive(true);
        }
    }

    public void SelectedSpace(int[] coords) {
        if (game.MoveSelectedPipe(coords)) {
            pipeGrid[coords[0], coords[1], coords[2]].DrawPipe(game.Grid[coords[0], coords[1], coords[2]]);
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
            int[] newExits = game.RotateSelected(this.rotationAxis, (int) Direction.Forward);
            cursor.Rotate(newExits);
        }
    }

    public void OnRotateBackward(InputAction.CallbackContext context) {
        if (context.started) {
            int[] newExits = game.RotateSelected(this.rotationAxis, (int) Direction.Backward);
            cursor.Rotate(newExits);
        }
    }

    public bool IsSelected() {
        return game.IsSelected();
    }
}
