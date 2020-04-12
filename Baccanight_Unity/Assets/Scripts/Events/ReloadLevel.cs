using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadLevel : MonoBehaviour
{

    public void PlayerEnter()
    {
        PlayerManager.Instance.PlayerInputController.OnInteract.AddListener(Reload);
    }

    public void PlayerExit()
    {
        PlayerManager.Instance.PlayerInputController.OnInteract.RemoveListener(Reload);
    }

    [ContextMenu("Reload")]
    public void Reload()
    {
        StartCoroutine(LevelManager.Instance.ReloadScene());
    }
}
