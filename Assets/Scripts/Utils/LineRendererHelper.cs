using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineRendererHelper : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private LineRenderer lineRenderer
    {
        get { return _lineRenderer == null ? _lineRenderer = GetComponent<LineRenderer>() : _lineRenderer; }
        set { _lineRenderer = value; }
    }

    private void Start()
    {
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            Destroy(this);
        }
    }

    private void Update()
    {
        lineRenderer.positionCount = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            lineRenderer.SetPosition(i, transform.GetChild(i).localPosition);
        }
    }
}
