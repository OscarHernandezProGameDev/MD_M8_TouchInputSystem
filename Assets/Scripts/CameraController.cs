using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Transform player;
    [SerializeField] private float distance = 15f;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private float maxYAngle = 80f;
    [SerializeField] private float minYAngle = 50f;
    private float rotationY = 0f;

    private void Update()
    {
        Vector2 input = playerInput.actions["Look"].ReadValue<Vector2>();
        float joyX = input.x;
        float joyY = input.y;

        float rotatioX = transform.localEulerAngles.y + joyX * sensitivity;

        rotationY += joyY * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -maxYAngle, minYAngle);

        transform.localEulerAngles = new Vector3(-rotationY, rotatioX, 0);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-rotationY, rotatioX, 0);
        transform.position = player.position + rotation * direction;
    }
}
