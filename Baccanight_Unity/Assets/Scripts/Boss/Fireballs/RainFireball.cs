using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFireball : Fireball
{
    #region Inpsector
#pragma warning disable 0649

#pragma warning restore 0649
    #endregion

    #region Variables
    #endregion

    private void Start()
    {
        StartCoroutine(_Move());
    }

    private IEnumerator _Move()
    {
        yield return new WaitForSeconds(m_timeBeforeMove);
        Move();
    }

    protected override void Move()
    {
        Vector3 direction = Vector3.down;
        Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }
}
