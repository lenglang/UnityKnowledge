using UnityEngine;
using System.Collections;

public class TankAlwaysForward : MonoBehaviour {
    public Material TrailMaterial;
    public float Speed;
    public float TrailSpeed;

    void FixedUpdate()
    {
        transform.position = transform.position + transform.forward * Speed;
        TrailMaterial.mainTextureOffset = new Vector2(TrailMaterial.mainTextureOffset.x + TrailSpeed,
                                                          TrailMaterial.mainTextureOffset.y);
    }
}
