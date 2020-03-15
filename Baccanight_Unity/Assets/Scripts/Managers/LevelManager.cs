using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField]
    private GameObject m_fonduBegin;

    [SerializeField]
    private GameObject m_fonduEnd;

    [SerializeField]
    private PlayerState m_playerState;
#pragma warning restore 0649
    #endregion

    #region Variables
    private int m_idBehindDoor;
    private int m_idLevelAimed;
    #endregion

    #region Getters / Setters
    public int ScenePersistentPlayer { get; } = 1;
    #endregion

    private void Start()
    {
        StartCoroutine(PlayerFirstSpawn(2));
        BehindDoor(-1, -1);
        PlayerManager.Instance.ResetPlayerData();
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
        int idBehind = m_idBehindDoor;
        int idLevel = m_idLevelAimed;

        if (IsIdValid(idBehind, idLevel))
        {
            ChangeScene(idBehind,idLevel);
        }
    }

    public void ChangeScene(int idBehindDoor, int idLevelAimed)
    {
        StartCoroutine(TeleportPlayer(idLevelAimed, idBehindDoor));
        BehindDoor(-1, -1);
    }

    public IEnumerator TeleportPlayer(int build, int doorId)
    {
        m_playerState.State = GameState.inLoading;
        Instantiate(m_fonduBegin);

        yield return new WaitForSeconds(1f);

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if(scene.buildIndex != ScenePersistentPlayer && scene.buildIndex != build)
            {
                SceneManager.UnloadSceneAsync(scene.buildIndex);
            }
        }

        yield return SceneManager.LoadSceneAsync(build, LoadSceneMode.Additive);
        Instantiate(m_fonduEnd);
        
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
        if (testDoor) Debug.LogError("Aucune porte n'a été trouvé");

        yield return new WaitForSeconds(1f);

        m_playerState.State = GameState.inGame;

        yield return 0;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsIdValid(int idBehindDoor, int idLevelAimed)
    {
        return (idBehindDoor != -1 && idLevelAimed != -1);
    }

    public IEnumerator PlayerFirstSpawn(int build)
    {
        SceneManager.LoadScene(build, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(ScenePersistentPlayer, LoadSceneMode.Additive);

        while(!SceneManager.GetSceneByBuildIndex(build).isLoaded)
        {
            yield return null;
        }

        Instantiate(m_fonduEnd);

        yield return new WaitForSeconds(1f);

        m_playerState.State = GameState.inGame;
    }
}
