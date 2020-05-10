using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class SetActiveObjectsStage : Stage
{
    public enum ActiveType { Activate, Deactivate }

    public ActiveType ActionType;

    public GameObject[] Objects;

    public override bool ConditionToFinish()
    {
        return !isFinished;
    }

    public override void InitStage()
    {
        if (ActionType == ActiveType.Activate)
            SetActive(true);
        else
            SetActive(false);

        StartCoroutine(WaitExitCoroutine(0.1f));
    }

    public override void UpdateStage()
    {
    }

    private void SetActive(bool value)
    {
        for (int i = 0; i < Objects.Length; i++)
        {
            Objects[i].SetActive(value);
        }
    }

    IEnumerator WaitExitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        ExitStage();
    }
}
