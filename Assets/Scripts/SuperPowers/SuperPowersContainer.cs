using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPowersContainer : MonoBehaviour
{
    public static SuperPowersContainer Instance;
    [SerializeField] TelekineticPush tkPush;
    [SerializeField] TelekineticPull tkPull;
    List<SuperPower> superPowers = new List<SuperPower>();
    int powerListIndex;

    SuperPower currentPower => superPowers[powerListIndex];

    private void OnDrawGizmosSelected()
    {
        if (superPowers.Count > 0)
            currentPower.Gizmos();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        superPowers.Add(tkPush);
        superPowers.Add(tkPull);

        foreach (var item in superPowers)
        {
            item.Initialize();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            powerListIndex++;
            powerListIndex %= superPowers.Count;
        }

        currentPower.UpdatePower();
    }
}
