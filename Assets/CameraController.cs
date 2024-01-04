using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float panSpeed = 20f;
    float panBorderThickness = 50f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        // Move camera if mouse pointer is near the edges of the screen
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            // Move forward (along Z-axis)
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            // Move backward (along Z-axis)
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            // Move right (along X-axis)
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            // Move left (along X-axis)
            pos.x -= panSpeed * Time.deltaTime;
        }

        // Update camera position
        transform.position = pos;
    }
}
