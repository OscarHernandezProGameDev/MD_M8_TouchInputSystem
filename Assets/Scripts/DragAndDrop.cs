using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPosition;
    private Camera mainCamera;
    [SerializeField] private float dragPhysicsSpeed = 10;
    [SerializeField] private float dragSpeed = .5f;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
    private Vector3 velocity = Vector3.zero;
    private Coroutine coroutine;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPosition = playerInput.actions["TouchPosition"];
        mainCamera = Camera.main;
    }

    public void FingerPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("Draggable"))
                {
                    coroutine = StartCoroutine(DragUpdate(hit.collider.gameObject, context.ReadValue<float>()));
                }
            }
        }

        if (context.canceled)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private IEnumerator DragUpdate(GameObject touchObject, float isTounching)
    {
        float initialDistance = Vector3.Distance(touchObject.transform.position, mainCamera.transform.position);
        touchObject.TryGetComponent<Rigidbody>(out var rb);

        while (isTounching != 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(touchPosition.ReadValue<Vector2>());

            if (rb != null)
            {
                Vector3 direction = ray.GetPoint(initialDistance) - touchObject.transform.position;
                rb.velocity = direction * dragPhysicsSpeed;

                yield return waitForFixedUpdate;
            }
            else
            {
                touchObject.transform.position = Vector3.SmoothDamp(touchObject.transform.position, ray.GetPoint(initialDistance), ref velocity, dragSpeed);

                yield return null;
            }
        }
    }
}
