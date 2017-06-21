using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;
public class CookiesControl : MonoBehaviour
{
    public enum MouseState
    {
        按下,
        弹起,
        禁止
    }
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
    private float _cookiesWidth = 0;//饼干宽
    private float _cookiesHeight = 0;//饼干高
    private float _startX = 0;//开始x位置
    private float _startY = 0;//开始y位置
    private List<GameObject> _cookiesList = new List<GameObject>();//饼干数组
    private MouseState _mouseState = MouseState.禁止;//鼠标状态
    private string _choseCookiesType = "";//选择饼干的类别名
    private List<GameObject> _deleteCookies = new List<GameObject>();//需删除的饼干
    private int _prevCookiesColumn = 0;//上个饼干列值
    private int _prevCookiesRow = 0;//上个饼干行值
    private List<GameObject> _otherCookies = new List<GameObject>();//其他颜色饼干
    private List<GameObject> _linesList = new List<GameObject>();//线数组
    private List<GameObject> _moveCookies = new List<GameObject>();//移动饼干
    private int _maxMove = 0;//最大移动距离
    private List<int> _columnDeleteNumList=new List<int>();//列删除的个数数组
    void Start ()
    {
        //获取饼干宽高
        RectTransform rtf = _cookiesPrefab.GetComponent<RectTransform>();
        _cookiesWidth = rtf.sizeDelta.x;
        _cookiesHeight = rtf.sizeDelta.y;
        //容错处理
        if (_column <= 1 || _row <= 1||_cookiesWidth> _intervalX || _cookiesHeight> _intervalY)
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
    public GameObject CreateCookies(int c,int r)
    {
        GameObject cookies = Instantiate(_cookiesPrefab);
        RectTransform rtf = cookies.GetComponent<RectTransform>();
        rtf.SetParent(_content);
        rtf.localPosition = new Vector2(_startX + c * _intervalX, _startY - r * _intervalY);
        //随机图标
        Sprite sprite = _spritesList[UnityEngine.Random.Range(0, _spritesList.Count)];
        rtf.GetComponent<Image>().sprite = sprite;
        rtf.name = sprite.name + "&" + c + "," + r;
        _cookiesList.Add(cookies);
        EventTriggerListener.Get(cookies).onDown = OnCookiesDown;
        EventTriggerListener.Get(cookies).onEnter = OnCookiesEnter;
        EventTriggerListener.Get(cookies).onUp = OnCookiesUp;
        return cookies;
    }
    /// <summary>
    /// 鼠标按下饼干
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnCookiesDown(PointerEventData evenData, GameObject obj)
    {
        if (_mouseState != MouseState.弹起) return;
        _choseCookiesType = obj.name.Split('&')[0];
        _otherCookies = _cookiesList.FindAll(n => n.name.Split('&')[0] != obj.name.Split('&')[0]);
        for (int i = 0; i < _otherCookies.Count; i++)
        {
            _otherCookies[i].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        _deleteCookies.Add(obj);
        RefreshColumnRow(obj.name);
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
        if (_choseCookiesType == obj.name.Split('&')[0])
        {
            int index = _deleteCookies.IndexOf(obj);
            string[] point = obj.name.Split('&')[1].Split(',');
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
                RefreshColumnRow(_deleteCookies[_deleteCookies.Count - 1].name);
            }
            else if(Math.Abs(int.Parse(point[0]) - _prevCookiesColumn) <= 1 && Math.Abs(int.Parse(point[1]) - _prevCookiesRow) <= 1)
            {
                _deleteCookies.Add(obj);
                RefreshColumnRow(obj.name);
                CreateLine();
                ChoseAnimation(obj);
            }
        }
    }
    /// <summary>
    /// 鼠标弹起
    /// </summary>
    /// <param name="evenData"></param>
    /// <param name="obj"></param>
    private void OnCookiesUp(PointerEventData evenData, GameObject obj)
    {
        //显示其他饼干
        for (int i = 0; i < _otherCookies.Count; i++)
        {
            _otherCookies[i].GetComponent<Image>().color = new Color(1, 1, 1);
        }
        _otherCookies.Clear();
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
                DOTween.Kill(_deleteCookies[i].transform);
                JudgeMove(_deleteCookies[i]);
                _cookiesList.RemoveAt(_cookiesList.IndexOf(_deleteCookies[i]));
                Destroy(_deleteCookies[i]);
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
    private void JudgeMove(GameObject obj)
    {
        _columnDeleteNumList[int.Parse(obj.name.Split('&')[1].Split(',')[0])]++;
        List<GameObject> moveCookies = _cookiesList.FindAll(cookies => cookies.name.Split('&')[1].Split(',')[0] == obj.name.Split('&')[1].Split(',')[0]&& int.Parse(cookies.name.Split('&')[1].Split(',')[1])< int.Parse(obj.name.Split('&')[1].Split(',')[1]));
        for (int i = 0; i < moveCookies.Count; i++)
        {
            string[] s = moveCookies[i].name.Split('&');
            if (s.Length <3)
            {
                moveCookies[i].name += "&1";
            }
            else
            {
                if(int.Parse(s[2]) + 1>_maxMove)_maxMove= int.Parse(s[2]) + 1;
                moveCookies[i].name = s[0] + "&" + s[1] + "&" + (int.Parse(s[2]) + 1).ToString();
            }
            if (_moveCookies.IndexOf(moveCookies[i]) == -1&&_deleteCookies.IndexOf(moveCookies[i])==-1)
            {
                _moveCookies.Add(moveCookies[i]);
            }
        }
    }
    /// <summary>
    /// 创建新的饼干
    /// </summary>
    private void CreateNewCookies()
    {
        GameObject cookies;
        for (int i = 0; i < _columnDeleteNumList.Count; i++)
        {
            if (_columnDeleteNumList[i] != 0)
            {
                for (int j = 0; j < _columnDeleteNumList[i]; j++)
                {
                    cookies=CreateCookies(i, j-_columnDeleteNumList[i]);
                    cookies.name += "&" + _columnDeleteNumList[i].ToString();
                    _moveCookies.Add(cookies);
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
            move = int.Parse(_moveCookies[i].name.Split('&')[2]);
            targetY = _startY - (move + int.Parse(_moveCookies[i].name.Split('&')[1].Split(',')[1])) * _intervalY;
            _moveCookies[i].transform.DOLocalMoveY(targetY, move * 0.3f).SetEase(Ease.OutBounce);
        }
    }
    /// <summary>
    /// 移动结束
    /// </summary>
    private void MoveEnd()
    {
        int move = 0;
        for (int i = 0; i < _moveCookies.Count; i++)
        {
            string[] names = _moveCookies[i].name.Split('&');
            move = int.Parse(names[2]);
            string[] point = names[1].Split(',');
            _moveCookies[i].name = names[0] + "&" + point[0] + "," + (move+int.Parse(point[1])).ToString();
        }
        _moveCookies.Clear();
        _mouseState = MouseState.弹起;
    }
    /// <summary>
    /// 刷新当前行列值
    /// </summary>
    /// <param name="cookiesName"></param>
    private void RefreshColumnRow(string cookiesName)
    {
        string[] cr = cookiesName.Split('&')[1].Split(',');
        _prevCookiesColumn =int.Parse(cr[0]);
        _prevCookiesRow = int.Parse(cr[1]);
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
}
