using UnityEngine;

public class DestructibleBlock : MonoBehaviour, I_Destructible
{
    
    //[Tooltip("Represents the amount of damage taken when crashing into this block")]
    //[SerializeField] private int hp = 5;
    [Tooltip("How much rubble the instigator gains")]
    //[SerializeField] private int rubble = 5;
    //[Tooltip("Prefab for the rubble chunk that spawns off of this")]
    [SerializeField] private GameObject rubblePickUp;
    [Tooltip("Number of rubble pick ups that spawn")]
    [SerializeField] private int numOfPickUps;
    private MeshRenderer rend;
    private Collider coll;
    private ReenableManager rm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<Collider>();
        rm = FindFirstObjectByType<ReenableManager>();
    }

    /// <summary>
    /// Implementable function for "destroying" this object
    /// </summary>
    /// <param name="instigator">The entity that caused this to occur (i.e., players, world).</param>
    /// <param name="cause">The physical thing causing the destruction (i.e., kart, item)</param>
    public void DestroyMe(GameObject instigator, GameObject cause)
    {
        //TODO: Deal damage to the cause
        //TODO: Instigator gains rubble
        SetObjectActive(false);
        rm.AddToBatch(this);
        for(int i = 0; i < numOfPickUps; i++)
        {
            GameObject rubbleObj = Instantiate(rubblePickUp, transform.position, Quaternion.identity);
        }
    }

    [ContextMenu("DestroyMe")]
    public void DestroyTest()
    {
        DestroyMe(this.gameObject, this.gameObject);
    }

    public void SetObjectActive(bool state)
    {
        rend.enabled = state;
        coll.enabled = state;
    }
}
