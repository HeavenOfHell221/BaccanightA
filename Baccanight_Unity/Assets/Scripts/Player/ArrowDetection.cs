using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetection : Arrow
{
    public void OnArrowEnter(GameObject target)
    {
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine(Disable(1f));

        if (target.tag == "BossHealth")
        {
            target.GetComponent<HealthBoss>().ModifyHealth(m_damage);
        }
        else if (target.tag == "BossShield")
        {
            ShieldAttack shield = target.GetComponent<ShieldAttack>();
            
            if(shield)
            {
                shield.CounterAttackEvent.Invoke(BossActionType.CounterAttack);
            }
            
        }
    }
}