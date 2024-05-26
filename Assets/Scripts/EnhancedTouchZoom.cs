using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class EnhancedTouchZoom : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed = 10;
    [SerializeField] private float maxZoomIn = 2;
    [SerializeField] private float maxZoomOut = 10;

    private Coroutine zoomCoroutine;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began && Touch.activeTouches.Count > 1)
            {
                zoomCoroutine = StartCoroutine(ZoomDetector(Touch.activeTouches[0], Touch.activeTouches[1]));
            }
            else if (touch.phase == TouchPhase.Ended && zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);
        }
    }

    private IEnumerator ZoomDetector(Touch firstTouch, Touch secondTouch)
    {
        float previousDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);
        float distance = 0;

        while (true)
        {
            distance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);

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

    void OnDisable()
    {
        //touchPress.performed -= TouchPress;
        //Touch.onFingerDown -= FingerDown;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }
}
