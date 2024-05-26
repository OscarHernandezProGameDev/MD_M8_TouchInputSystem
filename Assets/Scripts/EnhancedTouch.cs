using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class EnhancedTouch : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public void QuitGame()
    {
        Application.Quit();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        //Touch.onFingerDown += FingerDown;
        TouchSimulation.Enable();
    }

    void Update()
    {
        //Debug.Log(Touch.activeTouches);

        // foreach (var touch in Touch.activeTouches)
        // {
        //     switch (touch.phase)
        //     {
        //         case TouchPhase.None:
        //             Debug.Log("No hay actividad");
        //             break;
        //         case TouchPhase.Began:
        //             Debug.Log("Acabamos de tocar la pantalla con un dedo, hacer algo");
        //             break;
        //         case TouchPhase.Moved:
        //             Debug.Log("Tenemos un touch en movimiento, posicion: " + touch.screenPosition);
        //             break;
        //         case TouchPhase.Ended:
        //             Debug.Log("El toque continuo ha acabado en la posicion: " + touch.screenPosition);
        //             break;
        //         case TouchPhase.Stationary:
        //             Debug.Log("Se está mantenido el toque sin movimiento");
        //             break;
        //         case TouchPhase.Canceled:
        //             Debug.Log("No se llamará esta fase mediante la interacción del usuario");
        //             break;
        //     }
        // }

        var activeTouches = Touch.activeTouches;

        for (var i = 0; i < activeTouches.Count; ++i)
            Debug.Log("Active touch: " + activeTouches[i]);
    }

    private void FingerDown(Finger finger)
    {
        bool isPressed = finger.isActive;

        Debug.Log($"Touch Pressed is: {isPressed}");

        Vector2 fingerPosition = finger.screenPosition;
        Debug.Log($"Touch Position is: {fingerPosition}");

        Vector3 position = Camera.main.ScreenToWorldPoint(fingerPosition);

        position.z = player.transform.position.z;
        player.transform.position = position;
    }

    void OnDisable()
    {
        //touchPress.performed -= TouchPress;
        //Touch.onFingerDown -= FingerDown;
        TouchSimulation.Disable();
        EnhancedTouchSupport.Disable();
    }
}
