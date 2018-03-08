using System.Collections.Generic;
using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour
{

    public float TrailMaterialOffsetSpeed;
    public float MoveSpeed, MoveFriction, MoveAcceleration;
    public float RotateSpeed, RotateFriction, RotateAcceleration;
    public Material TrailMaterial;
    public Animator Animator;
    public List<Trail> TankTrackTrails;
    public TankWeaponController WeaponController;

    private float _moveSpeed;
    private float _rotateSpeed;

    public bool InControl = false;



    // LateUpdate is called once per frame
	void Update ()
	{
	    Animator.SetBool("InControl", InControl);
        
        if(InControl)
        {
            WeaponController.enabled = true;
            if (Input.GetKey(KeyCode.W))
            {
                Animator.SetBool("Forward", true);
                Animator.SetBool("Backward", false);

                _moveSpeed += MoveAcceleration * Time.deltaTime * 2;
                if (_moveSpeed > MoveSpeed)
                    _moveSpeed = MoveSpeed;

            }
            else if (Input.GetKey(KeyCode.S))
            {
                Animator.SetBool("Backward", true);
                Animator.SetBool("Forward", false);

                _moveSpeed -= MoveAcceleration * Time.deltaTime * 2;
                if (_moveSpeed < -MoveSpeed)
                    _moveSpeed = -MoveSpeed;

            }
            else
            {
                Animator.SetBool("Backward", false);
                Animator.SetBool("Forward", false);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rotateSpeed += RotateAcceleration * Time.deltaTime * 2;
                if (_rotateSpeed > RotateSpeed)
                    _rotateSpeed = RotateSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _rotateSpeed -= RotateAcceleration * Time.deltaTime * 2;
                if (_rotateSpeed < -RotateSpeed)
                    _rotateSpeed = -RotateSpeed;
            }
        }
        else
        {
            WeaponController.enabled = false;
        }

        if(Mathf.Abs(_moveSpeed) > 0)
        {
            TankTrackTrails.ForEach(trail => { trail.Emit = true; });
        }
        else
        {
            TankTrackTrails.ForEach(trail => { trail.Emit = false; });
        }

        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, transform.up, _rotateSpeed);


        TrailMaterial.mainTextureOffset = new Vector2(TrailMaterial.mainTextureOffset.x + Mathf.Sign(_moveSpeed) * Mathf.Lerp(0, TrailMaterialOffsetSpeed, Mathf.Abs(_moveSpeed / MoveSpeed) + Mathf.Abs(_rotateSpeed / RotateSpeed)),
                                                          TrailMaterial.mainTextureOffset.y);

        _moveSpeed = Mathf.MoveTowards(_moveSpeed, 0, MoveFriction * Time.deltaTime);
        _rotateSpeed = Mathf.MoveTowards(_rotateSpeed, 0, RotateFriction * Time.deltaTime);

	}

}
