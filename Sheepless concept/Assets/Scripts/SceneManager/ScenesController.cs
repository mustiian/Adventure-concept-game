using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{

    public void Load(string scene)
    {
        AsyncOperation operationScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        operationScene.allowSceneActivation = false;

        StartCoroutine(ActivateScene(operationScene));
    }

    public void Upload(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    IEnumerator ActivateScene(AsyncOperation sceneOperation)
    {
        yield return new WaitForSeconds(0.1f);

        sceneOperation.allowSceneActivation = true;
    }
}
