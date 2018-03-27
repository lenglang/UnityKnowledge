using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class ParticlePath : MonoBehaviour 
{
    [HideInInspector]
    public bool IsApprove = false;
    [HideInInspector]
    public bool IsPath = false;
    [HideInInspector]
    public List<Vector3> Waypoints;
    [HideInInspector]
    public bool IsHideInInspector = false;
    [HideInInspector]
    public ParticleSystem PS;
    [HideInInspector]
    public float Speed = 2;

    private ParticleSystem.MainModule _psmm;
    private float _oldSpeed;

    private void Awake()
    {
        if (!PS || Waypoints.Count <= 0)
        {
            IsApprove = false;
            IsPath = false;
            return;
        }

        _psmm = PS.main;
        _oldSpeed = Speed;
        _psmm.startLifetime = Waypoints.Count * Speed;
        _psmm.simulationSpace = ParticleSystemSimulationSpace.Custom;
        _psmm.customSimulationSpace = transform;

        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (IsApprove && IsPath)
        {
            if (_oldSpeed != Speed)
            {
                _oldSpeed = Speed;
                _psmm.startLifetime = Waypoints.Count * Speed;
            }

            ParticleSystem.Particle[] ps = new ParticleSystem.Particle[PS.particleCount];
            int pCount = PS.GetParticles(ps);

            for (int i = 0; i < pCount; i++)
            {
                float proportion = (ps[i].startLifetime - ps[i].remainingLifetime) / ps[i].startLifetime;
                int index = Mathf.FloorToInt(proportion * Waypoints.Count);
                if (index >= 0 && index < Waypoints.Count - 1)
                {
                    Vector3 direction = Waypoints[index + 1] - Waypoints[index];
                    ps[i].velocity = direction * (1.0f / Speed) * (1.0f / transform.localScale.x);
                }
                else
                {
                    ps[i].remainingLifetime = 0;
                }
            }

            PS.SetParticles(ps, pCount);
        }
    }
}
