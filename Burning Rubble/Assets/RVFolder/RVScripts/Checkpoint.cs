using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // This script must be attached to the trigger zone you wish to make a checkpoint or lap point (lap point must be value 0).
    // If it is a start/finish lap, check _isLapPoint in inspector.
    // Label each checkpoint with a value in _checkpointPlacement (starting from 1~N where N is the number of checkpoints)

    [SerializeField] private int _checkpointPlacement;
    [SerializeField] private bool _isLapPoint;
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
        if (other.transform.tag.Equals("Player"))
        {
            CheckpointDetection _checkDetect = other.GetComponent<CheckpointDetection>();
            LapManager _lapManager = FindFirstObjectByType<LapManager>();

            // Set current checkpoint
            _checkDetect._currCheckpoint = _checkpointPlacement;
            // Changes checkpoint placement (teleport location)
            Debug.Log("Current Checkpoint Placement: " + _checkpointPlacement);

            if (_hasPassed == false)
            {
                if (!_isLapPoint)
                {
                    // Deprecated
                    // Old ver of the checkpoint system; reworking into incremental
                    // Keeping this here in case we wanna do smth with it
                    // Updating checkpoint system
                    /*if (_tempMove._checkpointCount >= 0)
                    {
                        _tempMove._checkpointCount = GetCheckpoint();
                    }*/

                    // Incremental checkpoint system
                    // Increments checkpoint (determines who's in first)
                    if (!_hasPassed)
                    {
                        _checkDetect._checkpointCount++;
                        Debug.Log("Curr Checkpoint Count: " + _checkDetect._checkpointCount);
                        // Prevents passing through the same checkpoint
                        _hasPassed = true;
                        Debug.Log("Kart has passed");
                    }
                }
                else if (_checkDetect._checkpointCount >= _lapManager.RequirementReturn())
                {
                    _checkDetect._lapCount++;
                    _checkDetect._checkpointCount = 0;
                    _lapManager.DisableHasPassed();
                }
            }
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