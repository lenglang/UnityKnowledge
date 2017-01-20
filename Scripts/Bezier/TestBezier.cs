using UnityEngine;
using System.Collections;
public class TestBezier : MonoBehaviour
{
    public enum State
    {
        none,
        moveState,
        editState
    }
    public State state = State.none;

    private Bezier bezier;
    public GameObject line;//曲线的对象
    private LineRenderer lineRenderer;//曲线对象的曲线组件

    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public Transform targetObj;//要移动的对象

    public int pointAmount = 100;//值越大曲线越平滑
    public float pointTime = 0.01f;//每前进一个点的时间
    private Vector3 v1;

    void Start()
    {
        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(pointAmount);
        v1 = p1.localPosition;//起点不变
    }

    void Update()
    {
        if (state == State.editState)
        {
            float t = 1 / (float)pointAmount;
            bezier = new Bezier(v1, p2.localPosition, p3.localPosition, p4.localPosition);
            for (int i = 1; i <= pointAmount; i++)
            {
                //参数的取值范围 0 - 1 返回曲线每一点的位置
                //为了精确这里使用i * 0.01 得到当前点的坐标
                Vector3 vec = bezier.GetPointAtTime((float)(i * t));
                //把每条线段绘制出来 完成贝塞尔曲线的绘制
                lineRenderer.SetPosition(i - 1, vec);
            }
        }
        else if (state == State.moveState)
        {
            state = State.none;
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        bezier = new Bezier(v1, p2.localPosition, p3.localPosition, p4.localPosition);

        float t = 1 / (float)pointAmount;
        int count = 0;
        float timer = 0;

        while (count <= pointAmount)
        {
            if (timer < pointTime)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                timer = 0;
                targetObj.localPosition = bezier.GetPointAtTime((float)(count * t));
                count++;
            }
        }
    }
}