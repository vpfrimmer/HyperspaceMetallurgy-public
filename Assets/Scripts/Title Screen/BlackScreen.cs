using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    /// <summary>
    /// Animator
    /// </summary>
    private Animator _animator;

    private Image _screenImage;

    /// <summary>
    /// BlackScreen Event Type
    /// </summary>
    public enum BlackScreenEventType
    {
        Visible = 0,
        Hidden = 1
    }

    /// <summary>
    /// BlackScreen Event
    /// </summary>
    public delegate void BlackScreenEvent();
    public BlackScreenEvent onVisible;
    public BlackScreenEvent onHidden;

    //---------------------------------------------
    /// <summary>
    /// Start
    /// </summary>
    private void Awake()
    {
        _screenImage = GetComponent<Image>();
        _screenImage.enabled = true;

        this._animator = this.GetComponent<Animator>();
    }

    //---------------------------------------------
    /// <summary>
    /// Show
    /// </summary>
    public void Show()
    {
        this._animator.SetTrigger("show");
    }

    //---------------------------------------------
    /// <summary>
    /// Show
    /// </summary>
    public void SetVisible()
    {
        this._animator.SetTrigger("visible");
    }

    //---------------------------------------------
    /// <summary>
    /// Show Callback
    /// </summary>
    private void _VisibleCallback()
    {
        this.onVisible?.Invoke();
    }

    //---------------------------------------------
    /// <summary>
    /// Hide
    /// </summary>
    public void Hide()
    {
        this._animator.SetTrigger("hide");
    }

    //---------------------------------------------
    /// <summary>
    /// Hide
    /// </summary>
    public void SetHidden()
    {
        this._animator.SetTrigger("hidden");
    }

    //---------------------------------------------
    /// <summary>
    /// Hide Callback
    /// </summary>
    private void _HiddenCallback()
    {
        this.onHidden?.Invoke();
    }
}
