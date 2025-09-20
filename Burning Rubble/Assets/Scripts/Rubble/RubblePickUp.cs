using UnityEngine;

public class RubblePickUp : MonoBehaviour
{
    private MeshRenderer rend;
    private Collider coll;
    private void Awake()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<Collider>();
        SetObjectActive(false);
    }

    public void SetObjectActive(bool state)
    {
        rend.enabled = state;
        coll.enabled = state;
    }
}
