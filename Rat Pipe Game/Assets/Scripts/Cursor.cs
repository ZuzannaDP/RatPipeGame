using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;
using UnityEngine.Rendering;

public class Cursor : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private GameObject pipeObject;
    private Pipe selectedPipe; 
    private PipeController selectedPipeController;
    

    public void EnableCursor(PipeController pipeController) {
        selectedPipeController = pipeController;
        selectedPipeController.Hide();
        pipeObject.GetComponent<SpriteRenderer>().sprite = selectedPipeController.GetSpriteRenderer.sprite;
        Update();
        pipeObject.SetActive(true);
    }

    public void DisableCursor() {
        pipeObject.SetActive(false);
        selectedPipe = null;
    }

    private void Update() {
        // Set the position to the mouse
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 newPos = cam.ScreenToWorldPoint(mousePosition); 

        rectTransform.anchoredPosition = new Vector2(newPos.x, newPos.y);
    }

    public void DisplayPipe(Pipe pipe, int sortOrder) {
        string code = String.Join(",", pipe.Exits.Select(i => i.ToString()).ToArray());

        pipeObject.GetComponent<SortingGroup>().sortingOrder = sortOrder;
        pipeObject.GetComponent<SpriteRenderer>().sprite = SpriteManager.PipeSprites[code];
        pipeObject.GetComponent<PipeController>().UpdateCollider();
    }

    public void IncreaseSortOrder() {
        pipeObject.GetComponent<SortingGroup>().sortingOrder ++;
    }

    public void Cancel() {
        selectedPipeController.UnHide();
    }
}
