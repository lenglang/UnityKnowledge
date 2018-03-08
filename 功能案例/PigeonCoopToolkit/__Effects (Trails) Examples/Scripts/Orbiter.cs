using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour
{

    public float TankCollisionOrbitRadius = 1.5f;
    public float TankCollisionRotationSpeed = 1f;
    public Trail Trail;

    private TankController _tankBeingController;


    private Vector3 _pos;
	// Use this for initialization
	void Start ()
	{
	    _pos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    bool tankHit = false;

	    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
	    RaycastHit h;
	    TankController tc = null;
        if(Physics.Raycast(r,out h, 1000))
        {
            tc = h.collider.transform.root.GetComponent<TankController>();

            if(tc == null)
            {
                _pos = h.point;
                
            }
            else
            {
                tankHit = true;
                _pos = tc.transform.position;
            }
        }

        if (tankHit == false)
        {
            Trail.Emit = false;
        }
        else
        {
            if(_tankBeingController != tc)
            {
                Trail.Emit = true;

                transform.localScale = Vector3.one * TankCollisionOrbitRadius;
                transform.Rotate(Vector3.up, TankCollisionRotationSpeed * Time.deltaTime);
                transform.position = _pos;
            }
            

            if(Input.GetMouseButtonDown(0))
            {
                if (_tankBeingController != null)
                    _tankBeingController.InControl = false;

                tc.InControl = true;
                _tankBeingController = tc;
            }

           
        }

	}
}
