using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SuperPower
{
    [SerializeField] public float powerCooldown = 0.3f;
    protected bool canUse = true;

    public virtual void Gizmos()
    {

    }

    public virtual void Effect()
    {
        
    }

    public IEnumerator Cooldown()
    {
        canUse = false;
        yield return new WaitForSeconds(powerCooldown);
        canUse = true;
    }

    public abstract void Initialize();
    public abstract void UpdatePower();
}
