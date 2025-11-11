using UnityEngine;
using System.Collections.Generic;
using MarchingCubesProject;

#if UNITY_EDITOR
using UnityEditor;


[ExecuteInEditMode]
public class WorldVoxelization : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 1f;
    public Color gridColor = Color.gray;
    public bool includeInactive = false;
    public bool fitToRenderers = true;
    public bool fitToColliders = true;

    [Header("Layer Settings")]
    public string targetLayerName = "Voxelize";

    [Header("Cube Placement")]
    public GameObject cubePrefab;       // Optional prefab — if null, will use a built-in cube
    public bool clearOldCubes = true;   // Destroys previously placed cubes before spawning new ones
    public string spawnedParentName = "VoxelGrid";
    public GameObject parent;

    private Bounds sceneBounds;
    private int targetLayer;

    //[Header("Grid Locations")]
    public Vector3[,,] gridLocations = new Vector3[0,0,0];
    public byte[,,] voxelData = new byte[0,0,0];

    public Example marchingCubesScript;

    [ContextMenu("Generate Voxel Grid")]

    public void GenerateVoxelGrid()
    {
        targetLayer = LayerMask.NameToLayer(targetLayerName);
        if (targetLayer < 0)
        {
            Debug.LogWarning($"Layer \"{targetLayerName}\" not found. Please create it in the Tags & Layers settings.");
            return;
        }

        UpdateSceneBounds();

        // Optionally clear previously spawned cubes //does not work as far as I can tell
        if (clearOldCubes)
        {
            var oldParent = GameObject.Find(spawnedParentName);
            if (oldParent != null)
                DestroyImmediate(oldParent);
        }

        // Create parent to hold cubes
        parent = new GameObject(spawnedParentName);
        parent.transform.position = Vector3.zero;

        Vector3 start = new Vector3(
            Mathf.Floor(sceneBounds.min.x / cellSize) * cellSize,
            Mathf.Floor(sceneBounds.min.y / cellSize) * cellSize,
            Mathf.Floor(sceneBounds.min.z / cellSize) * cellSize
        );

        Vector3 end = new Vector3(
            Mathf.Ceil(sceneBounds.max.x / cellSize) * cellSize,
            Mathf.Ceil(sceneBounds.max.y / cellSize) * cellSize,
            Mathf.Ceil(sceneBounds.max.z / cellSize) * cellSize
        );

        // Initialize grid locations array
        gridLocations = new Vector3[Mathf.CeilToInt((end.x - start.x) / cellSize) +2, Mathf.CeilToInt((end.y - start.y) / cellSize) +2, Mathf.CeilToInt((end.z - start.z) / cellSize) +2];
        voxelData = new byte[gridLocations.GetLength(0), gridLocations.GetLength(1), gridLocations.GetLength(2)];
        //Debug.Log("Grid size: " + gridLocations.GetLength(0) + " x " + gridLocations.GetLength(1) + " x " + gridLocations.GetLength(2));
        //Debug.Log("Start X: " + start.x + " End X: " + end.x + "\nStart Y: " + start.y + " End Y: " + end.y + "\nStart Z: " + start.z + " End Z: " + end.z);


        int count = 0;

        List<MeshCollider> mcList = GetMeshCollidersInLayer();

        int a=0, b=0, c=0;
        // Main loop to fill the grid
        for (float x = start.x; x < end.x + 2; x += cellSize)
        {
            for (float y = start.y; y < end.y + 2; y += cellSize)
            {
                for (float z = start.z; z < end.z + 2; z += cellSize)
                {

                    Vector3 cellCenter = new Vector3((x + cellSize / 2f) - cellSize, (y + cellSize / 2f) - cellSize, (z + cellSize / 2f) - cellSize);

                    /*if(a == 0){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x - cellSize, cellCenter.y, cellCenter.z);
                    }else if(a == gridLocations.GetLength(0)-1){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x + cellSize, cellCenter.y, cellCenter.z);
                    }else if(b == 0){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x, cellCenter.y - cellSize, cellCenter.z);
                    }else if(b == gridLocations.GetLength(1)-1){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x, cellCenter.y + cellSize, cellCenter.z);
                    }else if(c == 0){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x, cellCenter.y,  cellCenter.z - cellSize);
                    }else if(c == gridLocations.GetLength(2)-1){
                        gridLocations[a,b,c] = new Vector3(cellCenter.x, cellCenter.y,  cellCenter.z + cellSize);
                    }else{*/
                        gridLocations[a,b,c] = new Vector3(cellCenter.x, cellCenter.y,  cellCenter.z);
                    //}

                    if (IsPointInsideMesh(cellCenter, mcList))
                    {
                        GameObject cube;
                        if (cubePrefab != null)
                            cube = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab);
                        else
                            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        cube.transform.position = cellCenter;
                        cube.transform.localScale = Vector3.one * cellSize;
                        cube.transform.SetParent(parent.transform);
                        cube.GetComponent<BoxCollider>().isTrigger = true;
                        cube.AddComponent<DestructibleBlock>();

                        count++;
                        voxelData[a, b, c] = 1; // Mark voxel as occupied
                    //}else if(gridLocations[a,b,c] == new Vector3(float.NaN, float.NaN, float.NaN)){
                        //voxelData[a, b, c] = 0; // Mark voxel as empty
                    }else{
                        voxelData[a, b, c] = 0; // Mark voxel as empty
                    }
                    c++;
                }
                c = 0;
                b++;
            }
            b = 0;
            a++;
        }

        marchingCubesScript.GenerateMarchingCubesMesh();

        Debug.Log($"Generated {count} cubes inside grid fitting \"{targetLayerName}\" objects.");
    }

    void UpdateSceneBounds()
    {
        bool initialized = false;
        sceneBounds = new Bounds();

        if (fitToRenderers)
        {
            Renderer[] renderers = includeInactive ? FindObjectsOfType<Renderer>(true) : FindObjectsOfType<Renderer>();
            foreach (Renderer r in renderers)
            {
                if (r.gameObject.layer != targetLayer) continue;

                if (!initialized)
                {
                    sceneBounds = r.bounds;
                    initialized = true;
                }
                else
                {
                    sceneBounds.Encapsulate(r.bounds);
                }
            }
        }

        if (fitToColliders)
        {
            Collider[] colliders = includeInactive ? FindObjectsOfType<Collider>(true) : FindObjectsOfType<Collider>();
            foreach (Collider c in colliders)
            {
                if (c.gameObject.layer != targetLayer) continue;

                if (!initialized)
                {
                    sceneBounds = c.bounds;
                    initialized = true;
                }
                else
                {
                    sceneBounds.Encapsulate(c.bounds);
                }
            }
        }

        if (!initialized)
        {
            sceneBounds = new Bounds(Vector3.zero, Vector3.one * 10f);
        }
    }

    bool IsPointInsideMesh(Vector3 point, List<MeshCollider> mcList)
    {
        Debug.Log("I EXIST!!!");
        // tiny overlap sphere at the point
        Collider[] overlaps = Physics.OverlapSphere(point, 0.0001f);

        foreach (var c in overlaps)
        {
            Debug.Log("Overlap with " + c.name);
            foreach (var mc in mcList)
            {
                if (c == mc)
                    return true; // definitely inside
            }
        }
        //int count = 0;

        return false;
    }

    public List<MeshCollider> GetMeshCollidersInLayer()
    {
        List<MeshCollider> mcList = new List<MeshCollider>();
        MeshCollider[] colliders = includeInactive ? FindObjectsOfType<MeshCollider>(true) : FindObjectsOfType<MeshCollider>();
        foreach (MeshCollider mc in colliders)
        {
            if (mc.gameObject.layer != targetLayer) continue;
            mcList.Add(mc);
        }
        Debug.Log("Found " + mcList.Count + " colliders in layer " + targetLayerName);
        return mcList;
    }

    /*void OnDrawGizmos()
    {
        // Draw only if grid has been generated
        if (gridLocations == null || gridLocations.Length == 0)
            return;

        Gizmos.color = gridColor;

        for (int x = 0; x < gridLocations.GetLength(0); x++)
        {
            for (int y = 0; y < gridLocations.GetLength(1); y++)
            {
                for (int z = 0; z < gridLocations.GetLength(2); z++)
                {
                    Vector3 pos = gridLocations[x, y, z];

                    // Skip unused boundary cells
                    //if (float.IsNaN(pos.x))
                        //continue;

                    // Draw cube outline for each valid grid cell
                    Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
                }
            }
        }
    }*/

}

#endif