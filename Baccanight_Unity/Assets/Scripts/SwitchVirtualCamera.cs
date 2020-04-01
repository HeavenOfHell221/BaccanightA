using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchVirtualCamera : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField]
    private GameObject m_camera;
#pragma warning restore 0649
    #endregion

    #region Variables
    private CinemachineVirtualCamera m_CVCamera;
    #endregion

    private void Start()
    {
        m_CVCamera = m_camera.GetComponent<CinemachineVirtualCamera>();
        m_camera.SetActive(false);
    }

    public void CameraSwitch(GameObject player)
    {
        m_CVCamera.Follow = player.transform;
        m_CVCamera.LookAt = player.transform.Find("Look");
        PlayerManager.Instance.SetCameraReference(m_camera);
    }
}
