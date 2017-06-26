using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Reusable;
using System;
using Lyric = LyricBundle.Lyric;
using UnityEngine.UI;
using DG.Tweening;

public class Test : MonoBehaviour
{
    public Transform _canvastf;
    public Transform _text;
    public float _countTime = 0;
    public int _frame = 24;//帧频
    void Start ()
    {
        RectTransform rtf = this.GetComponent<RectTransform>();
        _countTime = Time.time;
        float number = 0;
        DOTween.To(() => number, x => number = x, 1000, 10f).OnUpdate(delegate
        {
            if (Time.time-_countTime>=1/_frame)
            {
                _countTime = Time.time;
                _text.transform.SetParent(_canvastf);
                rtf.sizeDelta = new Vector2(number, 80);
                _text.transform.SetParent(this.transform);
            }

        }).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }
}
