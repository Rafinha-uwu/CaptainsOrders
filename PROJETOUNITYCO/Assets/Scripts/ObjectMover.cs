using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectMoverPhysics : MonoBehaviour
{
    public Transform helm;
    public float moveSpeed = 2f;
    public float turnMultiplier = 1f;

    private Rigidbody rb;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        float zRotation = helm.eulerAngles.z;
        if (zRotation > 180f) zRotation -= 360f;

        float turn = zRotation * turnMultiplier * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));

        Vector3 forwardMove = rb.transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove);
    }
}
