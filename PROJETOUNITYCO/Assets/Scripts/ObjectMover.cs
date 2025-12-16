using System.Collections;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform mainObject;       // The object whose Z rotation we check
    public Transform objectToMove;     // The object we want to move
    public float moveSpeed = 2f;       // Forward speed
    public float turnMultiplier = 1f;  // How much Z affects turning

    private bool isMoving = false;

    // Call this function from your Select event
    public void StartMoving()
    {
        if (!isMoving)
            StartCoroutine(MoveForward());
    }

    private IEnumerator MoveForward()
    {
        isMoving = true;

        while (isMoving)
        {
            // Get the Z rotation of the main object in euler angles
            float zRotation = mainObject.eulerAngles.z;

            // Convert to -180 to 180 range
            if (zRotation > 180f) zRotation -= 360f;

            // Calculate turn based on Z rotation
            float turnAmount = zRotation * turnMultiplier * Time.deltaTime;

            // Apply turning around Y-axis
            objectToMove.Rotate(0f, turnAmount, 0f);

            // Move object forward
            objectToMove.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    // Optional: Stop movement
    public void StopMoving()
    {
        isMoving = false;
    }
}
