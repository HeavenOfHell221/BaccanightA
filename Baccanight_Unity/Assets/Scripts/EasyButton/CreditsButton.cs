using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void GoToCredits()
    {
        SceneManager.LoadScene(10);
    }

    public void ReturnFromCredits()
    {
        SceneManager.LoadScene(0);
    }
}
