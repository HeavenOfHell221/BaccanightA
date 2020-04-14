using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handle the actions for the pause buttons.
/// </summary>
public class PauseMenuActions : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField] private PlayerState m_playerState;

#pragma warning restore 0649
    #endregion

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void Resume()
    {
        m_playerState.State = GamePlayerState.inGame;
        gameObject.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}