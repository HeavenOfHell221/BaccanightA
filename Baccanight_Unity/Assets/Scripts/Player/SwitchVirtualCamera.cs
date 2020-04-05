using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchVirtualCamera : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField] private GameObject m_camera;
    [SerializeField] [Range(3f, 12f)] private float m_orthographicSize = 6;
    [SerializeField] private bool m_cameraFollowPlayer = true;
    [SerializeField] Transform m_transformFollow;
#pragma warning restore 0649
    #endregion

    #region Variables
    private CinemachineVirtualCamera m_CVCamera;
    #endregion

    private void Start()
    {
        m_CVCamera = m_camera.GetComponent<CinemachineVirtualCamera>();
        m_camera.SetActive(false);
        m_CVCamera.m_Lens.OrthographicSize = m_orthographicSize;
    }

    public void CameraSwitch(GameObject player)
    {
        if (m_cameraFollowPlayer)
        { 
            m_CVCamera.Follow = player.transform;
            m_CVCamera.LookAt = player.transform.Find("Look");
        }
        else
        {
            m_CVCamera.Follow = m_transformFollow;
        }
        StartCoroutine(PlayerManager.Instance.SetCameraReference(m_camera));
    }

    public void ExitConfiner(GameObject player)
    {
        if (PlayerManager.Instance.LastCamera)
        {
            if (PlayerManager.Instance.LastCamera == m_camera)
            {
                PlayerManager.Instance.LastCamera = null;
            }
            else
            {
                StartCoroutine(PlayerManager.Instance.SetCameraReference(PlayerManager.Instance.LastCamera));
            }
        }
    }
}
