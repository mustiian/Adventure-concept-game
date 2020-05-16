using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class SheepStageManager : StagesManager
{
    public bool RepeatStages;

    private void Awake()
    {
        if (RepeatStages)
        {
            OnFinishStages += FinishStages;
        }
    }

    private void FinishStages(StagesManager manager)
    {
        Stages.Restart();

        StartNextStage();
    }
}
