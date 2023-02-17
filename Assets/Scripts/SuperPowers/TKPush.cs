using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TKPush : SuperPower
{
    [Header(nameof(TKPush))]
    [SerializeField] ParticleSystem fx;
    [SerializeField] float maximumPushForce = 500f;
    [SerializeField] float currentPushForce;
    [SerializeField] float chargeSpeed = 2f;
    [SerializeField] float pushRange = 15f;
    [SerializeField] float pushRadius = 50f;
    [SerializeField] LayerMask mask;
    [SerializeField] SphereCastVisual visual;

    public override void Gizmos()
    {
        base.Gizmos();
        visual.VisualizeCast(mask, new Ray(SuperPowersContainer.Instance.transform.position, Camera.main.transform.forward), pushRange, pushRadius);
    }

    public override void Initialize()
    {
        currentPushForce = maximumPushForce * 0.25f;
    }

    public override void Effect()
    {
        RaycastHit[] colliders = Physics.SphereCastAll(SuperPowersContainer.Instance.transform.position, pushRadius, Camera.main.transform.forward, pushRange, mask);
        foreach (var item in colliders)
        {
            item.rigidbody.velocity = new Vector3();
            item.rigidbody.AddExplosionForce(currentPushForce, SuperPowersContainer.Instance.transform.position, 80f, 3f);
        }

        currentPushForce = maximumPushForce * 0.25f;
        SuperPowersContainer.Instance.StartCoroutine(Cooldown());
    }

    public override void UpdatePower()
    {
        if (Input.GetMouseButton(0) && canUse && currentPushForce < maximumPushForce)
        {
            if (fx.isStopped)
                fx.Play();

            currentPushForce += chargeSpeed;
        }

        if (Input.GetMouseButtonUp(0) && canUse)
        {
            canUse = false;
            Effect();
            fx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        var f = fx.emission;
        var n = fx.noise;
        var s = fx.shape;
        s.radius = (float)(currentPushForce / maximumPushForce) * 3f;
        n.strength = (float)(currentPushForce / maximumPushForce) * 2f;
        f.rateOverTime = currentPushForce - (maximumPushForce * 0.25f);
    }
}
