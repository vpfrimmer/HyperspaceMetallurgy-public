using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TractorBeamController : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Material Beam1Material;
    [SerializeField]
    private Material Beam2Material;
    [SerializeField]
    private Material Beam3Material;
    [SerializeField, Range(0f, 1f)]
    private float Beam1Alpha;
    [SerializeField, Range(0f, 1f)]
    private float Beam2Alpha;
    [SerializeField, Range(0f, 1f)]
    private float Beam3Alpha;
    [SerializeField]
    private float Beam1OffsetSpeed;
    [SerializeField]
    private ParticleSystem AspirationPS;
    [SerializeField]
    private ParticleSystem ExpirationPS;
    [SerializeField]
    private bool VisibleByDefault = false;

    private bool visible;
    private int direction;

    private void Start()
    {
        if (VisibleByDefault)
            ToggleBeam(true, -1);
    }

    private void Update()
    {
        Beam1Material.SetFloat("_Alpha", Beam1Alpha);
        Beam1Material.SetVector("_ROffsetSpeed", Vector3.up * Beam1OffsetSpeed * direction);
        Beam2Material.SetFloat("_Alpha", Beam2Alpha);
        Beam3Material.SetFloat("_Alpha", Beam3Alpha);

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            visible = !visible;
            ToggleBeam(visible, 1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            visible = !visible;
            ToggleBeam(visible, -1);
        }
    }

    private void TogglePS(ParticleSystem ps, bool toggle)
    {
        if (toggle)
            ps.Play();
        else
            ps.Stop();
    }

    private void SetBeamDirection(bool isAspiration)
    {
        Beam1OffsetSpeed = Mathf.Abs(Beam1OffsetSpeed) * (isAspiration ? -1 : 1);
    }

    public void ToggleBeam(bool toggle, int direction)
    {
        this.direction = direction;
        Animator.SetBool("Visible", toggle);
        TogglePS(AspirationPS, direction < 0 && toggle);
        TogglePS(ExpirationPS, direction > 0 && toggle);
    }
}
