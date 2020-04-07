using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("Attributes")]
    [Space(5)]

    public GameObject m_rainball;
    public int maxNumberOfBall;
    public Vector3 initialSpawn;
    [SerializeField] [Range(0f, 1f)] private float m_ballPourcentage;
    [SerializeField] [Range(0, 10)] private int m_voidSpace;

    [Header("Normal")]
    [Space(5)]

    [SerializeField] [Range(0.1f, 25f)] private float m_ballSpeedFlat;
    [SerializeField] [Range(0.1f, 2f)] private float m_cooldown;
    [SerializeField] [Range(1, 10)] private int m_loopToBeDone;

    [Header("Upgraded Modifier")]
    [Space(5)]

    [SerializeField] [Range(1f, 2f)] private float m_upgradeSpeedRelative;
    [SerializeField] [Range(0.1f, 2f)] private float m_newCooldown;
    [SerializeField] [Range(1, 10)] private int m_newLoopToBeDone;
#pragma warning restore 0649
    #endregion

    #region Variables

    private int m_loopToDo;
    private int m_loopDone;
    private int m_line;
    #endregion

    public void Start()
    {
        m_loopToDo = m_loopToBeDone;
        m_rainball.GetComponent<RainFireball>().Speed = m_ballSpeedFlat;
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        m_loopDone = m_loopToDo;
        StartCoroutine(HandleAttack());
    } 

    protected override IEnumerator HandleAttack()
    {
        CreateLine();
        TransformLine();
        m_loopDone--;

        yield return new WaitForSeconds(m_cooldown);
        if (m_loopDone > 0 && !IsCanceled)
        {
            StartCoroutine(HandleAttack());
        }
        else
        {
            EndAttack();
        }
    }

    private void CreateLine()
    {
        bool atLeastOneSpace = false;
        int currentLine = 1;
        for (int i = 1; i < maxNumberOfBall;)
        {
            float test = Random.Range(0f, 1f);
            if (test <= m_ballPourcentage)
            {
                currentLine <<= 1;
                currentLine++;
                i++;
            }
            else
            {
                atLeastOneSpace = true;
                currentLine <<= m_voidSpace;
                i += m_voidSpace;
            }
        }
        if (!atLeastOneSpace)
        {
            //ajouter un espace au milieu de la ligne (1111111111111111 -> 1111111001111111)
            
        }
        m_line = currentLine;
    }

    private void TransformLine()
    {
        Vector3 spawn = initialSpawn;
        while (m_line != 0) {
            if ((m_line & 1) == 1)
            {
                m_line >>= 1;
                GameObject fireball = ObjectPooler.Instance.SpawnFromPool(m_rainball, spawn);
                fireball.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
                spawn += Vector3.right;
            }
            else
            {
                m_line >>= m_voidSpace;
                spawn += (Vector3.right * m_voidSpace);
            }
        }
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_cooldown = m_newCooldown;
        m_rainball.GetComponent<RainFireball>().Speed = m_upgradeSpeedRelative * m_ballSpeedFlat;
        m_loopToDo = m_newLoopToBeDone;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack(); 
    }
}
