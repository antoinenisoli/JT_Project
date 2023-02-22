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
    PhysicalObject currentItem;
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
        float moveSpeed = itemMoveSpeed;

        storedForce = movePosition - currentItem.transform.position;
        currentItem.Move(storedForce * moveSpeed);

        if (Input.GetMouseButtonDown(1))
        {
            storedForce = new Vector3();
            currentItem.Rigidbody.AddForce(mainCam.transform.forward * pushForce, ForceMode.Impulse);
            EndMoving();
        }
    }

    void StartMoving()
    {
        currentItem.StartMoving();
        distance = Vector3.Distance(mainCam.transform.position, currentItem.transform.position);
        distance = Mathf.Clamp(distance, distanceLimits.x, distanceLimits.y);
        isMoving = true;
    }

    void EndMoving()
    {
        currentItem.EndMoving(storedForce);
        currentItem = null;
        down = false;

        isMoving = false;
        currentItem = null;
    }

    void GetObject(Transform trsf)
    {
        if (trsf)
        {
            PhysicalObject obj = trsf.GetComponentInChildren<PhysicalObject>();
            if (obj)
            {
                if (currentItem && currentItem != obj)
                {
                    currentItem.Highlight(false);
                    currentItem = null;
                }

                currentItem = obj;
                currentItem.Highlight(true);
                return;
            }
        }

        if (currentItem)
        {
            currentItem.Highlight(false);
            currentItem = null;
        }
    }

    public override void UpdatePower()
    {
        Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, interactionRange, itemMask);
        if (Input.GetMouseButtonDown(0))
            down = true;

        if (currentItem)
        {
            if (Input.GetMouseButton(0) && down)
            {
                if (!isMoving)
                    StartMoving();
                else if (isMoving)
                    Effect();
            }
        }
        else
          GetObject(hit.transform);

        if (Input.GetMouseButtonUp(0) && currentItem)
            EndMoving();
    }
}
