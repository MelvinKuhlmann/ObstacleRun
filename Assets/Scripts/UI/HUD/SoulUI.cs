using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SoulUI : MonoBehaviour
{
    public int numberOfSoulsCollected = 0;
    public TMP_Text soulsLabel;
    public TMP_Text soulsAddedLabel;
    private float addedLabelDuration = 3f;
    private float addedTime;


    private void Start()
    {
        soulsLabel.text = numberOfSoulsCollected.ToString();
    }

    private void Update()
    {
        if (addedTime <= 0)
        {
            soulsAddedLabel.enabled = false;
        }
        else
        {
            addedTime -= Time.deltaTime;
        }
    }

    public void AddItem(int amount)
    {
        numberOfSoulsCollected += amount;
        soulsLabel.text = numberOfSoulsCollected.ToString();
        soulsAddedLabel.text = "+" + amount;
        soulsAddedLabel.enabled = true;
        addedTime = addedLabelDuration;
    }

    public void Initialize(int amount)
    {
        numberOfSoulsCollected = amount;
        soulsLabel.text = numberOfSoulsCollected.ToString();
    }

    public void RemoveItem(int amount)
    {
        numberOfSoulsCollected -= amount;
        soulsLabel.text = numberOfSoulsCollected.ToString();
    }
}
