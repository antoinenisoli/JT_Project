using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TKMove : SuperPower
{
    [Header(nameof(TKMove))]
    [SerializeField] float interactionRange = 15f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] LayerMask itemMask;
    Transform currentItem;
    bool isMoving;
    Vector3 pickItemPosition;

    public override void Initialize()
    {
        
    }

    public override void Effect()
    {
        base.Effect();
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        float dist = Vector3.Distance(pickItemPosition, Camera.main.transform.position);
        Vector3 movePosition = Camera.main.transform.position + Camera.main.transform.forward * dist;
        rb.position = Vector3.Lerp(rb.position, movePosition, 10f * Time.deltaTime);
    }

    public override void UpdatePower()
    {
        bool detect = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionRange, itemMask);

        if (Input.GetMouseButton(0))
        {
            if (!isMoving && hit.transform != null)
            {
                isMoving = true;
                currentItem = hit.transform;
                pickItemPosition = currentItem.position;
            }

            if (currentItem && isMoving)
                Effect();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            currentItem = null;
        }    
    }
}
