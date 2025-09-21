using UnityEngine;

public class RubblePickUp : MonoBehaviour
{
    //Instance Variables Set Dynamically
    private MeshRenderer rend;
    private Collider coll;

    //Sets our instance variables and sets our object active state to false
    private void Awake()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<Collider>();
        SetObjectActive(false);
    }

    /// <summary>
    /// Controls disabling/reenabling rubble pickups
    /// </summary>
    /// <param name="state">Object enabled state.</param>
    public void SetObjectActive(bool state)
    {
        rend.enabled = state;
        coll.enabled = state;
    }
}
