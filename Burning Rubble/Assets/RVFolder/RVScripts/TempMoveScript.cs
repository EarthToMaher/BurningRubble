using UnityEngine;
using UnityEngine.InputSystem;

public class TempMoveScript : MonoBehaviour
{
    public int _lapCount = 0;
    public bool _inCheckpointTrigger = false;


    // Everything below this honestly is just a temp movement script. Above it will be values associated to Lap Checking.
    private PlayerInput playerInput;
    private InputAction moveAction;

    public float moveSpeed = 5f;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0f, input.y);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        // In the actual game, we can create an array of the checkpoints, and if the checkpoint the player is at is higher
        // Than their previous one, they can't increase their checkpoint.
        // (Basically, we can set their checkpoint number to whatever number is in the array.)
        _inCheckpointTrigger = true;
        Debug.Log("Checkpoint trigger is: " + _inCheckpointTrigger);
        _lapCount++;
        Debug.Log("Lap Count is: " + _lapCount);
    }

}