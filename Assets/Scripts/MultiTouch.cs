using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiTouch : MonoBehaviour
{
    private bool onThreeTouches, ableToTMove;
    private int count;
    [SerializeField] private SpriteRenderer[] squaresRenderers;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (count >= 3)
        {
            onThreeTouches = true;
            ableToTMove = true;
        }
        else
            ableToTMove = false;
        CheckTouches();
    }

    public void FirstTouch(InputAction.CallbackContext context)
    {
        if (context.started)
            count++;
        if (context.canceled)
            count--;
    }

    public void SecondTouch(InputAction.CallbackContext context)
    {
        if (context.started)
            count++;
        if (context.canceled)
            count--;
    }

    public void ThirdTouch(InputAction.CallbackContext context)
    {
        if (context.started)
            count++;
        if (context.canceled)
            count--;
    }

    public void FirstMovement(InputAction.CallbackContext context)
    {
        if (ableToTMove)
        {
            squaresRenderers[0].transform.position = Utils.ScreenToWorldPoint(mainCamera, context.ReadValue<Vector2>());
        }
    }

    public void SecondMovement(InputAction.CallbackContext context)
    {
        if (ableToTMove)
        {
            squaresRenderers[1].transform.position = Utils.ScreenToWorldPoint(mainCamera, context.ReadValue<Vector2>());
        }
    }

    public void ThirdMovement(InputAction.CallbackContext context)
    {
        if (ableToTMove)
        {
            squaresRenderers[2].transform.position = Utils.ScreenToWorldPoint(mainCamera, context.ReadValue<Vector2>());
        }
    }

    private void CheckTouches()
    {
        if (onThreeTouches)
        {
            foreach (var sprite in squaresRenderers)
            {
                if (sprite.color != Color.green)
                    sprite.color = Color.green;
            }
            onThreeTouches = false;
        }
    }
}
