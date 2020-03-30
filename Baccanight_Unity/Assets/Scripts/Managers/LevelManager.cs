using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField]
    private PlayerState m_playerState;
#pragma warning restore 0649
    #endregion

    #region Variables
    private int m_idBehindDoor;
    private int m_idLevelAimed;
    private int m_sceneActive;
    #endregion

    #region Getters / Setters
    public int ScenePersistentPlayer { get; } = 1;
    #endregion

    private void Start()
    {
        BehindDoor(-1, -1);
        PlayerManager.Instance.ResetPlayerData();
    }

    public void Play()
    {
        StartCoroutine(PlayerFirstSpawn(2));
    }

    public void BehindDoor(int idDoor, int idLevelAimed)
    {
        m_idBehindDoor = idDoor;
        m_idLevelAimed = idLevelAimed;
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

        GameObject levelLoader = GameObject.FindGameObjectWithTag("LevelLoader");
        DontDestroyOnLoad(levelLoader);
        Animator animator = levelLoader.GetComponentInChildren<Animator>();
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.UnloadSceneAsync(m_sceneActive);

        yield return SceneManager.LoadSceneAsync(build, LoadSceneMode.Additive);

        Destroy(levelLoader);

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        bool testDoor = true;
        
        foreach (GameObject obj in doors)
        {
            DoorTeleportation door = obj.GetComponent<DoorTeleportation>();
            if(door)
            {
                if (door.GetDoorId() == doorId)
                {
                    GameObject player = PlayerManager.Instance.PlayerReference;
                    if(player)
                    {
                        player.transform.position = door.GetSpawnPosition();
                        PlayerManager.Instance.CameraReference.TeleportCamera(player.transform);
                        testDoor = !testDoor;
                    }
                    break;
                }
            }
        }
        if (testDoor)
        {
            Debug.LogError("Aucune porte n'a été trouvé");
        }

        m_sceneActive = build;
        yield return new WaitForSeconds(1f);
        m_playerState.State = GameState.inGame;
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
        m_sceneActive = build;
        SceneManager.LoadScene(build, LoadSceneMode.Single);
        SceneManager.LoadScene(ScenePersistentPlayer, LoadSceneMode.Additive);

        while (!SceneManager.GetSceneByBuildIndex(build).isLoaded
            || !SceneManager.GetSceneByBuildIndex(ScenePersistentPlayer).isLoaded)
        {
            yield return null;
        }

        m_playerState.State = GameState.inGame;
    }
}
