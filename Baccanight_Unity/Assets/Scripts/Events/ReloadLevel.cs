using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadLevel : MonoBehaviour
{
    [ContextMenu("Reload")]
    public void Reload()
    {
        StartCoroutine(LevelManager.Instance.ReloadScene());
    }
}
