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
    [SerializeField] float pushForce = 10f;
    [SerializeField] float scrollSpeed, itemMoveSpeed = 5f, interactionRange = 15f;

    Vector3 storedForce;
    Rigidbody itemRigidbody;
    Transform currentItem;
    bool isMoving;
    Camera mainCam;
    float distance;
    bool down;

    public override void Initialize()
    {
        mainCam = Camera.main;
    }

    public override void Effect()
    {
        base.Effect();
        distance += Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);

        Vector3 movePosition = mainCam.transform.position + mainCam.transform.forward * distance;
        float min = 0.38f;
        float moveSpeed = itemMoveSpeed - ((massInfluence * min) * itemRigidbody.mass);

        storedForce = movePosition - currentItem.position;
        itemRigidbody.velocity = storedForce * moveSpeed;
        itemRigidbody.angularVelocity = new Vector3();

        if (Input.GetMouseButtonDown(1))
        {
            storedForce = new Vector3();
            itemRigidbody.AddForce(mainCam.transform.forward * pushForce, ForceMode.Impulse);
            EndMoving();
        }
    }

    void StartMoving()
    {
        itemRigidbody = currentItem.GetComponentInChildren<Rigidbody>();
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
        itemRigidbody.AddForce(storedForce);
        itemRigidbody = null;
        down = false;

        isMoving = false;
        currentItem = null;
    }

    public override void UpdatePower()
    {
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, interactionRange, itemMask);
        if (Input.GetMouseButtonDown(0))
            down = true;

        if (Input.GetMouseButton(0) && down)
        {
            if (!isMoving && hit.transform != null)
            {
                currentItem = hit.transform;
                StartMoving();
            }

            if (currentItem && isMoving)
                Effect();
        }

        if (Input.GetMouseButtonUp(0) && currentItem)
            EndMoving();
    }
}
