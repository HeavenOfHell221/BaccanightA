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
    public void SetSpeed(float speed) => m_speed = speed;
    #endregion

    protected abstract void Start();
    protected abstract void Move();
    protected void OnEnable()
    {
        Start();
        Invoke("DesactiveObject", 6f);
    }

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Invoke("DesactiveObject", 10f);
    }

    public virtual void OnEnterPlayer(GameObject player)
    {
        player.GetComponent<Health>().ModifyHealth(-1, gameObject);
        CancelInvoke();
        DesactiveObject();
    }

    private void DesactiveObject()
    {
        Rigidbody.velocity = Vector2.zero;
        gameObject.SetActive(false);     
    }
}
