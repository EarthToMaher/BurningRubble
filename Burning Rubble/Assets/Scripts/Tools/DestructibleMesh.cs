using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

//A lot of this script used a ChatGPT script as a base (mainly so I didn't have to deal with typing out as much math)


public class DestructibleMesh : MonoBehaviour
{
    public Vector3[,,] voxelPositions;
    public byte[,,] voxelData;

    public int size = 16;       // number of voxels per axis
    public float voxelSize = 1f;

    public Vector3 hitRadius = new Vector3(1.1f,1,1.5f); // x,y,z radius for destruction

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        if (voxelData == null || voxelPositions == null)
        {
            InitializeVoxels();
            RebuildMesh();
        }
    }

    void InitializeVoxels()
    {
        voxelPositions = new Vector3[size, size, size];
        voxelData = new byte[size, size, size];

        Vector3 offset = new Vector3(size, size, size) * voxelSize * 0.5f;

        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                for (int z = 0; z < size; z++)
                {
                    voxelPositions[x, y, z] = new Vector3(x, y, z) * voxelSize - offset;
                    voxelData[x, y, z] = 1;
                }
    }

    void OnTriggerEnter(Collider other)
    {
        ApplyHit(other.transform.position);
    }

    void OnTriggerStay(Collider other)
    {
        ApplyHit(other.transform.position);
    }

    void ApplyHit(Vector3 worldPoint)
    {
        // Convert world point to local voxel space
        Vector3 localPoint = transform.InverseTransformPoint(worldPoint);

        localPoint.y += 0.1f;
        localPoint.x += 0.15f;

        Vector3 min = localPoint - hitRadius;
        Vector3 max = localPoint + hitRadius;

        bool modified = false;

        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                for (int z = 0; z < size; z++)
                {
                    if (voxelData[x, y, z] == 0) continue;

                    Vector3 voxelCenter = voxelPositions[x, y, z];

                    // Check if voxel center is inside the hit bounds
                    if (voxelCenter.x >= min.x && voxelCenter.x <= max.x &&
                        voxelCenter.y >= min.y && voxelCenter.y <= max.y &&
                        voxelCenter.z >= min.z && voxelCenter.z <= max.z)
                    {
                        voxelData[x, y, z] = 0;
                        modified = true;
                    }
                }

        if (modified)
            RebuildMesh();
    }

    public void RebuildMesh()
    {
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
            { new Vector3(-0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f) },
            { new Vector3(0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f) },
            { new Vector3(-0.5f,0.5f,0.5f), new Vector3(0.5f,0.5f,0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(-0.5f,0.5f,-0.5f) },
            { new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,-0.5f,0.5f), new Vector3(-0.5f,-0.5f,0.5f) },
            { new Vector3(0.5f,-0.5f,0.5f), new Vector3(0.5f,-0.5f,-0.5f), new Vector3(0.5f,0.5f,-0.5f), new Vector3(0.5f,0.5f,0.5f) },
            { new Vector3(-0.5f,-0.5f,-0.5f), new Vector3(-0.5f,-0.5f,0.5f), new Vector3(-0.5f,0.5f,0.5f), new Vector3(-0.5f,0.5f,-0.5f) }
        };

        int sx = voxelData.GetLength(0);
        int sy = voxelData.GetLength(1);
        int sz = voxelData.GetLength(2);

        for (int x = 0; x < sx; x++)
            for (int y = 0; y < sy; y++)
                for (int z = 0; z < sz; z++)
                {
                    if (voxelData[x, y, z] == 0) continue;

                    Vector3 center = voxelPositions[x, y, z];

                    for (int f = 0; f < 6; f++)
                    {
                        Vector3Int neighbor = new Vector3Int(x, y, z) + Vector3Int.RoundToInt(faceDirs[f]);
                        bool neighborSolid = false;

                        if (neighbor.x >= 0 && neighbor.x < sx &&
                            neighbor.y >= 0 && neighbor.y < sy &&
                            neighbor.z >= 0 && neighbor.z < sz)
                        {
                            neighborSolid = voxelData[neighbor.x, neighbor.y, neighbor.z] == 1;
                        }

                        if (neighborSolid) continue;

                        int start = verts.Count;
                        for (int i = 0; i < 4; i++)
                            verts.Add(center + faceVerts[f, i] * voxelSize);

                        tris.Add(start + 0);
                        tris.Add(start + 1);
                        tris.Add(start + 2);
                        tris.Add(start + 2);
                        tris.Add(start + 3);
                        tris.Add(start + 0);
                    }
                }

        Mesh newMesh = new Mesh();
        newMesh.indexFormat = IndexFormat.UInt32;
        newMesh.SetVertices(verts);
        newMesh.SetTriangles(tris, 0);
        newMesh.RecalculateNormals();
        newMesh.RecalculateBounds();

        meshFilter.sharedMesh = newMesh;

        // Update collider
        if (meshCollider)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = newMesh;
        }
    }
}