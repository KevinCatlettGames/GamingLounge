using System;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    // Rotation speed in degrees per second
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    private bool goRight = true;
    public int timeUntilDirectionSwitch = 5;
    private float currentTime = 0f;

    private void Awake()
    {
        currentTime = timeUntilDirectionSwitch;
    }

    void Update()
    {
        if (goRight)
        {
            // Rotate the object continuously
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(-rotationSpeed * Time.deltaTime);
        }
        
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            goRight = !goRight;
            currentTime = timeUntilDirectionSwitch;
        }
        
    }
}
