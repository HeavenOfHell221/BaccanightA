using UnityEngine;

public class ArrowDetection : Arrow
{
    [SerializeField] GameObject m_FX;

    public void OnArrowEnter(GameObject target)
    {
        if(target.layer == LayerMask.NameToLayer("Stage"))
        {
            StopArrow();
            return;
        }

        switch (target.tag)
        {
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
        Instantiate(m_FX, transform.position, Quaternion.identity, null);
        m_isStopping = true;
        m_rigidbody.velocity = Vector2.zero;
        StartCoroutine(Disable(0.1f));
    }
}