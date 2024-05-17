using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchBasicManager : MonoBehaviour
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
        playerInput = GetComponent<PlayerInput>();
        touchBasicMap = playerInput.actions.FindActionMap("TouchBasic");
        touchPosition = touchBasicMap["TouchPosition"];
        touchPress = touchBasicMap["TouchPress"];
    }

    void OnEnable()
    {
        touchPress.performed += TouchPress;
    }

    void OnDisable()
    {
        touchPress.performed -= TouchPress;
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
