using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddCollidersToChildren : MonoBehaviour
{
    private int timeOfLastFrame;
    private List<GameObject> voxels = new List<GameObject>();
    [SerializeField] private float loadTime = 100f;
    private int numOfVoxels;
    private int numActivated = 0;
    [SerializeField] private int loadAmount = 100;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Canvas loadingScreen;

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
        do
        {
            for (int i = 0; i < loadAmount; i++)
            {
                AddBoxCollider(voxels[numActivated]);
                AddDestructibleScript(voxels[numActivated]);
                numActivated++;
                if (numActivated >= voxels.Count) break;
            }
            loadingBar.fillAmount = ((float)numActivated / (float)voxels.Count);
            yield return new WaitForEndOfFrame();
        } while (numActivated < voxels.Count);
        Debug.Log("Finished Loading");
        Destroy(loadingScreen);

    }
}
