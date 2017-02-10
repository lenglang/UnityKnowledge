using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
public class GameOver : MonoBehaviour
{
    private ColorCorrectionCurves _colorCor;
    // Use this for initialization
    void Start()
    {
        if (_colorCor == null)
            _colorCor = FindObjectOfType<ColorCorrectionCurves>();
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 100, 100, 50), "画面还原"))
        {
            SetColorCorrectionCurvesSaturation(1);
        }
        if (GUI.Button(new Rect(50, 200, 100, 50), "画面变灰"))
        {
            SetColorCorrectionCurvesSaturation(0);
        }
    }
    /// <summary>
    /// 设定当前摄像机的颜色度
    /// </summary>
    /// <param name="duration">Duration.</param>
    void SetColorCorrectionCurvesSaturation(int duration)
    {
        if (_colorCor)
            _colorCor.saturation = duration;
    }
}