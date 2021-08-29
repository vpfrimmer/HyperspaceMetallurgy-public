using TMPro;
using UnityEngine;
using UnityEngine.UI;


//=================================================================================================================	<summary>
///  This class is used to control the UI behaviour of a notification.												</summary> 
///=================================================================================================================
public class NotificationGUICtrl : MonoBehaviour
{
	// This is a great inspiration : http://codeseven.github.io/toastr/demo.html
	// First simple version of notifications
	// Maybe change the pop system to animation system

	[SerializeField][Tooltip("The canvas group used to control the notification.")]
	private CanvasGroup		_canvasGroup = null;
	[SerializeField][Tooltip("The information text of the notification.")]
	private TMP_Text _text = null;
	[SerializeField][Tooltip("The image used to display a sprite.")]
	private Image  _image = null;
	[SerializeField][Tooltip("The time used to make the notification smoothly appear and disappear.")]
	private float	_time = 1.0f;
	[SerializeField][Tooltip("The time the notification is visible.")]
	private float	_defaultTime = 1.0f;

	/// <summary>The start time of the lerp.</summary>
	private float _startTime = 0.0f;
	/// <summary>The alpha value you're lerping from, should be 1 or 0.</summary>
	private float _from = 0.0f;
	/// <summary>The alpha value you're lerping to, should be 1 or 0.</summary>
	private float _to = 1.0f;
	/// <summary>Are you lerping and make the notification appear/disappear ?</summary>
	private bool    _isLerping = false;
	/// <summary>Enable this when you want to destroy the notification after the lerp.</summary>
	private bool    _toDestroy = false;

    public delegate void NotificationDepop();
    /// <summary>Raised when the notification is deleted.</summary>
    public event NotificationDepop OnNotificationDeleted;

	///-------------------------------------------------------	<summary> 
	///  Make a notification appear.								</summary> 
	///-------------------------------------------------------
	public void Pop(string text, Sprite sprite = null)
	{
		Pop(text, _defaultTime, sprite);
	}

	///-------------------------------------------------------	<summary> 
	///  Make a notification appear.								</summary> 
	///-------------------------------------------------------
	public void Pop(string text, float time, Sprite sprite = null)
	{
		_text.text = text;
		if (sprite != null)
		{
			_image.gameObject.SetActive(true);
			_image.sprite = sprite;
		}
		_startTime = Time.time;
		_from = 0.0f;
		_to = 1.0f;

		_isLerping = true;

        gameObject.SetActive(true);

		// Don't go below the time to make the notification appear/disappear
		Invoke("Depop", Mathf.Max(time, _time));
	}

	///-------------------------------------------------------	<summary> 
	///  Make a notification disappear.							</summary> 
	///-------------------------------------------------------
	private void Depop()
	{
		_startTime = Time.time;
		_to = 0.0f;
		_from = 1.0f;

		_toDestroy = true;
		_isLerping = true;
	}

	///-------------------------------------------------------	<summary> 
	///  Unity Update.											</summary> 
	///-------------------------------------------------------
	private void Update()
	{
		if (_isLerping)
		{
			float elapsedTime = (Time.time - _startTime);
			float progress01 = elapsedTime / _time;

			_canvasGroup.alpha = Mathf.Lerp(_from, _to, progress01);

			if (progress01 >= 1.0f)
			{
				_isLerping = false;
				if (_toDestroy)
				{
                    OnNotificationDeleted?.Invoke();

                    Destroy(gameObject);
                }
			}
		}
	}
}

