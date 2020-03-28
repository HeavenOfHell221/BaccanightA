using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Arrow;

    void Start()
    {
        
    }

    public void OnShoot()
    {
        Instantiate(Arrow, gameObject.transform.position, new Quaternion());;
    }
}
