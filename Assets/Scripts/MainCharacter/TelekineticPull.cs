using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TelekineticPull : SuperPower
{
    [SerializeField] float interactionRange = 15f;
    [SerializeField] float interactionRadius = 10f;
    [SerializeField] float pullForce = 25;
    [SerializeField] Transform pullPoint;
    [SerializeField] LayerMask itemMask;
    Transform currentItem;

    [SerializeField] SphereCastVisual visual;

    public override void Initialize()
    {
        
    }

    public override void Gizmos()
    {
        base.Gizmos();
        visual.VisualizeCast(itemMask, new Ray(SuperPowersContainer.Instance.transform.position, Camera.main.transform.forward), interactionRange, interactionRadius);
    }

    public override void Effect()
    {
        base.Effect();
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        Vector3 vel = (pullPoint.position - currentItem.position).normalized * pullForce;
        rb.velocity = vel;
    }

    public override void UpdatePower()
    {
        bool detect = Physics.SphereCast(Camera.main.transform.position, interactionRadius, Camera.main.transform.forward, out RaycastHit hit, interactionRange, itemMask);
        if (detect)
            MonoBehaviour.print(hit.transform);

        if (Input.GetMouseButton(1) && hit.transform != null)
        {
            currentItem = hit.transform;
            Effect();
        }
    }
}
