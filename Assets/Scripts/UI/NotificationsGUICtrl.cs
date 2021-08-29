using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public enum DialogType
{
    Error,
    Warning,
    Info
}

/// <summary>This class is used to control the UI behaviour of notifications.</summary> 
public class NotificationsGUICtrl : MonoBehaviour
{
    // This is a great inspiration : http://codeseven.github.io/toastr/demo.html
    // First simple version of notifications
    // Maybe change the pop system to animation system

    [Header("Messages")]
    [SerializeField]
    private GameObject _mainCanvasContainer;

    [Header("Messages")]
    [SerializeField]
    [Tooltip("The prefab used to display planets notification.")]
    private NotificationGUICtrl _lostRobotNotificationPrefab = null;
    [SerializeField][Tooltip("The prefab used to display the notification.")]
    private NotificationGUICtrl _notificationGUICtrlPrefab = null;
    [SerializeField][Tooltip("The RectTransform container of all the notifications.")]
    private RectTransform _notificationsContainer = null;

    [SerializeField][Tooltip("The sprite used for an error.")]
    private Sprite _errorSprite = null;
    [SerializeField][Tooltip("The sprite used for an error.")]
    private Sprite _warningSprite = null;
    [SerializeField][Tooltip("The sprite used for an error.")]
    private Sprite _infoSprite = null;

    [Header("Loading Gauge")]
    [SerializeField][Tooltip("The image representing the gauge")]
    private Image _gaugeImg = null;
    [SerializeField][Tooltip("The gauge parent, used to disable them all.")]
    private GameObject _parentObject = null;

    private int _currentLoadPriority = -1;

    /// <summary>Number of current notifications.</summary>
    private int _runningNotifications = 0;

    private void Awake()
    {
        // For performance matters.
        _notificationGUICtrlPrefab.gameObject.SetActive(false);

        _parentObject.SetActive(false);

        _mainCanvasContainer.SetActive(false);
    }

    [Button]
    public void SetLoadValue(float v, int priority = 0, float min = 0f, float max = 1f)
    {
        if(priority < _currentLoadPriority)
        {
            return;
        }
        _currentLoadPriority = priority;

        _gaugeImg.fillAmount = v = Mathf.InverseLerp(min, max, v);

        if(v >= 1.0f)
        {
            _currentLoadPriority = -1;

            if (_runningNotifications == 0)
            { _mainCanvasContainer.SetActive(false); }

            _parentObject.SetActive(false);
        }
        else
        {
            _parentObject.SetActive(true);

            ShowCanvas();
        }
    }
        
    /// <summary>Make a notification appear.</summary> 
    public void Notify(string text)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_notificationGUICtrlPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        notification.Pop(text);
    }

    /// <summary>Make a notification appear.</summary> 
    public void NotifyLostRobot(Planet planet)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_lostRobotNotificationPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        notification.Pop("", planet.PlanetMiniature);
    }

    /// <summary>Make a notification appear.</summary> 
    public void Notify(string text, DialogType type)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_notificationGUICtrlPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        switch (type)
        {
            case DialogType.Error:
                notification.Pop(text, _errorSprite);
                break;
            case DialogType.Warning:
                notification.Pop(text, _warningSprite);
                break;
            case DialogType.Info:
                notification.Pop(text, _infoSprite);
                break;
        }
    }

    public void Notify(string text, Sprite sprite)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_notificationGUICtrlPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        notification.Pop(text, sprite);
    }

    /// <summary>Make a notification appear.</summary> 
    public void Notify(string text, float time)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_notificationGUICtrlPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        notification.Pop(text, time);
    }

    /// <summary>Make a notification appear.</summary> 
    [Button]
    public void Notify(string text, float time, DialogType type)
    {
        ++_runningNotifications;

        ShowCanvas();

        NotificationGUICtrl notification = Instantiate(_notificationGUICtrlPrefab, _notificationsContainer);

        notification.OnNotificationDeleted += OnNotificiationClosed;

        switch (type)
        {
            case DialogType.Error:
                notification.Pop(text, time, _errorSprite);
                break;
            case DialogType.Warning:
                notification.Pop(text, time, _warningSprite);
                break;
            case DialogType.Info:
                notification.Pop(text, time, _infoSprite);
                break;
        }
    }

    private void ShowCanvas()
    {
        if (!_mainCanvasContainer.activeSelf)
        {
            _mainCanvasContainer.SetActive(true);
        }
    }

    private void OnNotificiationClosed()
    {
        --_runningNotifications;

        if (_runningNotifications <= 0)
        {
            _mainCanvasContainer.SetActive(false);
        }
    }
}

