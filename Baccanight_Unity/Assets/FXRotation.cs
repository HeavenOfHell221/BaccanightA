using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXRotation : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_animator.enabled = true;
        gameObject.transform.Rotate(new Vector3(0f, 0f, Random.Range(-180f, 180)), Space.World);
    }
}
