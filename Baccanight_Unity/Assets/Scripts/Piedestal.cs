using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piedestal : MonoBehaviour
{
    public void CollisionEnter(GameObject obj)
    {
        if(obj.tag == "Statue")
        {
            Debug.Log("Statue !");
        }
    }
}
