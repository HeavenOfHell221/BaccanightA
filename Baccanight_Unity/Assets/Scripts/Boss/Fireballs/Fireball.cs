using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fireball : MonoBehaviour
{
    #region Inpsector
#pragma warning disable 0649
    [Header("Generics Attributes", order = 0)]
    [Space(5)]
    [SerializeField] private float m_speed;
    [SerializeField] protected float m_timeBeforeMove;
#pragma warning restore 0649
    #endregion

    #region Getters / Setters
    public Rigidbody2D Rigidbody { get; private set; }
    public float Speed { get => m_speed; private set => m_speed = value; }
    #endregion

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected abstract void Move();
    public virtual void OnEnterPlayer(GameObject player) => Destroy(gameObject);
    public void SetSpeed(float speed) => m_speed = speed;
}
