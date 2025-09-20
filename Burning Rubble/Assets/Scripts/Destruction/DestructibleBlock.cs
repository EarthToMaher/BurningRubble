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
    private RubblePickUp[] pickUps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(numOfPickUps>0) pickUps = new RubblePickUp[numOfPickUps];
        rend = gameObject.GetComponent<MeshRenderer>();
        coll = gameObject.GetComponent<Collider>();
        rm = FindFirstObjectByType<ReenableManager>();
        for (int i = 0; i < numOfPickUps; i++)
        {
            GameObject rubbleObj = Instantiate(rubblePickUp, transform.position, Quaternion.identity);
            pickUps[i] = rubbleObj.GetComponent<RubblePickUp>();
        }
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
        //TODO: Particle Effects
        SetObjectActive(false);
        rm.AddToBatch(this);
        if(numOfPickUps>0) foreach (RubblePickUp pickUp in pickUps) pickUp.SetObjectActive(true);
    }

    public void RepairMe()
    {
        //TODO: Repair Animation
        if(numOfPickUps>0)foreach (RubblePickUp pickUp in pickUps) pickUp.SetObjectActive(false);
        SetObjectActive(true);
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
