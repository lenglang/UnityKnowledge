using UnityEngine;
using System.Collections;

public class PoolManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MyPoolManager.Instance.GetInst("Cube");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
