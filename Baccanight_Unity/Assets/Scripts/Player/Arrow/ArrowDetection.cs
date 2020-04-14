using UnityEngine;

public class ArrowDetection : Arrow
{
    public void OnArrowEnter(GameObject target)
    {
        switch (target.tag)
        {
            case "Stage":
                StopArrow();
                break;
            case "BossHealth":
                target.GetComponent<HealthBoss>().ModifyHealth(m_damage);
                PlayerManager.Instance.ShakeCamera.Shake(1f, 1f, 0.15f);
                StopArrow();
                break;
            case "BossShield":
                if (!m_isStopping)
                {
                    target.GetComponent<ShieldAttack>().HandleArrowHit();
                }
                StopArrow();
                break;
            default:
                break;
        }
    }

    private void StopArrow()
    {
        m_isStopping = true;
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine(Disable(1f));
    }
}