using UnityEngine;
using UnityEngine.InputSystem;

public class KartMovement : MonoBehaviour
{
    [SerializeField] private float accelerationMultiplier;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float brakingMultiplier;
    [SerializeField] private float reverseMultiplier = 15;
    private Vector2 moveDirection;
    private float currAcceleration;
    private float currReverse;
    private float currBraking;
    private InputAction moveAction;
    private InputAction reverseAction;
    private InputAction accelerateAction;
    private InputAction brakeAction;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        reverseAction = InputSystem.actions.FindAction("Reverse");
        accelerateAction = InputSystem.actions.FindAction("Accelerate");
        brakeAction = InputSystem.actions.FindAction("Brake");
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // read in move input
        moveDirection = moveAction.ReadValue<Vector2>().normalized;
        currAcceleration = accelerateAction.ReadValue<float>();
        currAcceleration *= accelerationMultiplier;
        currReverse = reverseAction.ReadValue<float>();
        currReverse *= reverseMultiplier;
        currBraking = brakeAction.ReadValue<float>();
        currBraking *= brakingMultiplier;
    }

    void FixedUpdate()
    {
        // flip steering direction for reversing
        //if (moveDirection.y < 0f)
        //{
           // moveDirection.x *= -1;
        //}

        // steering
        float speedFactor = rb.linearVelocity.magnitude / maxSpeed;
        Quaternion turnValue = Quaternion.Euler(0f, moveDirection.x * turnSpeed * speedFactor, 0f);
        rb.MoveRotation(rb.rotation * turnValue);

        // eliminate sideways velocity resulting from steering
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = 0;

        // convert to world velocity
        rb.linearVelocity = transform.TransformDirection(localVel);
        Debug.Log(currReverse);
        // kart acceleration
        if (currBraking != 0)
        {
            // Step 1: Get current velocity in local space
            Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.linearVelocity);

            // Step 2: Modify the local Z velocity
            localVelocity.z = Mathf.Abs(localVelocity.z) - currBraking;
            if (localVelocity.z < 0) localVelocity.z = 0;

            // Step 3: Convert modified local velocity back to world space
            rb.linearVelocity = rb.transform.TransformDirection(localVelocity);
        }
        else if (currAcceleration == 0)
        {
            rb.AddRelativeForce(new Vector3(0f, 0f, -1f) * currReverse, ForceMode.Acceleration);
        }
        else
        {
            //moveDirection.x *= -1;
            rb.AddRelativeForce(new Vector3(0f, 0f, 1f) * currAcceleration, ForceMode.Acceleration);
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