using System.Collections.Generic;
using UnityEngine;

public class AddCollidersToChildren : MonoBehaviour
{
    private List<GameObject> voxels = new List<GameObject>();
    [SerializeField] private float loadTime = 100f;
    private int numOfVoxels;
    private int numActivated = 0;
    [SerializeField] private int loadAmount = 100;
    void Awake()
    {
        foreach (Transform childTransform in this.transform)
        {
            //Debug.Log(childTransform);
            voxels.Add(childTransform.gameObject);
        }
        numOfVoxels = voxels.Count;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FinalizeObjects();
        StartCoroutine(LoadObjects());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinalizeObjects()
    {
        foreach(GameObject voxel in voxels)
        {
            AddBoxCollider(voxel);
            //AddDestructibleScript(voxel);
        }
    }

    public void AddBoxCollider(GameObject gameObject)
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    public void AddDestructibleScript(GameObject gameObject)
    {
        gameObject.AddComponent<DestructibleBlock>();
    }
    
    public IEnumerator<WaitForEndOfFrame> LoadObjects()
    {
        for (int i = 0; i < loadAmount; i++)
        {
            AddBoxCollider(voxels[numActivated]);
            AddDestructibleScript(voxels[numActivated]);
            numActivated++;
            if (numActivated >= voxels.Count) break;
        }
        yield return new WaitForEndOfFrame();
        if (numActivated < voxels.Count) StartCoroutine(LoadObjects());
        else Debug.Log("Finished Loading");

    }
}
