using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttack : BossAttack
{
    [SerializeField] private Collider2D m_collider;
    [SerializeField] private GameObject m_fireball;
    [SerializeField] [Range(0.1f, 2f)] private float m_cooldown;
    [SerializeField] [Min(3f)] private float m_DistanceMinPlayer = 10f;

    private Transform m_player;

    [ContextMenu("Handle Fire Ball")]
    public override void StartAttack()
    {
        m_player = PlayerManager.Instance.PlayerReference.transform;
        IsStarted = true;
        StartCoroutine(HandleAttack());
    }

    public override IEnumerator HandleAttack()
    {
        InProgress = true;
        Vector3 center = m_collider.bounds.center;
        Vector3 extents = m_collider.bounds.extents;
        Vector3 spawn;
        do
        {
            yield return null;
            spawn = new Vector3(
            Random.Range((center.x - extents.x), (center.x + extents.x)),
            Random.Range((center.y - extents.y), (center.y + extents.y)),
            0f);
        } while (Vector2.Distance(spawn, m_player.position) < m_DistanceMinPlayer);
        

        Instantiate(m_fireball, spawn, new Quaternion());

        InProgress = false;
        yield return new WaitForSeconds(m_cooldown);
        StartCoroutine(HandleAttack());
    }

    public override void EndAttack()
    {
        IsFinish = true;
    }
}
