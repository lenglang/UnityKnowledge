using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
public enum PropType
{
    None,
    列消除,
    横消除,
    炸弹,
    时间
}
public enum MouseState
{
    按下,
    弹起,
    禁止
}
public class CookiesControl : MonoBehaviour
{
    [Header("饼干预制体")]
    public GameObject _cookiesPrefab;
    [Header("饼干容器")]
    public Transform _content;
    [Header("饼干Sprite数组")]
    public List<Sprite> _spritesList=new List<Sprite>();
    [Header("连线")]
    public GameObject _line;
    [Header("几列")]
    public int _column= 8;
    [Header("几行")]
    public int _row = 8;
    [Header("X方向偏移，默认居中生成")]
    public int _deviationX= 0;
    [Header("Y方向偏移，默认居中生成")]
    public int _deviationY = 0;
    [Header("饼干之间X方向间隔")]
    public int _intervalX = 128;
    [Header("饼干之间Y方向间隔")]
    public int _intervalY = 128;
    [Header("生成种类个数")]
    public int _typeNum = 5;
    private float _cookiesWidth = 0;//饼干宽
    private float _cookiesHeight = 0;//饼干高
    private float _startX = 0;//开始x位置
    private float _startY = 0;//开始y位置
    private MouseState _mouseState = MouseState.禁止;//鼠标状态
    private string _choseCookiesType = "";//选择饼干的类别名
    private List<CookiesAttribute> _deleteCookies = new List<CookiesAttribute>();//需删除的饼干
    private int _prevCookiesColumn = 0;//上个饼干列值
    private int _prevCookiesRow = 0;//上个饼干行值
    private List<GameObject> _linesList = new List<GameObject>();//线数组
    private List<CookiesAttribute> _moveCookies = new List<CookiesAttribute>();//移动饼干
    private int _maxMove = 0;//最大移动距离
    private List<int> _columnDeleteNumList=new List<int>();//列删除的个数数组
    private Dictionary<GameObject, CookiesAttribute> _cookiesAttributeDictionary = new Dictionary<GameObject, CookiesAttribute>();//饼干属性字典
    void Start ()
    {
        //获取饼干宽高
        RectTransform rtf = _cookiesPrefab.GetComponent<RectTransform>();
        _cookiesWidth = rtf.sizeDelta.x;
        _cookiesHeight = rtf.sizeDelta.y;
        //容错处理
        if (_column <= 1 || _row <= 1||_cookiesWidth> _intervalX || _cookiesHeight> _intervalY|| _typeNum==0||_typeNum>_spritesList.Count)
        {
            Debug.LogError("参数不合规！");
            return;
        }
        //设置开始生成点位置
        _startX = -((_column - 1) * _intervalY + _cookiesWidth) / 2 + _deviationX + _cookiesWidth / 2;
        _startY = ((_row - 1) * _intervalY + _cookiesHeight) / 2 + _deviationY - _cookiesHeight / 2;
        //初始数据
        for (int i = 0; i < _column; i++)
        {
            _columnDeleteNumList.Add(0);
        }
        //初始生成饼干
        InItCreateCookies();
        _mouseState = MouseState.弹起;
    }
    /// <summary>
    /// 初始生成所有饼干
    /// </summary>
    private void InItCreateCookies()
    {
        for (int i = 0; i < _column; i++)
        {
            for (int j = 0; j < _row; j++)
            {
                CreateCookies(i, j);
            }
        }
    }
    /// <summary>
    /// 生成饼干
    /// </summary>
    /// <param name="c">列</param>
    /// <param name="r">行</param>
    public CookiesAttribute CreateCookies(int c,int r)
    {
        GameObject cookies = Instantiate(_cookiesPrefab);
        RectTransform rtf = cookies.GetComponent<RectTransform>();
        rtf.SetParent(_content);
        rtf.localPosition = new Vector2(_startX + c * _intervalX, _startY - r * _intervalY);
        //随机图标
        Sprite sprite = _spritesList[UnityEngine.Random.Range(0, _typeNum)];
        CookiesAttribute ca = cookies.AddComponent<CookiesAttribute>();
        ca._c = c;
        ca._r = r;
        ca._image = rtf.GetComponent<Image>();
        ca._image.sprite = sprite;
        ca._type = sprite.name;
        _cookiesAttributeDictionary.Add(cookies, ca);
        EventTriggerListener.Get(cookies)._onDown = OnCookiesDown;
        EventTriggerListener.Get(cookies)._onEnter = OnCookiesEnter;
        EventTriggerListener.Get(cookies)._onUp = OnCookiesUp;
        return ca;
    }
    /// <summary>
    /// 鼠标按下饼干
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnCookiesDown(PointerEventData evenData, GameObject obj)
    {
        if (_mouseState != MouseState.弹起) return;
        CookiesAttribute ca = _cookiesAttributeDictionary[obj];
        _choseCookiesType = ca._type;
        ShowOther(false);
        _deleteCookies.Add(ca);
        RefreshColumnRow(ca);
        ChoseAnimation(obj);
        _mouseState = MouseState.按下;
    }
    /// <summary>
    /// 鼠标经过饼干
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnCookiesEnter(PointerEventData evenData, GameObject obj)
    {
        if (_mouseState!=MouseState.按下) return;
        CookiesAttribute ca = _cookiesAttributeDictionary[obj];
        if (_choseCookiesType == ca._type)
        {
            int index = _deleteCookies.IndexOf(ca);
            if (index != -1)
            {
                //已存在数组列表
                if (index == _deleteCookies.Count - 1) return;
                for (int i = _deleteCookies.Count-1; i>index ; i--)
                {
                    DOTween.Kill(_deleteCookies[i].transform);
                    _deleteCookies[i].transform.localScale = new Vector2(1, 1);
                    _deleteCookies.RemoveAt(i);
                    Destroy(_linesList[i-1]);
                    _linesList.RemoveAt(i-1);
                }
                RefreshColumnRow(_deleteCookies[_deleteCookies.Count - 1]);
            }
            else if(Math.Abs(ca._c - _prevCookiesColumn) <= 1 && Math.Abs(ca._r - _prevCookiesRow) <= 1)
            {
                _deleteCookies.Add(ca);
                RefreshColumnRow(ca);
                CreateLine();
                ChoseAnimation(obj);
            }
        }
    }
    /// <summary>
    /// 刷新当前行列值
    /// </summary>
    /// <param name="cookiesName"></param>
    private void RefreshColumnRow(CookiesAttribute ca)
    {
        _prevCookiesColumn = ca._c;
        _prevCookiesRow = ca._r;
    }
    /// <summary>
    /// 鼠标弹起
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnCookiesUp(PointerEventData evenData, GameObject obj)
    {
        //显示其他饼干
        ShowOther(true);
        //移除线
        for (int i = 0; i < _linesList.Count; i++)
        {
            Destroy(_linesList[i]);
        }
        _linesList.Clear();
        //删除饼干
        if (_deleteCookies.Count >= 2)
        {
            _mouseState = MouseState.禁止;
            _maxMove = 1;
            for (int i = 0; i < _deleteCookies.Count; i++)
            {
                _cookiesAttributeDictionary.Remove(_deleteCookies[i].gameObject);
                DOTween.Kill(_deleteCookies[i].transform);
                JudgeMove(_deleteCookies[i]);
                Destroy(_deleteCookies[i].gameObject);
            }
            CreateNewCookies();
            StartMove();
            WaitActionControl.Instance.AddWaitAction(delegate
            {
                MoveEnd();
            }, _maxMove * 0.3f);
        }
        else
        {
            _mouseState = MouseState.弹起;
            for (int i = 0; i < _deleteCookies.Count; i++)
            {
                DOTween.Kill(_deleteCookies[i].transform);
                _deleteCookies[i].transform.localScale = new Vector2(1, 1);
            }
        }
        _deleteCookies.Clear();
    }
    /// <summary>
    /// 判断移动
    /// </summary>
    private void JudgeMove(CookiesAttribute ca)
    {
        _columnDeleteNumList[ca._c]++;
        foreach (KeyValuePair<GameObject,CookiesAttribute> item in _cookiesAttributeDictionary)
        {
            if (item.Value._c == ca._c && item.Value._r < ca._r)
            {
                item.Value._move++;
                if (_moveCookies.IndexOf(item.Value) == -1 && _deleteCookies.IndexOf(item.Value) == -1)
                {
                    _moveCookies.Add(item.Value);
                }
            }
        }
    }
    /// <summary>
    /// 创建新的饼干
    /// </summary>
    private void CreateNewCookies()
    {
        CookiesAttribute ca;
        _maxMove = 1;
        for (int i = 0; i < _columnDeleteNumList.Count; i++)
        {
            //最大移动间隔
            if (_columnDeleteNumList[i] > _maxMove) _maxMove = _columnDeleteNumList[i];
            if (_columnDeleteNumList[i] != 0)
            {
                for (int j = 0; j < _columnDeleteNumList[i]; j++)
                {
                    ca=CreateCookies(i, j-_columnDeleteNumList[i]);
                    ca._move = _columnDeleteNumList[i];
                    _moveCookies.Add(ca);
                }
                _columnDeleteNumList[i] = 0;
            }
        }
    }
    /// <summary>
    /// 开始移动
    /// </summary>
    private void StartMove()
    {
        int move = 0;
        float targetY = 0;
        for (int i = 0; i < _moveCookies.Count; i++)
        {
            move = _moveCookies[i]._move;
            targetY = _startY - (move + _moveCookies[i]._r) * _intervalY;
            _moveCookies[i].transform.DOLocalMoveY(targetY, move * 0.3f).SetEase(Ease.OutBounce);
        }
    }
    /// <summary>
    /// 移动结束
    /// </summary>
    private void MoveEnd()
    {
        for (int i = 0; i < _moveCookies.Count; i++)
        {
            _moveCookies[i]._r += _moveCookies[i]._move;
            _moveCookies[i]._move = 0;
        }
        _moveCookies.Clear();
        _mouseState = MouseState.弹起;
    }
    /// <summary>
    /// 创建线
    /// </summary>
    private void CreateLine()
    {
        LineRenderer lineRenderer =Instantiate(_line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, _deleteCookies[_deleteCookies.Count - 2].transform.localPosition);
        lineRenderer.SetPosition(1, _deleteCookies[_deleteCookies.Count - 1].transform.localPosition);
        _linesList.Add(lineRenderer.gameObject);
    }
    /// <summary>
    /// 选择动画
    /// </summary>
    private void ChoseAnimation(GameObject obj)
    {
        obj.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBounce);
    }
    /// <summary>
    /// 显示|透明饼干
    /// </summary>
    /// <param name="b"></param>
    private void ShowOther(bool b=false)
    {
        foreach (KeyValuePair<GameObject, CookiesAttribute> item in _cookiesAttributeDictionary)
        {
            if (_choseCookiesType != item.Value._type)
            {
                if (b)
                {
                    item.Value._image.color = new Color(1f, 1f, 1f);
                }
                else
                {
                    item.Value._image.color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }
}
