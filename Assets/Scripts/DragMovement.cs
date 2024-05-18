using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPosition;
    private InputAction primaryContactAction;

    private Camera mainCamera;
    [SerializeField] private SpriteRenderer mapRenderer;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private bool isTouching;
    private Vector3 dragOrigin;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPosition = playerInput.actions["TouchPosition"];
        primaryContactAction = playerInput.actions["PrimaryContact"];
        mainCamera = Camera.main;

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2;
    }

    private void OnEnable()
    {
        primaryContactAction.started += StartTouch;
        primaryContactAction.canceled += EndTouch;
    }

    private void OnDisable()
    {
        primaryContactAction.started -= StartTouch;
        primaryContactAction.canceled -= EndTouch;
    }

    private void Update()
    {
        if (isTouching)
        {
            Vector3 fingerPosition = Utils.ScreenToWorldPoint(mainCamera, touchPosition.ReadValue<Vector2>());
            Vector3 difference = dragOrigin - fingerPosition;

            mainCamera.transform.position = ClampCamera(mainCamera.transform.position + difference);
        }
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        isTouching = true;
        dragOrigin = Utils.ScreenToWorldPoint(mainCamera, touchPosition.ReadValue<Vector2>());
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        isTouching = false;
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float minX = mapMinX + cameraWidth;
        float maxX = mapMaxX - cameraWidth;
        float minY = mapMinY + cameraHeight;
        float maxY = mapMaxY - cameraHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Nos aseguramos que se vea el mapa
        return new Vector3(newX, newY, -20f);
    }
}
