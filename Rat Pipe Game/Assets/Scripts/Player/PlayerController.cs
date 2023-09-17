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
    private float xspeed = 0.005f;
    private float yspeed = 0.0025f;
    private float currentxspeed;
    private float currentyspeed;
    private (Vector3 xvector, Vector3 yvector) directionVectors;
    private bool isMoving = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateDirection(int[] direction) {
        if (direction[(int) Axis.Xaxis] == 1) {
            spriteRenderer.sprite = spriteSE;
            directionVectors = (Vector3.right, Vector3.down);
        } else if (direction[(int) Axis.Xaxis] == -1) {
            spriteRenderer.sprite = spriteNW;
            directionVectors = (Vector3.left, Vector3.up);
        } else if (direction[(int) Axis.Yaxis] == 1) {
            spriteRenderer.sprite = spriteNE;
            directionVectors = (Vector3.right, Vector3.up);
        } else if (direction[(int) Axis.Yaxis] == -1) {
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

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (isMoving) {
            transform.position += directionVectors.xvector * currentxspeed;
            transform.position += directionVectors.yvector * currentyspeed;
        }
    }
}