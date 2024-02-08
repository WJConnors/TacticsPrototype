using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    readonly float bobbingAmount = 0.05f; // How high the object moves
    readonly float bobbingSpeed = 3f; // How fast the object moves up and down

    private float originalY; // Original Y position of the object

    void Start()
    {
        originalY = transform.position.y; // Store the original Y position
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;

        // Update the object's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
