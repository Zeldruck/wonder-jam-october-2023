using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float maxRotation;

    [SerializeField] private float mouseSensX;
    [SerializeField] private float mouseSensY;
    [SerializeField] private float stickSensX;
    [SerializeField] private float stickSensY;
    private Vector2 lookInput;
    private Vector2 cameraMovement;
    private Vector2 rotation;
    private bool isStick = false;
    public void SetLookInput(Vector2 value)
    {
        lookInput = value;

    }

    public void SetIsStick(bool isStick)
    {
        this.isStick = isStick;
    }

    private void Update()
    {
        if (!isStick)
        {
            cameraMovement.x = lookInput.x * Time.deltaTime * mouseSensX;
            cameraMovement.y = lookInput.y * Time.deltaTime * mouseSensY;
        }
        else
        {
            cameraMovement.x = lookInput.x * Time.deltaTime * stickSensX;
            cameraMovement.y = lookInput.y * Time.deltaTime * stickSensY;

        }

        rotation.x -= cameraMovement.y;
        rotation.y += cameraMovement.x;

        rotation.x = Mathf.Clamp(rotation.x, -maxRotation, maxRotation);

        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
    }
}
