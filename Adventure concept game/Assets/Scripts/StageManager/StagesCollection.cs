using System.Collections.Generic;
using UnityEngine;

namespace StageManager
{
    public class StagesCollection : MonoBehaviour
    {
        private List<Stage> stages = new List<Stage>();

        private void Awake()
        {
            foreach (var stage in GetComponentsInChildren<Stage>())
            {
                stages.Add(stage);
            }
        }

        private int nextStageIndex = 0;

        public Stage GetNextStage()
        {
            if (nextStageIndex < stages.Count)
            {
                return stages[nextStageIndex++];
            }
            else
                return null;
        }

        public void Restart()
        {
            for (int i = 0; i < stages.Count; i++)
            {
                stages[i].isFinished = false;
            }

            nextStageIndex = 0;
        }
    }
}


