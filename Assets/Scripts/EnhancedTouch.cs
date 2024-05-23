using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class EnhancedTouch : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputActionMap touchBasicMap;
    private InputAction touchPosition;
    private InputAction touchPress;
    [SerializeField] private GameObject player;

    public void QuitGame()
    {
        Application.Quit();
    }

    void Awake()
    {
        //playerInput = GetComponent<PlayerInput>();
        //touchBasicMap = playerInput.actions.FindActionMap("TouchBasic");
        //touchPosition = touchBasicMap["TouchPosition"];
        //touchPress = touchBasicMap["TouchPress"];
    }

    void OnEnable()
    {
        //touchPress.performed += TouchPress;
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        TouchSimulation.Enable();
    }

    void OnDisable()
    {
        //touchPress.performed -= TouchPress;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void FingerDown(Finger finger)
    {
        bool isPressed = finger.isActive;

        Debug.Log($"Touch Pressed is: {isPressed}");
    }

    private void TouchPress(InputAction.CallbackContext context)
    {
        // bool isPressed = context.ReadValueAsButton();

        // Debug.Log($"Touch Pressed is: {isPressed}");

        // Vector2 position = touchPosition.ReadValue<Vector2>();
        // Debug.Log($"Touch Position is: {position}");

        Vector3 position = Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());

        position.z = player.transform.position.z;
        player.transform.position = position;
    }
}
