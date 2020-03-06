using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    #region Inspector
#pragma warning disable 0649

    [SerializeField]
    private int m_DoorId; 

    [SerializeField]
    private bool m_RightToLeft; // pour placer le joueur=

    [SerializeField]
    private int m_LevelIdAimed; // pour chercher la scene to be loaded

    [SerializeField]
    private float m_offsetSpawnX = 2;

#pragma warning restore 0649
    #endregion

    private Vector3 m_spawnPoint;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        m_spawnPoint = transform.position;
        int dir = m_RightToLeft ? -1 : 1;
        m_spawnPoint.x += m_offsetSpawnX * dir;
        if (m_DoorId == -1) Debug.LogError("Porte " + this.name + " ID NOT CORRECT");
        if (m_LevelIdAimed == -1) Debug.LogError("Porte " + this.name + " Level Id aimed NOT CORRECT");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            
            changeLevel();
        }
    }

    public void changeLevel()
    {
        LevelManager.Instance.changeScene(m_LevelIdAimed, m_DoorId);
    }

    public int getDoorId()
    {
        return m_DoorId;
    }
    public Vector3 getSpawnPosition()
    {
        return m_spawnPoint;
    }



}
