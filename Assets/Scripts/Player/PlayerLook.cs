using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float maxRotation;

    [SerializeField] private float mouseSensX;
    [SerializeField] private float mouseSensY;
    private Vector2 mouseInput;
    private Vector2 cameraMovement;
    private Vector2 rotation;

    public void SetMouseInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        cameraMovement.x = mouseInput.x * Time.deltaTime * mouseSensX;
        cameraMovement.y = mouseInput.y * Time.deltaTime * mouseSensY;

        rotation.x -= cameraMovement.y;
        rotation.y += cameraMovement.x;

        rotation.x = Mathf.Clamp(rotation.x, -maxRotation, maxRotation);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
    }
}
