using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeController : MonoBehaviour, Clickable {
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer => spriteRenderer;
    private int[] coordinate;

    public GameController gameController;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        if (!gameController.IsSelected()) {
            spriteRenderer.color = new Color (1, 0, 0, 1); 
        }
    }

    void OnMouseExit()
    {
        if (!gameController.IsSelected()) {
            spriteRenderer.color = new Color (255, 255, 255, 255); 
        }
    }

    public void Click() {
        gameController.Selected(this, coordinate);
    }

    public void UpdateCollider() {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void UpdateSprite() {

    }

    public void UpdateCoordinates(int[] newCoordinate) {
        coordinate = newCoordinate;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void UnHide() {
        spriteRenderer.color = new Color (255, 255, 255, 255); 
        gameObject.SetActive(true);
    }
}