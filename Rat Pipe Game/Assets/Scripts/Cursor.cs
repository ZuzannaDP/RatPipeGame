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
        selectedPipeController = null;
    }

    private void Update() {
        // Set the position to the mouse
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 newPos = cam.ScreenToWorldPoint(mousePosition); 

        rectTransform.anchoredPosition = new Vector2(newPos.x, newPos.y);
    }

    public void DisplayPipe(Pipe pipe, int sortOrder) {
        pipeObject.GetComponent<SortingGroup>().sortingOrder = sortOrder;
        Rotate(pipe.Exits);
        pipeObject.GetComponent<PipeController>().UpdateCollider();
    }

    public void Rotate(int[] exits) {
        pipeObject.GetComponent<PipeController>().RotateSprite(exits);
    }

    public void IncreaseSortOrder() {
        Debug.Log(pipeObject.GetComponent<SortingGroup>().sortingOrder);
        pipeObject.GetComponent<SortingGroup>().sortingOrder ++;
    }

    public void DecreaseSortOrder() {
        pipeObject.GetComponent<SortingGroup>().sortingOrder --;
    }

    public void OnDeselect() {
        if (selectedPipeController != null) {
            selectedPipeController.UnHide();
            DisableCursor();
        }
    }
}
