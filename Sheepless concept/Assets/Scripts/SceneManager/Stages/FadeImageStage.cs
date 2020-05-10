using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class FadeImageStage : Stage
{
    public enum FadeType { FadeIn, FadeOut }

    public FadeType Type;

    public FaderController Fader;

    public float Duration;

    public override bool ConditionToFinish()
    {
        return !Fader.IsFading();
    }

    public override void InitStage()
    {
        switch (Type)
        {
            case FadeType.FadeIn:
                Fader.FadeIn(Duration);
                break;
            case FadeType.FadeOut:
                Fader.FadeOut(Duration);
                break;
            default:
                break;
        }
    }

    public override void UpdateStage()
    {
        if (ConditionToFinish())
            ExitStage();
    }
}
