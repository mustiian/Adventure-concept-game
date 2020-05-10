using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;
using UnityEngine.SceneManagement;

public class LoadSceneStage : Stage
{
    public string SceneName;

    public override bool ConditionToFinish()
    {
        return !isFinished;
    }

    public override void InitStage()
    {
        Load(SceneName);
    }

    public override void UpdateStage()
    {
    }

    public void Load(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }
}
