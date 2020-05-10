using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string LoadSceneName;
    public string UploadSceneName;

    ScenesController scenesController;

    private void Start()
    {
        scenesController = FindObjectOfType<ScenesController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Debug.Log("Scenes are changed");

        if (LoadSceneName.Length != 0)
        {
            if (!SceneManager.GetSceneByName(LoadSceneName).isLoaded)
                scenesController.Load(LoadSceneName);
        }

        if (UploadSceneName.Length != 0)
        {
            if (SceneManager.GetSceneByName(UploadSceneName).isLoaded)
                StartCoroutine(UploadScene(UploadSceneName));
        }
    }

    private IEnumerator UploadScene(string scene)
    {
        yield return new WaitForSeconds(0.05f);
        scenesController.Upload(UploadSceneName);
    }
}
