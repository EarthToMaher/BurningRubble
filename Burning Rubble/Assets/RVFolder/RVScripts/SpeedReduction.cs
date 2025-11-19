using Unity.VisualScripting;
using UnityEngine;

// This can be put on any object as long as it's istrigger is enabled.
// Please feel free to use this on varying sized objects

public class SpeedReduction : MonoBehaviour
{
    public KartMovement _kart;
    public float _MaxOffSpeed;
    public float _SpeedReduceMultiplier;
    private float _MaxSpeedStorage;
    private float _CurrAccelStorage;

    // Gets the kart object
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered collider");
        _kart = other.GetComponent<KartMovement>();
        _MaxSpeedStorage = _kart.ReturnMaxSpeed();
        //_CurrAccelStorage = _kart.GetAccelerateValue();
    }

    // When Kart enters collider, lower max speed and reduce acceleration
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stayed in Collider");
        // Set max speed to half
        _kart.SetMaxSpeed(_MaxOffSpeed);
        // Set max accel to half
        float currAccel = _kart.GetAccelerateValue();
        _kart.SetCurrVelocity(currAccel * _SpeedReduceMultiplier); // Reduce movement by half
        
        Debug.Log("Kart's current max speed is: " + _kart.ReturnMaxSpeed());
    }

    // When Kart leaves collider, set max speed back to normal.
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited collision");
        _kart.SetMaxSpeed(_MaxSpeedStorage);
        Debug.Log("Kart's current max speed is: " + _kart.ReturnMaxSpeed());
    }
}
