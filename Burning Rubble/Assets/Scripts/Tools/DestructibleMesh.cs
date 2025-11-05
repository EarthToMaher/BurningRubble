using UnityEngine;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

using ProceduralNoiseProject;
using Common.Unity.Drawing;
public class DestructibleMesh : MonoBehaviour
{
    public Vector3[,,] voxelPositions;
    public byte[,,] voxelData;
    int size = 32;
    int voxelSize = 1;
    Vector3 contactPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        Vector3 contactPoint = other.ClosestPoint(transform.position);
Vector3 localImpact = transform.InverseTransformPoint(contactPoint);

// Offset so localImpact is relative to voxel grid start
Vector3 gridOrigin = voxelPositions[0,0,0];
Vector3 relativePos = localImpact - gridOrigin;

// Compute indices
int vx = Mathf.Clamp(Mathf.FloorToInt(relativePos.x / voxelSize), 0, voxelData.GetLength(0) - 1);
int vy = Mathf.Clamp(Mathf.FloorToInt(relativePos.y / voxelSize), 0, voxelData.GetLength(1) - 1);
int vz = Mathf.Clamp(Mathf.FloorToInt(relativePos.z / voxelSize), 0, voxelData.GetLength(2) - 1);
            
            if (vx < 0 || vy < 0 || vz < 0 ||
        vx >= voxelData.GetLength(0) ||
        vy >= voxelData.GetLength(1) ||
        vz >= voxelData.GetLength(2))
        return; // outside voxel grid

        voxelData[vx, vy, vz] = 0;
float hitRadiusX = 1.0f;
float hitRadiusY = 1.0f;
float hitRadiusZ = 1.5f;

/*// localImpact is the impact point in voxel local space
for (int x = 0; x < voxelData.GetLength(0); x++)
for (int y = 0; y < voxelData.GetLength(1); y++)
for (int z = 0; z < voxelData.GetLength(2); z++)
{
    if (voxelData[x, y, z] == 0) continue;

    Vector3 voxelPos = voxelPositions[x, y, z];
    Vector3 offset = voxelPos - localImpact;

    if (Mathf.Abs(offset.x) <= hitRadiusX &&
        Mathf.Abs(offset.y) <= hitRadiusY &&
        Mathf.Abs(offset.z) <= hitRadiusZ)
    {
        voxelData[x, y, z] = 0; // Destroy this voxel
    }
}*/
    RebuildMesh();
    }
    public void RebuildMesh()
    {
        Debug.Log("Rebuilding");
        if (voxelData == null || voxelPositions == null)
            return;

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        Vector3[] faceDirs =
        {
            Vector3.forward, Vector3.back,
            Vector3.up, Vector3.down,
            Vector3.right, Vector3.left
        };

        Vector3[,] faceVerts = new Vector3[6, 4]
        {
            { new Vector3(-0.5f, -0.5f,  0.5f), new Vector3( 0.5f, -0.5f,  0.5f), new Vector3( 0.5f,  0.5f,  0.5f), new Vector3(-0.5f,  0.5f,  0.5f) },
            { new Vector3( 0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f,  0.5f, -0.5f), new Vector3( 0.5f,  0.5f, -0.5f) },
            { new Vector3(-0.5f,  0.5f,  0.5f), new Vector3( 0.5f,  0.5f,  0.5f), new Vector3( 0.5f,  0.5f, -0.5f), new Vector3(-0.5f,  0.5f, -0.5f) },
            { new Vector3(-0.5f, -0.5f, -0.5f), new Vector3( 0.5f, -0.5f, -0.5f), new Vector3( 0.5f, -0.5f,  0.5f), new Vector3(-0.5f, -0.5f,  0.5f) },
            { new Vector3( 0.5f, -0.5f,  0.5f), new Vector3( 0.5f, -0.5f, -0.5f), new Vector3( 0.5f,  0.5f, -0.5f), new Vector3( 0.5f,  0.5f,  0.5f) },
            { new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f,  0.5f), new Vector3(-0.5f,  0.5f,  0.5f), new Vector3(-0.5f,  0.5f, -0.5f) }
        };

        int sx = voxelData.GetLength(0);
        int sy = voxelData.GetLength(1);
        int sz = voxelData.GetLength(2);

        for (int x = 0; x < sx; x++)
        for (int y = 0; y < sy; y++)
        for (int z = 0; z < sz; z++)
        {
            if (voxelData[x, y, z] == 0) continue;
            Vector3 cubeCenter = voxelPositions[x, y, z];

            for (int f = 0; f < 6; f++)
            {
                Vector3Int neighbor = new Vector3Int(x, y, z) + Vector3Int.RoundToInt(faceDirs[f]);
                bool neighborSolid = false;

                if (neighbor.x >= 0 && neighbor.x < sx &&
                    neighbor.y >= 0 && neighbor.y < sy &&
                    neighbor.z >= 0 && neighbor.z < sz)
                    neighborSolid = voxelData[neighbor.x, neighbor.y, neighbor.z] == 1;

                if (neighborSolid) continue;

                int start = verts.Count;
                for (int i = 0; i < 4; i++)
                    verts.Add(cubeCenter + faceVerts[f, i] * voxelSize);

                tris.Add(start + 0);
                tris.Add(start + 1);
                tris.Add(start + 2);
                tris.Add(start + 2);
                tris.Add(start + 3);
                tris.Add(start + 0);
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        var mc = GetComponent<MeshCollider>();
        mc.sharedMesh = null;
        mc.sharedMesh = mf.sharedMesh;
    }

}
