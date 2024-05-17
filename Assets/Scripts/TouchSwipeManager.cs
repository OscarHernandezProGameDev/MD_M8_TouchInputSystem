using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

// Que se ejecute el primero
[DefaultExecutionOrder(-1)]
public class TouchSwipeManager : MonoBehaviour
{
    public static TouchSwipeManager tMSharedInstance;

    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    private PlayerInput playerInput;
    private InputAction primaryContactAction;
    private InputAction primaryPositionAction;

    private Camera mainCamera;

    public Vector3 PrimaryVector()
    {
        return Utils.ScreenToWorldPoint(mainCamera, primaryPositionAction.ReadValue<Vector2>());
    }

    private void Awake()
    {
        if (tMSharedInstance == null)
            tMSharedInstance = this;
        else
            Destroy(gameObject);

        playerInput = GetComponent<PlayerInput>();
        primaryContactAction = playerInput.actions["PrimaryContact"];
        primaryPositionAction = playerInput.actions["PrimaryPosition"];
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        primaryContactAction.started += StartTouchPrimary;
        primaryContactAction.canceled += EndTouchPrimary;
    }

    private void OnDisable()
    {
        primaryContactAction.started -= StartTouchPrimary;
        primaryContactAction.canceled -= EndTouchPrimary;
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        var position = primaryPositionAction.ReadValue<Vector2>();

        // hacemos esto por un bug en unity3d que la primera llamada da el vector zero
        if (position == Vector2.zero)
            StartCoroutine(StartTouchDelay(context));
        else
            OnStartTouch?.Invoke(PrimaryVector(), (float)context.startTime);
    }

    private IEnumerator StartTouchDelay(InputAction.CallbackContext context)
    {
        yield return new WaitForEndOfFrame();

        OnStartTouch?.Invoke(PrimaryVector(), (float)context.startTime);
        StopCoroutine(StartTouchDelay(context));
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(PrimaryVector(), (float)context.time);
    }
}
