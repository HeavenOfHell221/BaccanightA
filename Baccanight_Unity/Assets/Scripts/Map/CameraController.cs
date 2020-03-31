using Cinemachine;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera CVirtualCamera;
    public CinemachineConfiner CConfiner;

    private void Awake()
    {
        PlayerManager.Instance.CameraReference = this;
    }

    private void Start()
    {
        StartGetConfinerCamera();
    }

    public void TeleportCamera(Transform player)
    {
        Vector3 delta = player.transform.position - transform.position;
        CVirtualCamera.OnTargetObjectWarped(player, delta);
    }

    public void StartGetConfinerCamera()
    {
        StartCoroutine(GetConfinerCamera());
    }

    private IEnumerator GetConfinerCamera()
    {
        GameObject confiner;
        CConfiner.m_BoundingShape2D = null;

        while (CConfiner.m_BoundingShape2D == null)
        {
            confiner = GameObject.FindGameObjectWithTag("ConfinerCamera");
             
            if (confiner)
            {
                CConfiner.m_BoundingShape2D = confiner.GetComponent<CompositeCollider2D>();
            }

            yield return null;
        }

        CConfiner.InvalidatePathCache();
    }
}
