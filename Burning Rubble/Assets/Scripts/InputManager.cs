using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public MPManager multiplayer;
    private PlayerInput playerInput;
    private bool gameStarted;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        multiplayer = FindFirstObjectByType<MPManager>();
    }

    public void OnJoin()
    {
        //starts game if the join action was triggered by player 1 (this will only trigger once)
        if(!gameStarted && playerInput.playerIndex == 0)
        {
            multiplayer.StartGame();
            gameStarted = true;
        }
    }
}
