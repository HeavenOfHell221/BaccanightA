using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXRandomRotation : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 360f)), Space.World);
    }
}
