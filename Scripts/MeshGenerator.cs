using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [Header("Dimensions")]

    [Tooltip("X Size")]
    public float xSize;

    [Tooltip("Z Size")]
    public float zSize;

    [Tooltip("X Subdivisions")]
    public int xSubdivisions;

    [Tooltip("Z Subdivisions")]
    public int zSubdivisions;

    [Header("Material")]
    public Material material;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Awake ()
    {
        GenerateNewMesh();
    }

    public void GenerateNewMesh ()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[(xSubdivisions + 1) * (zSubdivisions + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        float xSubLength = xSize / (float)xSubdivisions;
        float zSubLength = zSize / (float)zSubdivisions;

        for (int i = 0, z = 0; z <= zSubdivisions; z++)
        {
            for(int x = 0; x <= zSubdivisions; x++, i++)
            {
                vertices[i] = new Vector3(x * xSubLength, 0, z * zSubLength);
                uv[i] = new Vector2((float)x / xSubdivisions, (float)z / zSubdivisions);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;

        int[] tris = new int[xSubdivisions * zSubdivisions * 6];

        for (int ti = 0, vi = 0, y = 0; y < zSubdivisions; y++, vi++)
        {
            for (int x = 0; x < xSubdivisions; x++, ti += 6, vi++)
            {
                tris[ti] = vi;
                tris[ti + 3] = tris[ti + 2] = vi + 1;
                tris[ti + 4] = tris[ti + 1] = vi + xSubdivisions + 1;
                tris[ti + 5] = vi + xSubdivisions + 2;
            }
        }

        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }
}