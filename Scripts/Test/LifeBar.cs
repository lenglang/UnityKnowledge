using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour {

    [Header("目标对象")]
    public Transform _target;
    [Header("偏移值，不同的角色值不同")]
    public Vector2 _offsetPosition;
    private RectTransform rectTF;
	// Use this for initialization
	void Start () {
        rectTF =GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //测试
        if (_target == null) return;
        Vector3 targetPosition = _target.position;
        Vector2 position = Camera.main.WorldToScreenPoint(targetPosition);
        rectTF.position = position + _offsetPosition;

	}
}
