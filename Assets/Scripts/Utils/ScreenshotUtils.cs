using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotUtils : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            ScreenCapture.CaptureScreenshot("screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png", 6);
        }
    }
}
