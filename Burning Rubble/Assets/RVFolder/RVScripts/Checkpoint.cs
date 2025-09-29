using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int _checkpointPlacement;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered Checkpoint " + _checkpointPlacement + " collider");
        if (other.transform.tag.Equals("Player"))
        {
            // Testing purposes, actual game we take the kart script
            TempMoveScript _tempMove = other.GetComponent<TempMoveScript>();

            // Deprecated until further notice, keeping it as a remnant
            // in case we wanna reuse it.
            // old checkpoint system (updating system)
            /*if (_tempMove._checkpointCount >= 0)
            {
                _tempMove._checkpointCount = GetCheckpoint();
            }*/

            _tempMove._checkpointCount = _tempMove._checkpointCount++;
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
