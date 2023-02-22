using UnityEngine;

public class SmoothRotate : MonoBehaviour
{
    public float targetRotation = 0f; // The target Z-axis rotation
    public float rotationSpeed = 5f; // The speed at which to rotate the object

    private void Update()
    {
        float zRotation = transform.rotation.eulerAngles.z;

        if (zRotation > 180f)
        {
            zRotation -= 360f;
        }

        // Set the target rotation based on the current Z-axis rotation
        if (zRotation >= -2.5f && zRotation <= 2.5f)
        {
            targetRotation = 0f;
        }
        else if (zRotation < -2.5f)
        {
            targetRotation = -5f;
        }
        else if (zRotation > 2.5f)
        {
            targetRotation = 5f;
        }

        // Rotate the object smoothly towards the target rotation
        float newZRotation = Mathf.MoveTowards(zRotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, newZRotation);
    }
}
