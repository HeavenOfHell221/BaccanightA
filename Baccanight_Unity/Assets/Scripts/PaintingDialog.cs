using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingDialog : MonoBehaviour
{
    private DialogBox m_dialogBox;

    private void Start()
    {
        GameObject box = GameObject.FindGameObjectWithTag("DialogBox");
        if(box)
        {
            m_dialogBox = box.GetComponent<DialogBox>();
        }
    }


    public void EnterPlayer(GameObject player)
    {

    }
}
