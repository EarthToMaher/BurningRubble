using UnityEngine;
using UnityEngine.InputSystem;

public class KartMovement : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    private Vector2 moveDirection;
    private InputAction moveAction;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // read in move input
        moveDirection = moveAction.ReadValue<Vector2>().normalized;
    }

    void FixedUpdate()
    {
        // steering
        float speedFactor = rb.linearVelocity.magnitude / maxSpeed;
        Quaternion turnValue = Quaternion.Euler(0f, moveDirection.x * turnSpeed * speedFactor, 0f);
        rb.MoveRotation(rb.rotation * turnValue);

        // eliminate sideways velocity resulting from steering
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = 0;

        // prevent reversing on forward movement
        if (localVel.z < 0f && moveDirection.y > 0f)
        {
            Debug.Log("Flip movement direction");
            moveDirection.y *= -1;
        }

        // convert to world velocity
        rb.linearVelocity = transform.TransformDirection(localVel);

        // kart acceleration
        rb.AddRelativeForce(new Vector3(0f, 0f, moveDirection.y) * acceleration, ForceMode.Acceleration);

        // caps acceleration to maxSpeed 
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
