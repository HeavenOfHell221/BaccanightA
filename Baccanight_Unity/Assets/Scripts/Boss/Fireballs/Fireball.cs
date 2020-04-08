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
    [SerializeField] [Range(0f, 0.5f)] protected float m_timeBeforeMove = 0.1f;
    [SerializeField] [Range(0, -2)] protected int m_damage = -1;
#pragma warning restore 0649
    #endregion

    #region Getters / Setters
    public Rigidbody2D Rigidbody { get; private set; }
    public float Speed { get => m_speed; set => m_speed = value; }
    #endregion

    protected abstract void Start();
    protected abstract void Move();
    protected void OnEnable()
    {
        Start();
        Invoke("DesactiveObject", 10f);
    }

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Invoke("DesactiveObject", 10f);
    }

    public virtual void OnEnterPlayer(GameObject player)
    {
        player.GetComponent<Health>().ModifyHealth(m_damage, gameObject);
        CancelInvoke();
        DesactiveObject();
    }

    private void DesactiveObject()
    {
        Rigidbody.velocity = Vector2.zero;
        gameObject.SetActive(false);     
    }
}
