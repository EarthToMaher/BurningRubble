using UnityEngine;
using System.Collections.Generic;

public class FractureCube : MonoBehaviour
{
    [SerializeField] private Vector3 voxelScale = new Vector3(1, 1, 1);
    private Vector3 scale;

    void Awake()
    {
        scale = transform.localScale;

        if (scale.x % voxelScale.x != 0f || scale.y % voxelScale.y != 0f || scale.z % voxelScale.z != 0f)
        {
            Debug.LogWarning("The scale of the object is not a multiple of the voxel scale. This may lead to unexpected results.");

        }
        for (float x = 0; x < scale.x; x += voxelScale.x)
        {
            for (float y = 0; y < scale.y; y += voxelScale.y)
            {
                for (float z = 0; z < scale.z; z += voxelScale.z)
                {
                    //Vector3 localPos = new Vector3(((x + voxelScale.x * 0.5f) / scale.x) - 0.5f, ((y + voxelScale.y * 0.5f) / scale.y) - 0.5f, ((z + voxelScale.z * 0.5f) / scale.z) - 0.5f);

                    //Convert to world space
                    //Vector3 worldPos = transform.TransformPoint(localPos);

                    //if (IsPointInsideMesh(worldPos, this.gameObject.GetComponent<MeshCollider>()))
                    //{
                        Debug.Log("Creating Cube");
                        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        voxel.AddComponent<DestructibleBlock>();
                        voxel.transform.parent = transform;
                        voxel.transform.localPosition = new Vector3(((x + voxelScale.x * 0.5f) / scale.x) - 0.5f, ((y + voxelScale.y * 0.5f) / scale.y) - 0.5f, ((z + voxelScale.z * 0.5f) / scale.z) - 0.5f);
                        voxel.transform.localScale = new Vector3(voxelScale.x / scale.x, voxelScale.y / scale.y, voxelScale.z / scale.z);
                        //voxel.AddComponent<Rigidbody>();
                    //}
                }
            }

        }
    }
    bool IsPointInsideMesh(Vector3 point, MeshCollider mc)
    {
        Debug.Log("I EXIST!!!");
        // Cast a ray upward and count intersections
        Ray ray = new Ray(point, new Vector3(point.x, point.y + scale.y, point.z));
        Debug.DrawLine(point, new Vector3(point.x, point.y + scale.y, point.z), Color.red, Mathf.Infinity);
        Debug.Log("I Drew a line!!!");
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        int count = 0;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == mc)
                count++;
        }
        Debug.Log(count);

        // Odd = inside, even = outside
        return (count % 2) == 1;
    }//*/

    /*bool IsPointInsideMesh(Vector3 point, MeshCollider mc)
    {
        // tiny overlap sphere at the point
        Collider[] overlaps = Physics.OverlapSphere(point, 0.001f);

        foreach (var c in overlaps)
        {
            if (c == mc)
                return true; // definitely inside
        }

        // fallback: raycast test (for points near the outside)
        Ray ray = new Ray(point, Vector3.up);
        int count = 0;
        foreach (var hit in Physics.RaycastAll(ray, 100f))
        {
            if (hit.collider == mc)
                count++;
        }
        return (count % 2) == 1;
    }*/
}
