using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator2 : MonoBehaviour
{
    public int width = 10;
    public int depth = 10;
    public int height = 10;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        StartCoroutine(CreateMesh());

    }

    private void Update()
    {
        UpdateMesh();
    }
    IEnumerator CreateMesh()
    {
        //---------------------------------------------------------
        // Create Vertices
        //---------------------------------------------------------
        int i = 0;
        vertices = new Vector3[(width + 1) * (depth + 1)];
        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;   
            }
        }

        //---------------------------------------------------------
        // Create Triangles
        //---------------------------------------------------------

        triangles = new int[width * depth * 6];
        int vertice = 0;
        int num = 0;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[vertice + 0] = num + 0;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 0]);
                triangles[vertice + 1] = num + width + 1;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 1]);
                triangles[vertice + 2] = num + 1;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 2]);

                triangles[vertice + 3] = num + 1;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 3]);
                triangles[vertice + 4] = num + width + 1;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 4]);
                triangles[vertice + 5] = num + width + 2;
                Debug.Log("vertex " + num + " = " + triangles[vertice + 5]);

                num++;
                vertice += 6;
                yield return new WaitForSeconds(0.1f);
            }
            num++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void SmoothHeight()
    {

    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i],0.1f);
        }
    }
}
