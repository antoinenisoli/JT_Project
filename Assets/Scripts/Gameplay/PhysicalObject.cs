using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PhysicalObject : MonoBehaviour
{
    [SerializeField] float weight = 3;
    [SerializeField] ShaderOutline[] outlineShaders;
    Rigidbody rb;

    public Rigidbody Rigidbody { get => rb; set => rb = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        outlineShaders = GetComponentsInChildren<ShaderOutline>();
        Highlight(false);
    }

    public void StartMoving()
    {
        rb.velocity = new Vector3();
        rb.angularVelocity = new Vector3();
        rb.useGravity = false;
    }

    public void Move(Vector3 force)
    {
        rb.velocity = force;
        rb.angularVelocity = new Vector3();
    }

    public void EndMoving(Vector3 storedForce)
    {
        rb.useGravity = true;
        rb.AddForce(storedForce);
    }

    public void Highlight(bool value)
    {
        foreach (var item in outlineShaders)
            item.enabled = value;
    }
}
