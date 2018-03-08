using PigeonCoopToolkit.Effects.Trails;
using UnityEngine;
using System.Collections;

public class FPSWeaponTrigger : MonoBehaviour
{

    public Transform ShellEjectionTransform;
    public float EjectionForce;
    public Rigidbody Shell;

    public Transform Muzzle;
    public GameObject Bullet;

    public float SmokeAfter;
    public float SmokeMax;
    public float SmokeIncrement;
    public SmokePlume MuzzlePlume;

    public GameObject MuzzleFlashObject;

    private float _smoke;

    void Update()
    {
        MuzzlePlume.Emit = _smoke > SmokeAfter;
        _smoke -= Time.deltaTime;
        if(_smoke > SmokeMax)
            _smoke = SmokeMax;

        if (_smoke < 0)
            _smoke = 0;


    }

    public void Fire()
    {
        MuzzleFlashObject.SetActive(true);
        Invoke("LightsOff",0.05f);
        _smoke += SmokeIncrement;
        Rigidbody r = (Instantiate(Shell.gameObject, ShellEjectionTransform.position, ShellEjectionTransform.rotation) as
             GameObject).GetComponent<Rigidbody>();

        r.velocity = (ShellEjectionTransform.right * EjectionForce) + Random.onUnitSphere * 0.25f;
        r.angularVelocity = Random.onUnitSphere*EjectionForce;

        Instantiate(Bullet, Muzzle.transform.position, Muzzle.rotation);
    }

    private void LightsOff()
    {
        MuzzleFlashObject.SetActive(false);
    }
    

}
