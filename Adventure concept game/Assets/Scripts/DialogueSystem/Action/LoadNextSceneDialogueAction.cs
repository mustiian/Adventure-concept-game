using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneDialogueAction : DialogueAction
{
    public SceneChangerDrivenCondition sceneChangerCondition;

    public override void Activate()
    {
        sceneChangerCondition.SetActivation(true);
    }
}
