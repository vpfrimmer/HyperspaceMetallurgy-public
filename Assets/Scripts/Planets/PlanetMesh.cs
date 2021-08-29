using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMesh : MonoBehaviour
{
    [SerializeField]
    private Material PlanetMaterial;
    [SerializeField, OnValueChanged("Generate")]
    private Vector2Int MeshGridSizePerSegment;
    [SerializeField, OnValueChanged("Generate")]
    private float PlanetRadius = 10f;
    [SerializeField]
    private float radiusOffset = 0f;
    [SerializeField, OnValueChanged("Generate")]
    private float PlanetThickness = 1f;
    [SerializeField, OnValueChanged("Generate")]
    private float TextureRepeat = 1f;
    [SerializeField]
    private bool isAtmosphere = false;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh mesh;
    private Vector3[] vertices;

    private MeshFilter meshFilter
    {
        get
        {
            var existing = gameObject.GetComponent<MeshFilter>();
            return existing != null ? existing : gameObject.AddComponent<MeshFilter>();
        }
    }

    private MeshRenderer meshRenderer
    {
        get
        {
            var existing = gameObject.GetComponent<MeshRenderer>();
            return existing != null ? existing : gameObject.AddComponent<MeshRenderer>();
        }
    }

    public bool IsAtmosphere
    {
        get { return isAtmosphere; }
    }

    public float Thickness
    {
        get { return PlanetThickness; }
    }

    public float RadiusOffset
    {
        get { return radiusOffset; }
    }

    [Button("Generate")]
    public void Generate()
    {
        mesh = new Mesh();
        vertices = new Vector3[MeshGridSizePerSegment.x * MeshGridSizePerSegment.y];
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < MeshGridSizePerSegment.y; i++)
        {
            for (int j = 0; j < MeshGridSizePerSegment.x; j++)
            {
                float radiusAtVertex = PlanetRadius + i * PlanetThickness / (MeshGridSizePerSegment.y - 1);
                float angleAtVertex = Mathf.PI * 2f / (MeshGridSizePerSegment.x - 1) * j;
                vertices[i * MeshGridSizePerSegment.x + j] =
                    new Vector3(
                        radiusAtVertex * Mathf.Sin(angleAtVertex),
                        radiusAtVertex * Mathf.Cos(angleAtVertex),
                        0f
                        );
                uv[i * MeshGridSizePerSegment.x + j] = new Vector2((float)j / (MeshGridSizePerSegment.x - 1) * TextureRepeat, (float)i / (MeshGridSizePerSegment.y - 1));
            }
        }

        int[] triangles = new int[6 * (MeshGridSizePerSegment.x - 1) * (MeshGridSizePerSegment.y - 1)];
        int currentIndex = 0;
        for (int i = 0; i < MeshGridSizePerSegment.y - 1; i++)
        {
            for (int j = 0; j < MeshGridSizePerSegment.x - 1; j++)
            {
                int index = i * MeshGridSizePerSegment.x + j;
                triangles[currentIndex] = index + 1;
                triangles[currentIndex + 1] = index;
                triangles[currentIndex + 2] = index + MeshGridSizePerSegment.x;
                triangles[currentIndex + 3] = index + 1;
                triangles[currentIndex + 4] = index + MeshGridSizePerSegment.x;
                triangles[currentIndex + 5] = index + MeshGridSizePerSegment.x + 1;
                currentIndex += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        meshFilter.mesh = mesh;
        meshRenderer.material = PlanetMaterial;
    }

    public void SetPlanetRadius(float newRadius)
    {
        PlanetRadius = newRadius;
    }
}
