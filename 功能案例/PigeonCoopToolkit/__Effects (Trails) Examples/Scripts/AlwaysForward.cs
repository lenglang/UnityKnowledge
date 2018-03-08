using UnityEngine;
using System.Collections;

public class AlwaysForward : MonoBehaviour
{
    public float Speed;
    public float yRotation;

    void Update()
    {
        transform.position = transform.position + transform.forward * Speed;
        transform.Rotate(Vector3.up, yRotation);
    }
}
