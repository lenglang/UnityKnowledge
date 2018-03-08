using UnityEngine;
using System.Collections;

public class TankWeaponController : MonoBehaviour
{

    public TankProjectile ProjectilePrefab;
    public Transform Nozzle;

    private Animation _animation;

    void Awake()
    {
        _animation = GetComponent<Animation>();
    }

	// Update is called once per frame
	void Update () {
        if (_animation.isPlaying == false && Input.GetKeyDown(KeyCode.Space))
	    {
            _animation.Play();
            Instantiate(ProjectilePrefab, Nozzle.position, Nozzle.rotation);
	    }
	}
}
