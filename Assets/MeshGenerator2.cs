using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MeshGenerator2 : MonoBehaviour
{
    public int width = 10;
    public int depth = 10;

    public float amp = 5.0f;
    public float freq = 5.0f;

    public int waterLevel = 2;

    public Material waterMaterial;
    public Material sandMaterial;
    public Material metalMaterial;
    public Material grassMaterial;
    public Material chosenMaterial;

    public GameObject takeControlOn;
    public GameObject takeControlOff;
    public GameObject sandMaterialOn;
    public GameObject sandMaterialOff;
    public GameObject metalMaterialOn;
    public GameObject metalMaterialOff;
    public GameObject grassMaterialOn;
    public GameObject grassMaterialOff;

    public Slider ampSlider;
    public Slider freqSlider;

    public float maxOffsetNumber = 10;

    public float scrollSpeed = 0.1f;

    public bool takeMovementControl = false;

    float offSetX;
    float offSetZ;

    Mesh mesh;
    GameObject water;
    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        SliderInit();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = chosenMaterial;
        CreateMesh();
        CreateWater();
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
            Debug.Log("ExitApplication");
        }
        SliderUpdate();
        CreateMesh();
        MoveMesh();
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

        for (int z = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                if (z == 0 || z == depth)
                {
                    y = -1;
                }

                else if (x == 0 || x == width)
                {
                    y = -1;
                }

                else
                {
                    y = PerlinNoise2D(((float)x * freq) + offSetX, ((float)z * freq) + offSetZ) * amp;
                }
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
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 0]);
                triangles[vertice + 1] = num + width + 1;
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 1]);
                triangles[vertice + 2] = num + 1;
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 2]);

                triangles[vertice + 3] = num + 1;
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 3]);
                triangles[vertice + 4] = num + width + 1;
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 4]);
                triangles[vertice + 5] = num + width + 2;
                //Debug.Log("vertex " + num + " = " + triangles[vertice + 5]);

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

        scale.x = width / 10.0f;
        scale.z = depth / 10.0f;

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

    void MoveMesh()
    {
        if (takeMovementControl)
        {
            if (Input.GetKey(KeyCode.W))
            {
                offSetZ += scrollSpeed;
            }

            if (Input.GetKey(KeyCode.S))
            {
                offSetZ += -scrollSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                offSetX += -scrollSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                offSetX += scrollSpeed;
            }
        }
        else AutoScrollMesh();
    }

    void AutoScrollMesh()
    {
        offSetX += scrollSpeed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    // UI 
    public void NewRandomTerrainButton()
    {
        // Takes Value x and y and adds random offsets to them
        //----------------------------------------------------
        offSetX = Random.Range(0, maxOffsetNumber);
        offSetZ = Random.Range(0, maxOffsetNumber);
        //----------------------------------------------------
        CreateWater();
        GetComponent<MeshRenderer>().material = chosenMaterial;
    }

    public void TakeControlToggleButton()
    {
        if (takeMovementControl)
        {
            takeMovementControl = false;
            takeControlOn.SetActive(false);
            takeControlOff.SetActive(true);
        }
        else
        {
            takeMovementControl = true;
            takeControlOn.SetActive(true);
            takeControlOff.SetActive(false);
        }
    }

    public void PickMaterialSandButton()
    {
        chosenMaterial = sandMaterial;
        sandMaterialOn.SetActive(true);
        metalMaterialOn.SetActive(false);
        grassMaterialOn.SetActive(false);
        //--------------------------------------
        sandMaterialOff.SetActive(false);
        metalMaterialOff.SetActive(true);
        grassMaterialOff.SetActive(true);
    }

    public void PickMaterialMetalButton()
    {
        chosenMaterial = metalMaterial;
        sandMaterialOn.SetActive(false);
        metalMaterialOn.SetActive(true);
        grassMaterialOn.SetActive(false);
        //--------------------------------------
        sandMaterialOff.SetActive(true);
        metalMaterialOff.SetActive(false);
        grassMaterialOff.SetActive(true);
    }

    public void PickMaterialGrassButton()
    {
        chosenMaterial = grassMaterial;
        sandMaterialOn.SetActive(false);
        metalMaterialOn.SetActive(false);
        grassMaterialOn.SetActive(true);
        //--------------------------------------
        sandMaterialOff.SetActive(true);
        metalMaterialOff.SetActive(true);
        grassMaterialOff.SetActive(false);
    }

    public void SliderInit()
    {
        ampSlider.maxValue = 10.0f;
        freqSlider.maxValue = 0.2f;
        ampSlider.value = amp;
        freqSlider.value = freq;
    }

    public void SliderUpdate()
    {
        amp = ampSlider.value;
        freq = freqSlider.value;
    }
}
