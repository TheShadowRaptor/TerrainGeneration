using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
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

        CreateMesh();
        UpdateMesh();
    }

    void CreateMesh()
    {
        GenerateVertices();
        GenerateTriangles();
        
    }

    void UpdateMesh()
    {
        
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void GenerateVertices()
    {
        int i = 0;
        vertices = new Vector3[(width + 1) * (depth + 1)];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                vertices[i] = new Vector3(x, 0, z);
                Debug.Log("vertices " + vertices[i]);
                i++;
            }
        }
    }

    void GenerateTriangles()
    {
        //*--*--*--*
        //| /| /| /|
        //|/ |/ |/ |
        //*--*--*--*
        //| /| /| /|
        //|/ |/ |/ |
        //*--*--*--*
        //| /| /| /|
        //|/ |/ |/ |
        //*--*--*--*

        //quadWidth = 3;
        //quadDepth = 3;

        //VerticesWidth = (quadWidth[3 + 1] = (4); 
        //VerticesDepth = (quadDepth[3 + 1] = (4);

        //VerticesPerTriangle = 3;
        //TrianglesPerQuad = 2;

        //numTriangles = trianglesPerQuad * width * depth;

        //numTriangleVertices = numTriangles * verticesPerTriangle

        int vertice = 0;
        int trianglesPerQuad = 2;
        int verticesPerTriangle = 3;

        triangles = new int[width * depth * verticesPerTriangle * trianglesPerQuad];
        for (int i = 0; i < 4; i++)
        {
            triangles[vertice] = i;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;

            triangles[vertice] = i + 1;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;

            triangles[vertice] = i + width + 1;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;

            //triangles[vertice] = i + 1;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;

            //triangles[vertice] = i + width + 2;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;

            //triangles[vertice] = i + width + 1;
            Debug.Log("vertex: " + vertice + " = " + triangles[vertice]);
            vertice++;


        }

    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
