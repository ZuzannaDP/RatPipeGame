using System.Collections;
using System.Collections.Generic;
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
    private int[] facingSide;
    private (Vector3 xvector, Vector3 yvector) directionVectors;
    private bool isMoving = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateDirection(int[] facingSide) {
        this.facingSide = facingSide;

        if (facingSide[2] == 1) {
            spriteRenderer.sprite = spriteSE;
            directionVectors = (Vector3.right, Vector3.down);
        } else if (facingSide[3] == 1) {
            spriteRenderer.sprite = spriteNW;
            directionVectors = (Vector3.left, Vector3.up);
        } else if (facingSide[5] == 1) {
            spriteRenderer.sprite = spriteNE;
            directionVectors = (Vector3.right, Vector3.up);
        } else if (facingSide[0] == 1) {
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
            UpdateDirection(Pipe.Rotation(facingSide, (int) Axis.Zaxis, -1));
        }
    }
    
    public void OnTurnRight(InputAction.CallbackContext context) {
        if (context.started) {
            UpdateDirection(Pipe.Rotation(facingSide, (int) Axis.Zaxis, 1));
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isMoving) {
            transform.position += directionVectors.xvector * currentxspeed;
            transform.position += directionVectors.yvector * currentyspeed;
            gameController.MoveRat(transform.position);
        }
    }
}