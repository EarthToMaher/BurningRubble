using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GridDrawer : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 1f;
    public Color gridColor = Color.gray;
    public bool includeInactive = false;
    public bool fitToRenderers = true;
    public bool fitToColliders = true;

    [Header("Layer Settings")]
    public string targetLayerName = "Voxelize";

    private Bounds sceneBounds;
    private int targetLayer;

    void OnDrawGizmos()
    {
        targetLayer = LayerMask.NameToLayer(targetLayerName);
        if (targetLayer < 0)
        {
            Debug.LogWarning($"Layer \"{targetLayerName}\" not found. Please create it in the Tags & Layers settings.");
            return;
        }

        UpdateSceneBounds();

        Gizmos.color = gridColor;

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

        // Draw XZ grid lines per Y level (3D-aligned grid)
        for (float y = start.y; y <= end.y; y += cellSize)
        {
            for (float x = start.x; x <= end.x; x += cellSize)
            {
                Vector3 from = new Vector3(x, y, start.z);
                Vector3 to = new Vector3(x, y, end.z);
                Gizmos.DrawLine(from, to);
            }

            for (float z = start.z; z <= end.z; z += cellSize)
            {
                Vector3 from = new Vector3(start.x, y, z);
                Vector3 to = new Vector3(end.x, y, z);
                Gizmos.DrawLine(from, to);
            }
        }
    }

    void UpdateSceneBounds()
    {
        bool initialized = false;
        sceneBounds = new Bounds();

        // Include renderers
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

        // Include colliders
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

        // Fallback if nothing found
        if (!initialized)
        {
            sceneBounds = new Bounds(Vector3.zero, Vector3.one * 10f);
        }
    }
}