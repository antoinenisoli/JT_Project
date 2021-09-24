using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekineticPush : MonoBehaviour
{
    [SerializeField] ParticleSystem fx;
    [SerializeField] float maximumPushForce = 500f;
    [SerializeField] float currentPushForce;
    [SerializeField] float chargeSpeed = 2f;
    [SerializeField] float pushRange = 15f;
    [SerializeField] float pushRadius = 50f;
    [SerializeField] LayerMask mask;
    [SerializeField] SphereCastVisual visual;
    bool canUse = true;

    private void OnDrawGizmosSelected()
    {
        visual.VisualizeCast(mask, new Ray(transform.position, Camera.main.transform.forward), pushRange, pushRadius);
    }

    public void Start()
    {
        currentPushForce = maximumPushForce * 0.25f;
    }

    IEnumerator Push()
    {
        RaycastHit[] colliders = Physics.SphereCastAll(transform.position, pushRadius, Camera.main.transform.forward, pushRange, mask);
        foreach (var item in colliders)
        {
            //Vector3 force = item.transform.position - transform.position;
            //item.rigidbody.AddForceAtPosition(force * pushForce, transform.position);
            item.rigidbody.velocity = new Vector3();
            item.rigidbody.AddExplosionForce(currentPushForce, transform.position, 80f, 3f);
        }

        currentPushForce = maximumPushForce * 0.25f;
        yield return new WaitForSeconds(0.3f);
        canUse = true;
    }

    private void Update()
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
            StartCoroutine(Push());
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
