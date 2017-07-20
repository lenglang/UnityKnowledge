using UnityEngine;
using System.Collections;

public class Paint : MonoBehaviour {
    public enum AXIS
    {
        axis_X,
        axis_Y,
        axis_Z
    }

    public AXIS stateAxis = AXIS.axis_X;
	// Use this for initialization
    private Mesh mMesh = null;
    private Vector3[] vertices = null;
    private Color[] colorList = null;
    public float num = 0;
    private float startValue = 0;
    private bool isChangeColor = false;
    private Color _color = Color.white;
    private Color startColor = Color.white;

    public GameObject targetObj = null;
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1) && !isChangeColor)
        {
            _color = Color.yellow;
            isChangeColor = true;
        }
        if (Input.GetKeyDown(KeyCode.F2) && !isChangeColor)
        {
            _color = Color.red;
            isChangeColor = true;
        }

        if (isChangeColor)
        {
            SetChangeModeColor(_color);
        }
	}

    void Init()
    {
        if (null == targetObj) targetObj = gameObject;
        mMesh = targetObj.GetComponent<MeshFilter>().mesh;
        if (null == mMesh)
        {
            Debug.LogError(targetObj.name + "_MeshFilter is null");
            return;
        }
        vertices = mMesh.vertices;
        colorList = new Color[vertices.Length];
        //SetChangeModeColor(Color.red);
        num = -gameObject.GetComponent<Renderer>().bounds.extents.z;
        startValue = num;
    }

    void SetChangeModeColor(Color targetColor)
    {
        if (startColor == targetColor)
        {
            isChangeColor = false;
            return;
        }
        if (num > -startValue)
        {
            isChangeColor = false;
            startColor = targetColor;
            num = startValue;
        }
        for (int index = 0; index < vertices.Length; index++)
        {
            if (stateAxis == AXIS.axis_X)
                colorList[index] = Color.Lerp(startColor, targetColor, vertices[index].x + num);
            if (stateAxis == AXIS.axis_Y)
                colorList[index] = Color.Lerp(startColor, targetColor, vertices[index].y + num);
            if (stateAxis == AXIS.axis_Z)
                colorList[index] = Color.Lerp(startColor, targetColor, vertices[index].z + num);
        }
        num += (Time.deltaTime*1);
        mMesh.colors = colorList;
    }
}
