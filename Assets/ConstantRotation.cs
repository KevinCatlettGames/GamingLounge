using System;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    // Rotation speed in degrees per second
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);

    void Update()
    {
        // Rotate the object continuously
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
