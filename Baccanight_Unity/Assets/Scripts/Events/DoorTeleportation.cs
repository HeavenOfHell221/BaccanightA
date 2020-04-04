using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportation : MonoBehaviour
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
    private Vector2 m_offsetSpawn;

#pragma warning restore 0649
    #endregion

    private Vector3 m_spawnPoint;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        m_spawnPoint = transform.position;
        int dir = m_RightToLeft ? -1 : 1;
        m_spawnPoint.x += m_offsetSpawn.x * dir;
        m_spawnPoint.y += m_offsetSpawn.y;
        if (m_DoorId == -1) Debug.LogError("Porte " + this.name + " ID NOT CORRECT");
        if (m_LevelIdAimed == -1) Debug.LogError("Porte " + this.name + " Level Id aimed NOT CORRECT");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.PlayerInputController.OnInteract.AddListener(LevelManager.Instance.OnInteract);
            LevelManager.Instance.BehindDoor(m_DoorId, m_LevelIdAimed);    
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerManager.Instance.PlayerInputController.OnInteract.RemoveListener(LevelManager.Instance.OnInteract);
            LevelManager.Instance.BehindDoor(-1, -1);
        }
    }

    public int GetDoorId()
    {
        return m_DoorId;
    }
    public Vector3 GetSpawnPosition()
    {
        return m_spawnPoint;
    }

}
