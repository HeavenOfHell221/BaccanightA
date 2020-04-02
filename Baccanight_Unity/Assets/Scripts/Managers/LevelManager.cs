using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

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
    private GameObject m_levelLoader;
    private DoorTeleportation m_door;
    #endregion

    #region Getters / Setters

    #endregion

    private void Start()
    {
        PlayerManager.Instance.ResetPlayerData();
        BehindDoor(-1, -1);
    }

    public void Play()
    {
        StartCoroutine(PlayerFirstSpawn(GameConstants.SceneLobby));
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
        //BehindDoor(-1, -1);
    }

    private void LevelLoaderStart()
    {
        m_levelLoader = GameObject.FindGameObjectWithTag("LevelLoader");
        DontDestroyOnLoad(m_levelLoader);
        m_levelLoader.GetComponentInChildren<Animator>().SetTrigger("Start");     
    }

    private void SetDoors(int doorId)
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        bool testDoor = false;
        GameObject player = PlayerManager.Instance.PlayerReference;

        foreach (GameObject obj in doors)
        {
            DoorTeleportation door = obj.GetComponent<DoorTeleportation>();
            if (door.GetDoorId() == doorId)
            {
                player.transform.position = door.GetSpawnPosition();
                testDoor = true;
                m_door = door;
                break;
            }
        }
        if (!testDoor)
        {
            Debug.LogError("Aucune porte n'a été trouvé");
        }
    }

    public IEnumerator TeleportPlayer(int build, int doorId)
    {
        m_playerState.State = GamePlayerState.inLoading;
        CinemachineBrain brain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();       
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        LevelLoaderStart();
        yield return new WaitForSeconds(1f);

        PlayerManager.Instance.ResetCameraReference();

        SceneManager.UnloadSceneAsync(m_sceneActive);
        yield return SceneManager.LoadSceneAsync(build, LoadSceneMode.Additive);

        Destroy(m_levelLoader);
        SetDoors(doorId);
        m_sceneActive = build;

        yield return new WaitForSeconds(1f);
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;

        m_playerState.State = GamePlayerState.inGame;
    }

    public void ReloadScene()
    {
        StartCoroutine(TeleportPlayer(m_sceneActive, m_door.GetDoorId()));
    }

    public bool IsIdValid(int idBehindDoor, int idLevelAimed)
    {
        return (idBehindDoor != -1 && idLevelAimed != -1);
    }

    public IEnumerator PlayerFirstSpawn(int build)
    {
        m_sceneActive = build;
        SceneManager.LoadScene(build, LoadSceneMode.Single);
        SceneManager.LoadScene(GameConstants.ScenePersistentPlayer, LoadSceneMode.Additive);

        while (!SceneManager.GetSceneByBuildIndex(build).isLoaded || !SceneManager.GetSceneByBuildIndex(GameConstants.ScenePersistentPlayer).isLoaded)
        {
            yield return null;
        }

        m_playerState.State = GamePlayerState.inGame;
    }
}
