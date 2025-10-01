using TMPro;
using UnityEngine;

public class CheckpointDetection : MonoBehaviour
{
    public int _lapCount = 1;
    public int _checkpointCount = 0;
    public int _currCheckpoint = 0;

    public GameObject _txtCheckpoint;
    public GameObject _txtLapCount;

    // Update is called once per frame
    void Update()
    {
        _txtCheckpoint.GetComponent<TextMeshProUGUI>().text = "Current Checkpoint: " + _currCheckpoint;
        _txtLapCount.GetComponent<TextMeshProUGUI>().text = ("Lap: " + _lapCount + "/3");
    }
}
