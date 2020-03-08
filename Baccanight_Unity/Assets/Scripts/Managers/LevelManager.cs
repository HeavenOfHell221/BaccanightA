using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBehaviour<LevelManager>
{ 
    #region Variables

    private int m_idBehindDoor;
    private int m_idLevelAimed;
    #endregion


    private void Start()
    {
        BehindDoor(-1, -1);
    }

    public void BehindDoor(int idDoor, int idLevelAimed)
    {
        m_idBehindDoor = idDoor;
        m_idLevelAimed = idLevelAimed;
    }

    public void LoadScene(int build)
    {
        SceneManager.LoadScene(build);
    }

    public void OnInteract()
    {
        if (LevelManager.Instance.isIdValid())
        {
            LevelManager.Instance.ChangeScene();
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(TeleportPlayer(m_idLevelAimed, m_idBehindDoor));
        BehindDoor(-1, -1);

    }

    public IEnumerator TeleportPlayer(int build, int doorId)
    {
        //Debug.Log("Salut");
        yield return SceneManager.LoadSceneAsync(build);

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bool testDoor = true;
        
        foreach (GameObject obj in doors)
        {
            if (obj.GetComponent<Door>().GetDoorId() == doorId)
            {
                //Debug.Log(obj.GetComponent<Door>().getSpawnPosition());
                player.transform.position = obj.GetComponent<Door>().GetSpawnPosition();
                testDoor = !testDoor;
                break;
            }
        }
        if (testDoor) Debug.Log("Aucune porte n'a été trouvé");
        yield return 0;
    }

    

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool isIdValid()
    {
        return (m_idBehindDoor != -1 && m_idLevelAimed != -1);
    }
}
