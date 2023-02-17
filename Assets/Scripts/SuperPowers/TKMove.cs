using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TKMove : SuperPower
{
    [Header(nameof(TKMove))]
    [SerializeField] LayerMask itemMask;
    [SerializeField] Vector2 distanceLimits;
    [SerializeField] [Range(0,1)] float massInfluence = 1f;
    [SerializeField] float scrollSpeed, itemMoveSpeed = 5f, interactionRange = 15f;

    Rigidbody itemRigidbody;
    Transform currentItem;
    bool isMoving;
    Camera mainCam;
    float distance;

    public override void Initialize()
    {
        mainCam = Camera.main;
    }

    public override void Effect()
    {
        base.Effect();
        Vector2 scroll = Input.mouseScrollDelta;
        distance += scroll.y * scrollSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);

        Vector3 movePosition = mainCam.transform.position + mainCam.transform.forward * distance;
        float min = 0.38f;
        float moveSpeed = itemMoveSpeed - ((massInfluence * min) * itemRigidbody.mass);

        Vector3 difference = movePosition - currentItem.position;
        itemRigidbody.velocity = difference * moveSpeed;
        itemRigidbody.angularVelocity = new Vector3();
    }

    void StartMoving()
    {
        itemRigidbody = currentItem.GetComponent<Rigidbody>();
        itemRigidbody.velocity = new Vector3();
        itemRigidbody.angularVelocity = new Vector3();
        itemRigidbody.useGravity = false;

        distance = Vector3.Distance(mainCam.transform.position, itemRigidbody.position);
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);
        isMoving = true;
    }

    void EndMoving()
    {
        itemRigidbody.useGravity = true;
        isMoving = false;
        currentItem = null;
    }

    public override void UpdatePower()
    {
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, interactionRange, itemMask);

        if (Input.GetMouseButton(0))
        {
            if (!isMoving && hit.transform != null)
            {
                currentItem = hit.transform;
                StartMoving();
            }

            if (currentItem && isMoving)
                Effect();
        }

        if (Input.GetMouseButtonUp(0))
            EndMoving();
    }
}
