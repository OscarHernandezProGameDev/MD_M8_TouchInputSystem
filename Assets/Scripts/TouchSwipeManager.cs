using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchSwipeManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction primaryContactAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        primaryContactAction = playerInput.actions["PrimaryContact"];
    }
}
