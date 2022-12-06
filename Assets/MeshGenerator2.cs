using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator2 : MonoBehaviour
{
    public int width = 10;
    public int depth = 10;

    public float amp = 5.0f;
    public float freq = 5.0f;

    public int waterLevel = 2;
    public Material waterMaterial;

    float offSetX;
    float offSetZ;

    float maxOffsetNumber = 10;

    Mesh mesh;
    GameObject water;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
        CreateWater();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateMesh();
            CreateWater();
        }
        UpdateMesh();
    }
    void CreateMesh()
    {
        //---------------------------------------------------------
        // Create Vertices
        //---------------------------------------------------------

        int i = 0;
        float y = 0;
        vertices = new Vector3[(width + 1) * (depth + 1)];

        // Takes Value x and y and adds random offsets to them
        //----------------------------------------------------
        offSetX = Random.Range(0, maxOffsetNumber);
        offSetZ = Random.Range(0, maxOffsetNumber);
        //----------------------------------------------------

        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                y = PerlinNoise2D(((float)x * freq) + offSetX, ((float)z * freq) + offSetZ) * amp; 
                
                vertices[i] = new Vector3(x, y, z);
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
                //yield return new WaitForSeconds(0.1f);
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

    void CreateWater()
    {
        if (water == null)
        {
            water = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }

        // Center water 
        Vector3 pos = water.transform.position;

        //Center depending on map size
        pos.x = width / 2;
        pos.z = depth / 2;

        pos.y = waterLevel;

        water.transform.position = pos;

        // Scale water
        Vector3 scale = water.transform.localScale;

        scale.x = width / 7.0f - 0.3f;
        scale.z = depth / 7.0f - 0.3f; 

        water.transform.localScale = scale;

        // Give material to plane
        Material mat = water.GetComponent<MeshRenderer>().material = waterMaterial;
        Color color = mat.color;
    }

    float PerlinNoise2D(float x, float y)
    {
        // 0.0..1.0
        // *2
        // 0.0..2.0
        // -1
        // -1.0..1.0

        return Mathf.PerlinNoise(x, y) * 2.0f - 1.0f;
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
