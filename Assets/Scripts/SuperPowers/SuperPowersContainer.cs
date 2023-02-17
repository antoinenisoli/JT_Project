using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuperPowersContainer : MonoBehaviour
{
    public static SuperPowersContainer Instance;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TKPush tkPush;
    [SerializeField] TKPull tkPull;
    [SerializeField] TKMove tkMove;
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
        superPowers.Add(tkMove);
        foreach (var item in superPowers)
            item.Initialize();

        if (powerText)
            powerText.text = currentPower.name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            powerListIndex++;
            powerListIndex %= superPowers.Count;
            if (powerText)
                powerText.text = currentPower.name;
        }

        currentPower.UpdatePower();
    }

    private void FixedUpdate()
    {
        currentPower.FixedUpdate();
    }
}
