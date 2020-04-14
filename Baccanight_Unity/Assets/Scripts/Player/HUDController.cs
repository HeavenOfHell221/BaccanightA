using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private PlayerState m_playerState;
    [SerializeField] private GameObject m_pauseMenu;

    public void OnEscape()
    {
        switch (m_playerState.State)
        {
            case GamePlayerState.inPause:
                m_playerState.State = GamePlayerState.inGame;
                m_pauseMenu.gameObject.SetActive(false);
                GameConstants.GameIsPaused = false;
                break;
            case GamePlayerState.inGame:
                m_playerState.State = GamePlayerState.inPause;
                m_pauseMenu.gameObject.SetActive(true);
                GameConstants.GameIsPaused = true;
                break;
            default: break;
        }
    }
}
