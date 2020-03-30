using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;
    
    public event Action<float> OnHealthPctChanged = delegate { };

    public void ModifyHealth(int amount)
    {
        playerHealth.CurrentHealth += amount;

        float currentHealthPct = playerHealth.GetRatio();
        OnHealthPctChanged(currentHealthPct);
    }
}
