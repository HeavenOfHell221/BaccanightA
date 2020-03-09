using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonBehaviour<PlayerManager>
{
    [HideInInspector]
    public GameObject PlayerReference;
    [HideInInspector]
    public CameraController CameraReference;
}
