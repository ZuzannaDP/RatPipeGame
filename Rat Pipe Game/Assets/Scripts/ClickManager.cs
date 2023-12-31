using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    private Camera mainCamera;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnSelect(InputAction.CallbackContext context) {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        Clickable clicker = rayHit.collider.GetComponent<Clickable>();

        if (clicker != null) {
            clicker.Click();
        }
    }
}
