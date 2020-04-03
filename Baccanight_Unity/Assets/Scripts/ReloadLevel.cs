using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadLevel : MonoBehaviour
{
    public void Reload()
    {
        LevelManager.Instance.ReloadScene();
    }
}
