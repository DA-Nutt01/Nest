using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    // The speed the cam pans around the scene
    public float panSpeed = 20.0f;
    // How close the mouse must be to the border of the screen to pan the camera that way
    public float panBorderThickness = 10.0f;
    // Limit of how far on the x & z the cam can move in the word
    public Vector2 panLimit;
    // 
    public float scrollSpeed = 20.0f;
    public float minYPos = 20.0f;
    public float maxYPos = 120.0f;

    void Update()
    {
        // Cache current pos of the cam
        Vector3 pos = transform.position;

        // Forward Movement
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        // Left Movement
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Backward Movement
        if (Input.GetKey("s") || Input.mousePosition.y <=panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        // Right Movement
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        // Clamp the cam's movement within range of the panLimit 
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minYPos, maxYPos);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
