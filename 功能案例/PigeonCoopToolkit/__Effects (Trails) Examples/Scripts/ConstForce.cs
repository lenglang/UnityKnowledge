using UnityEngine;
using System.Collections;

public class ConstForce : MonoBehaviour {
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        foreach(var plume in GetComponents<PigeonCoopToolkit.Effects.Trails.SmokePlume>())
        {
            plume.ConstantForce = transform.forward * speed;
        }
	}
}
