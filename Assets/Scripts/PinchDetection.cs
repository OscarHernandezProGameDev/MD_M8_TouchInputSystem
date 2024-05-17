using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PinchDetection : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 10;
    [SerializeField] private float maxZoomIn = 2;
    [SerializeField] private float maxZoomOut = 10;

    private PlayerInput playerInput;
    private InputAction primaryContactAction;
    private InputAction secundayContactAction;
    private InputAction primaryFingerPositionAction;
    private InputAction secondaryFingerPositionAction;

    private Coroutine zoomCoroutine;
    private Camera mainCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        primaryContactAction = playerInput.actions["PrimaryTouchContact"];
        secundayContactAction = playerInput.actions["SecundaryTouchContact"];
        primaryFingerPositionAction = playerInput.actions["PrimaryFingerPosition"];
        secondaryFingerPositionAction = playerInput.actions["SecundaryFingerPosition"];
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        primaryContactAction.canceled += ZoomEnd;
        secundayContactAction.started += ZoomStart;
        secundayContactAction.canceled += ZoomEnd;
    }

    private void OnDisable()
    {
        primaryContactAction.canceled -= ZoomEnd;
        secundayContactAction.started -= ZoomStart;
        secundayContactAction.canceled -= ZoomEnd;
    }

    private void ZoomStart(InputAction.CallbackContext context)
    {
        zoomCoroutine = StartCoroutine(ZoomDetector());
    }

    private void ZoomEnd(InputAction.CallbackContext context)
    {
        StopCoroutine(zoomCoroutine);
    }

    private IEnumerator ZoomDetector()
    {
        float previousDistance = Vector2.Distance(primaryFingerPositionAction.ReadValue<Vector2>(), secondaryFingerPositionAction.ReadValue<Vector2>());
        float distance = 0;

        while (true)
        {
            distance = Vector2.Distance(primaryFingerPositionAction.ReadValue<Vector2>(), secondaryFingerPositionAction.ReadValue<Vector2>());

            float targetZoom = mainCamera.orthographicSize;

            // Para 3D
            Vector3 targetZoom3D = mainCamera.transform.position;
            targetZoom3D.z -= 1;
            // -----

            cameraSpeed = Mathf.Abs(distance - previousDistance);
            //zoom out
            if (distance < previousDistance && mainCamera.orthographicSize < maxZoomOut)
            {
                targetZoom += 1f;
            }
            //zoom in
            else if (distance > previousDistance && mainCamera.orthographicSize > maxZoomIn)
            {

                targetZoom -= 1f;
            }

            targetZoom = Mathf.Clamp(targetZoom, maxZoomIn, maxZoomOut);
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * cameraSpeed);

            // Para 3D usar:
            //Vector3.Slerp .-....

            previousDistance = distance;

            yield return null;
        }
    }
}
