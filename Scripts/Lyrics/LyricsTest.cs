using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class LyricsTest : MonoBehaviour
{
    public Transform _canvastf;
    public Text _text;
    public RectTransform _imageRtf;
    [HideInInspector]
    public int _frame = 24;//帧频
    private float _countTime = 0;
    void Start ()
    {
        //获取该文本的文字实际宽高
        //_text.preferredWidth
        //_text.preferredHeight
        _countTime = Time.time;
        float number = 0;
        DOTween.To(() => number, x => number = x, 1000, 10f).OnUpdate(delegate
        {
            if (Time.time-_countTime>=1/_frame)
            {
                _countTime = Time.time;
                _text.transform.SetParent(_canvastf);
                _imageRtf.sizeDelta = new Vector2(number, 80);
                _text.transform.SetParent(_imageRtf);
            }

        }).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);

        //更合理方式，通过Image-FillAmount来改变遮罩宽
    }
}
