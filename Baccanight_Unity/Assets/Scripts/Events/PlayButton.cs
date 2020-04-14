using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void Play()
    {
        LevelManager.Instance.Play();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
