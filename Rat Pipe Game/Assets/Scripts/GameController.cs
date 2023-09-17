using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering;
using System;
using System.Linq;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    // Tilemap for pipes
    [SerializeField]
    private Tilemap pipes;

    // Game logic and progresss
    private Game game;

    // Test level
    public LevelData currentLevel;

    // Prefabs
    public GameObject pipePrefab;
    public GameObject spacePrefab;
    public GameObject layerPrefab;

    [SerializeField]
    private Cursor cursor;
    private int[] selectedObjectCoordinates = null;
    private int selectedLayer = 0;
    private GameObject[] layers;

    void Awake() {
        game = LevelManager.LoadLevel(currentLevel);
        SpriteManager.LoadSprites();

        DisplayGame();

        DisplaySpaces();
    }

    /// <summary>
    /// Display the grid in isometric coordinates.
    /// </summary>
    public void DisplayGame() {
        for (int x = 0; x < game.Grid.GetLength(0); x++) {
            for (int y = 0; y < game.Grid.GetLength(1); y++) {
                for (int z = 0; z < game.Grid.GetLength(2); z++) {
                    // Calculate the isometric coordinates
                    float[] coords = IsometricCoords(x, y, z);
                    if (game.Grid[x,y,z] != null) {
                        CreatePipe(game.Grid[x, y, z], new Vector3(coords[0], coords[1], coords[2]), x, y, z);
                    }
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
                    float[] coords = IsometricCoords(x, y, z);
                    CreateSpace(layers[z], new Vector3(coords[0], coords[1], coords[2]), x, y, z, game.Grid.GetLength(2));
                }
            }
        }

        this.layers = layers;
    }
 
    private float[] IsometricCoords(int x, int y, int z) {
        float xCoord = (float) (y * 0.5 + x * 0.5);
        float yCoord = (float) (y * 0.25 - x * 0.25 + z * 0.5);
        float zCoord = (float) (yCoord - z);
        return new float[] {xCoord, yCoord, zCoord};
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
    public void CreatePipe(Pipe pipe, Vector3 pos, int x, int y, int z) {
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.transform.SetParent(transform, false);
        newPipe.transform.position = newPipe.transform.position + pos;

        string code = String.Join(",", pipe.Exits.Select(i => i.ToString()).ToArray());

        newPipe.GetComponent<SortingGroup>().sortingOrder = z;
        newPipe.GetComponent<SpriteRenderer>().sprite = SpriteManager.PipeSprites[code];
        PipeController pipeController = newPipe.GetComponent<PipeController>();

        pipeController.UpdateCollider();
        pipeController.UpdateCoordinates(new int[] {x, y, z});
        pipeController.gameController = this;
        
    }

    public void Selected(PipeController pipeController, int[] coordinates) {
        if (!IsSelected()) {
            selectedObjectCoordinates = coordinates;
            selectedLayer = coordinates[2];
            cursor.EnableCursor(pipeController);
            layers[selectedLayer].SetActive(true);
        }
    }

    public void OnDeselect() {
        layers[selectedLayer].SetActive(false);
        selectedObjectCoordinates = null;
        cursor.OnDeselect();
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

    public bool IsSelected() {
        return selectedObjectCoordinates != null;
    }

    public bool IsMovable() {
        return false;
    }

    public void RotateSelected() {
        // cursor.Rotate();
    }
}
