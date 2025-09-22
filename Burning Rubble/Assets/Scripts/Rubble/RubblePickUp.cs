using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class RubblePickUp : MonoBehaviour
{
    //Instance Variables Set Dynamically
    private MeshRenderer rend;
    private Collider coll;
    private Rigidbody rb;
    [SerializeField] private float luanchHori;
    [SerializeField] private float launchVert;
    private Vector3 startingPos;

    //Sets our instance variables and sets our object active state to false
    private void Awake()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<Collider>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        startingPos = transform.position;
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
        rb.useGravity = state;
    }

    public void Spawn()
    {
        SetObjectActive(true);
        LaunchObject();
    }

    public void Despawn()
    {
        SetObjectActive(false);
        rb.linearVelocity = Vector3.zero;
        transform.position = startingPos;
    }

    public void LaunchObject()
    {
        rb.AddForce(new Vector2(luanchHori, launchVert), ForceMode.Impulse);
    }


}
