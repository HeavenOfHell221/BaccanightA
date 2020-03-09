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

    #region Getters / Setters
    public int ScenePersistentPlayer { get; } = 1;
    #endregion

    private void Start()
    {
        PlayerSpawn(2);
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
        if (IsIdValid())
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        StartCoroutine(TeleportPlayer(m_idLevelAimed, m_idBehindDoor));
        BehindDoor(-1, -1);

    }

    public IEnumerator TeleportPlayer(int build, int doorId)
    {
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.buildIndex != ScenePersistentPlayer && scene.buildIndex != build)
            {
                SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
        }

        yield return SceneManager.LoadSceneAsync(build, LoadSceneMode.Additive);

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        bool testDoor = true;
        
        foreach (GameObject obj in doors)
        {
            Door door = obj.GetComponent<Door>();
            if(door)
            {
                if (door.GetDoorId() == doorId)
                {
                    GameObject player = PlayerManager.Instance.PlayerReference;
                    if(player)
                    {
                        player.transform.position = obj.GetComponent<Door>().GetSpawnPosition();
                        PlayerManager.Instance.CameraReference.TeleportCamera(player.transform);
                        testDoor = !testDoor;
                    }
                    break;
                }
            }
        }
        if (testDoor) Debug.Log("Aucune porte n'a été trouvé");
        yield return 0;
    }

    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsIdValid()
    {
        return (m_idBehindDoor != -1 && m_idLevelAimed != -1);
    }

    public void PlayerSpawn(int build)
    {
        SceneManager.LoadScene(build, LoadSceneMode.Single);
        SceneManager.LoadScene(ScenePersistentPlayer, LoadSceneMode.Additive);
    }
}
