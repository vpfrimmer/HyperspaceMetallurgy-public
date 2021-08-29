using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerSelectorValidator : MonoBehaviour
{
    /// <summary>
    /// Text
    /// </summary>
    private Text _text;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        this._text = this.GetComponentInChildren<Text>();
    }

    /// <summary>
    /// Validate
    /// </summary>
    public void ResetFontStyle()
    {
        this._text.fontStyle = FontStyle.Italic;
    }

    /// <summary>
    /// Validate
    /// </summary>
    public void ValidatedFontStyle()
    {
        this._text.fontStyle = FontStyle.Bold;
    }
}
