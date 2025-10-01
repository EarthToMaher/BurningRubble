using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int _checkpointPlacement;
    public Vector3 _checkpointPosition;
    public bool _hasPassed;

    public void Start()
    {
        _hasPassed = false;
        _checkpointPosition = this.transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered Checkpoint " + _checkpointPlacement + " collider");
        if (other.transform.tag.Equals("Player") && _hasPassed == false)
        {
            // Testing purposes, actual game we take the kart script

            // Deprecated
            // Old ver of the checkpoint system; reworking into incremental
            // Keeping this here in case we wanna do smth with it
            // Updating checkpoint system
            TempMoveScript _tempMove = other.GetComponent<TempMoveScript>();
            /*if (_tempMove._checkpointCount >= 0)
            {
                _tempMove._checkpointCount = GetCheckpoint();
            }*/

            // Increments checkpoint (determines who's in first)
            //_tempMove._checkpointCount++;
            // Changes checkpoint placement (teleport location)
            //_tempMove._currCheckpoint = _checkpointPlacement;
            // Prevents passing through the same checkpoint
            _hasPassed = true;
        }
    }

    // Getter and setter
    public void SetCheckpoint(int checkpointPlacement)
    {
        _checkpointPlacement = checkpointPlacement;
    }

    public int GetCheckpoint()
    {
        return _checkpointPlacement;
    }
}