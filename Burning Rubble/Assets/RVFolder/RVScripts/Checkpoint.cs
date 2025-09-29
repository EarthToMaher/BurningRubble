using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int _checkpointPlacement;
    public bool _hasPassed;

    public void Start()
    {
        _hasPassed = false;
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

            _tempMove._checkpointCount++;
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
