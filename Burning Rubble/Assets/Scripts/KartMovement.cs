using UnityEngine;
using UnityEngine.InputSystem;

public class KartMovement : MonoBehaviour
{
    [SerializeField] private float accelerationMultiplier;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float brakingMultiplier;
    private Vector2 moveDirection;
    private float currAcceleration;
    private float currBraking;
    private InputAction moveAction;
    private InputAction brakeAction;
    private InputAction accelerateAction;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        brakeAction = InputSystem.actions.FindAction("Brake");
        accelerateAction = InputSystem.actions.FindAction("Accelerate");
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // read in move input
        moveDirection = moveAction.ReadValue<Vector2>().normalized;
        currAcceleration = accelerateAction.ReadValue<float>();
        currAcceleration *= accelerationMultiplier;
        currBraking = brakeAction.ReadValue<float>();
        currBraking *= brakingMultiplier;
    }

    void FixedUpdate()
    {
        // flip steering direction for reversing
        if (moveDirection.y < 0f)
        {
            moveDirection.x *= -1;
        }

        // steering
        float speedFactor = rb.linearVelocity.magnitude / maxSpeed;
        Quaternion turnValue = Quaternion.Euler(0f, moveDirection.x * turnSpeed * speedFactor, 0f);
        rb.MoveRotation(rb.rotation * turnValue);

        // eliminate sideways velocity resulting from steering
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = 0;

        // convert to world velocity
        rb.linearVelocity = transform.TransformDirection(localVel);

        // kart acceleration
        if (currBraking == 0)
        {
            rb.AddRelativeForce(new Vector3(0f, 0f, moveDirection.y) * currAcceleration, ForceMode.Acceleration);
        }
        else
        {
            rb.AddRelativeForce(-rb.linearVelocity.normalized * currBraking, ForceMode.Acceleration);
        }

        // caps acceleration to maxSpeed 
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    //Temporary Check for destruction, should probably be changed to a spherecast check
    private void OnTriggerStay(Collider other)
    {
        I_Destructible destructible = other.gameObject.GetComponent<I_Destructible>();
        if (destructible != null) destructible.DestroyMe(this.gameObject, this.gameObject);
    }
}


// old code we could need later can go here
// prevent reversing on forward movement
/*  if (localVel.z < 0f && moveDirection.y > 0f)
    {
        Debug.Log("Flip movement direction");
        moveDirection.y *= -1;
    }*/