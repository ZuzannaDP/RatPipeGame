using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Sprite spriteSE;
    [SerializeField]
    private Sprite spriteSW;
    [SerializeField]
    private Sprite spriteNE;
    [SerializeField]
    private Sprite spriteNW;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameController gameController;
    private float xspeed = 0.004f;
    private float yspeed = 0.002f;
    private float currentxspeed;
    private float currentyspeed;
    private Direction facingSide;
    private (Vector3 xvector, Vector3 yvector) directionVectors;
    private bool isMoving = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateDirection(Direction dir) {
        facingSide = dir;

        if (dir.Equals(Direction.SE)) {
            spriteRenderer.sprite = spriteSE;
            directionVectors = (Vector3.right, Vector3.down);
        } else if (dir.Equals(Direction.NW)) {
            spriteRenderer.sprite = spriteNW;
            directionVectors = (Vector3.left, Vector3.up);
        } else if (dir.Equals(Direction.NE)) {
            spriteRenderer.sprite = spriteNE;
            directionVectors = (Vector3.right, Vector3.up);
        } else if (dir.Equals(Direction.SW)) {
            spriteRenderer.sprite = spriteSW;
            directionVectors = (Vector3.left, Vector3.down);
        }
    }

    public void OnMoveForward(InputAction.CallbackContext context) {
        currentxspeed = xspeed;
        currentyspeed = yspeed;

        if (context.started) {
            isMoving = true;
        } 
        
        if (context.canceled) {
            isMoving = false;
        }
    }

    public void OnMoveBackward(InputAction.CallbackContext context) {
        currentxspeed = -xspeed;
        currentyspeed = -yspeed;

        if (context.started) {
            isMoving = true;
        } 
        
        if (context.canceled) {
            isMoving = false;
        }
    }

    public void OnTurnLeft(InputAction.CallbackContext context) {
        if (context.started) {
            UpdateDirection(facingSide.Rotate(Axis.Zaxis, -1));
            SnapToGrid();
        }
    }
    
    public void OnTurnRight(InputAction.CallbackContext context) {
        if (context.started) {
            UpdateDirection(facingSide.Rotate(Axis.Zaxis, 1));
            SnapToGrid();
        }
    }
 
    public void SnapToGrid() {
        transform.position = Position.WorldPosition(gameController.GetPlayerPosition());
    }
 
    public void OnJump(InputAction.CallbackContext context) {
        if (context.started) {
            if (gameController.JumpRat()) {
                transform.position = Position.WorldPosition(gameController.GetPlayerPosition());
            }
        }
    }

    public void OnFall(InputAction.CallbackContext context) {
        if (context.started) {
            if (gameController.FallRat()) {
                transform.position = Position.WorldPosition(gameController.GetPlayerPosition());
            }
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isMoving) {
            Vector3 newPosition = transform.position;
            newPosition += directionVectors.xvector * currentxspeed;
            newPosition += directionVectors.yvector * currentyspeed;

            if (gameController.MoveRat(newPosition)) {
                transform.position = newPosition;

                // Check won
                if (gameController.HasPlayerWon()) {
                    gameController.PlayerWon();
                };
            };
        }
    }
}