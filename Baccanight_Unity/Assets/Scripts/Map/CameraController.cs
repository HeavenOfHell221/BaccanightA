using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;

    void Awake()
    {
        PlayerManager.Instance.CameraReference = this;
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    public void TeleportCamera(Transform player)
    {
        Vector3 delta = player.transform.position - transform.position;
        cvc.OnTargetObjectWarped(player, delta);
    }
}
