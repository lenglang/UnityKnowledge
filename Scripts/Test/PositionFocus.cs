using UnityEngine;
using System.Collections;

public class PositionFocus : MonoBehaviour {

    public Transform _targetUI;
	// Use this for initialization
	void Start ()
    {
        transform.position = _targetUI.position;
        Debug.LogError("UI的localposition是二维坐标，所以注意赋值时一定都要是position");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
