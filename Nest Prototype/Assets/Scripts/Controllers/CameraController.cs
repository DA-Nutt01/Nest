using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    private float currentZoom = 10.0f;
    public float zoomSpeed = 4.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 15.0f;
    private Coroutine followCoroutine;

    public float smoothTime = 0.3f;
    public float maxSpeed = 10f;
    private Quaternion currentRotation;
    private Vector3 currentVelocity;

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed; 
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public IEnumerator FollowTarget(Interactable target)
    {   
        yield return null;

        while(true)
        {
            // Get the desired rotation to look at the target
            Vector3 directionToTarget = target.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);

            // Smoothly interpolate towards the desired rotation using Quaternion.Slerp
            currentRotation = Quaternion.Slerp(currentRotation, targetRotation, smoothTime * Time.deltaTime * maxSpeed);

            // Apply the rotation to the transform using Transform.rotation
            transform.rotation = currentRotation;  

            // Calculate the target position to move towards
            Vector3 targetPosition = target.transform.position + offset;

            // Smoothly move the camera towards the target position using Vector3.SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxSpeed);
            yield return null;
        }

    }

    public void StartFollow(Interactable target)
    {
        followCoroutine = StartCoroutine(FollowTarget(target));
    }

    // Public method to stop the coroutine
    public void StopFollow()
    {
        if (followCoroutine != null)
        {
            StopAllCoroutines();
        }
    }
}
