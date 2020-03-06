using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    public void changeScene(int build)
    {
        SceneManager.LoadScene(build);
    }

    public void changeScene(int build, int doorId)
    {
        StartCoroutine(teleportPlayer(build, doorId));
    }

    public IEnumerator teleportPlayer(int build, int doorId)
    {
        yield return SceneManager.LoadSceneAsync(build);

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bool testDoor = true;
        
        foreach (GameObject obj in doors)
        {
            if (obj.GetComponent<Door>().getDoorId() == doorId)
            {
                //Debug.Log(obj.GetComponent<Door>().getSpawnPosition());
                player.transform.position = obj.GetComponent<Door>().getSpawnPosition();
                testDoor = !testDoor;
                break;
            }
        }
        if (testDoor) Debug.Log("Aucune porte n'a été trouvé");
        yield return 0;

    }

    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
