using UnityEngine;
using System;

namespace StageManager
{
    public class StagesManager : MonoBehaviour
    {
        public StagesCollection Stages;

        public Action<StagesManager> OnFinishStages;
        public Action<StagesManager> OnChangeStage;

        protected bool isFinished = false;

        protected Stage currentStage;

        protected virtual void Start()
        {
            StartNextStage();
        }

        public bool StartNextStage()
        {
            currentStage = Stages.GetNextStage();

            if (currentStage != null)
            {
                currentStage.OnExitAction += StageFinish;
                currentStage.InitStage();
                return true;
            }
            else
                isFinished = true;

            return false;
        }

        protected virtual void Update()
        {
            if (currentStage != null)
                currentStage.UpdateStage();
        }

        protected virtual void StageFinish(Stage stage)
        {
            stage.OnExitAction = null;

            if (!StartNextStage())
            {
                OnFinishStages?.Invoke(this);
            }
            else
            {
                OnChangeStage?.Invoke(this);
            }
        }

        public bool IsFinished()
        {
            return isFinished;
        }
    }
}
