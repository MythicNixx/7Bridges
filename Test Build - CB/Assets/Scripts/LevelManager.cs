using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneToLoad = "";

    private bool objectiveComplete = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && objectiveComplete)
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}
