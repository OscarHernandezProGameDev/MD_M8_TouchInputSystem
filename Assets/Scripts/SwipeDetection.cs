using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private TouchSwipeManager touchSwipeManager;

    [SerializeField] private float minimumDistance = 2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.9f;
    [SerializeField] private GameObject trail;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private Coroutine trailCoroutine;

    private void Awake()
    {
        touchSwipeManager = TouchSwipeManager.tMSharedInstance;
    }

    private void OnEnable()
    {
        touchSwipeManager.OnStartTouch += SwipeStart;
        touchSwipeManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        touchSwipeManager.OnStartTouch -= SwipeStart;
        touchSwipeManager.OnEndTouch -= SwipeEnd;
    }

    void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        trailCoroutine = StartCoroutine(Trail());
    }

    IEnumerator Trail()
    {
        while (true)
        {
            trail.transform.position = touchSwipeManager.PrimaryVector();
            yield return null;
        }
    }

    void SwipeEnd(Vector2 position, float time)
    {
        trail.SetActive(false);
        StopCoroutine(trailCoroutine);
        endPosition = position;
        endTime = time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(startPosition, endPosition) >= minimumDistance && (endTime - startTime) <= maximumTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;

            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction2D)
    {
        if (Vector2.Dot(Vector2.up, direction2D) > directionThreshold)
            Debug.Log("Swipe Up");
        else if (Vector2.Dot(Vector2.down, direction2D) > directionThreshold)
            Debug.Log("Swipe Down");
        else if (Vector2.Dot(Vector2.left, direction2D) > directionThreshold)
            Debug.Log("Swipe Left");
        else if (Vector2.Dot(Vector2.right, direction2D) > directionThreshold)
            Debug.Log("Swipe Right");
    }
}
