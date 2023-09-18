using UnityEngine;

public class SpaceController : MonoBehaviour, Clickable {
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
        spriteRenderer.color = new Color (255, 255, 255, 0); 
    }

    void OnMouseEnter()
    {
        spriteRenderer.color = new Color (255, 255, 255, 255); 
    }

    void OnMouseExit()
    {
        spriteRenderer.color = new Color (255, 255, 255, 0); 
    }

    public void UpdateCoordinates(int[] newCoordinate) {
        coordinate = newCoordinate;
    }

    public void Click() {
        gameController.SelectedSpace(coordinate);
    }
}