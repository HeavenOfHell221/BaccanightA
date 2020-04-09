using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetection : Arrow
{
    public void OnArrowEnter(GameObject target)
    {
        switch(target.tag)
        {
            case "Stage":
                StopArrow();
                break;
            case "BossHealth":
                target.GetComponent<HealthBoss>().ModifyHealth(m_damage);
                PlayerManager.Instance.ShakeCamera.Shake(0.75f, 1f, 0.15f);
                //gameObject.transform.SetParent(target.transform.parent, true);
                StopArrow();
                break;
            case "BossShield":
                target.GetComponent<ShieldAttack>().CounterAttackEvent.Invoke(BossActionType.CounterAttack);
                //gameObject.transform.SetParent(target.transform.parent, true);
                StopArrow();
                break;
            default:
                break;
        }
    }

    private void StopArrow()
    {
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine(Disable(1f));
    }
}