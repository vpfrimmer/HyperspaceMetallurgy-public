using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetVisualController : MonoBehaviour
{
    [SerializeField, OnValueChanged("UpdateMeshes")]
    private float PlanetRadius = 5f;
    [SerializeField, OnValueChanged("UpdateMeshes")]
    private PlanetMesh[] PlanetMeshes;
    [SerializeField, OnValueChanged("UpdateMeshes")]
    private Transform CoreTransform;
    [SerializeField, OnValueChanged("UpdateMeshes")]
    private float CoreSizeOffset;

    public float Radius => PlanetRadius;

    [Button("Update Meshes")]
    public void UpdateMeshes()
    {
        float crustThickness = 0;
        foreach (PlanetMesh m in PlanetMeshes)
        {
            if (m.IsAtmosphere)
            {
                m.SetPlanetRadius(PlanetRadius + m.RadiusOffset);
            }
            else
            {
                m.SetPlanetRadius(PlanetRadius + m.RadiusOffset - m.Thickness - crustThickness);
                crustThickness += m.Thickness;
            }
            m.Generate();
        }
        CoreTransform.localScale = Vector3.one * (PlanetRadius - crustThickness + CoreSizeOffset) * 2f;
        GetComponentInChildren<PlanetDecorationsController>()?.UpdateDecorations(PlanetRadius);
    }
}
