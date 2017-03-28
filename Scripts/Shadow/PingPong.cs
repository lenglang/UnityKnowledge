using UnityEngine;
using System.Collections;

public class PingPong : MonoBehaviour {

	// Use this for initialization
    private float _add = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(-3 + Mathf.PingPong(_add, 6), transform.position.y, transform.position.z);
        _add += 0.1f;
	}
}
