using UnityEngine;
using System;

namespace StageManager
{
    public abstract class Stage : MonoBehaviour
    {
        public bool isFinished = false;

        public Action<Stage> OnExitAction;

        public abstract bool ConditionToFinish();

        public abstract void InitStage();
        public abstract void UpdateStage();
        public virtual void ExitStage()
        {
            isFinished = true;
            OnExitAction?.Invoke(this);
        }
    }
}

