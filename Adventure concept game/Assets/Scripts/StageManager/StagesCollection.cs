using UnityEngine;

namespace StageManager
{
    public class StagesCollection : MonoBehaviour
    {
        public Stage[] stages;

        private int nextStageIndex = 0;

        public Stage GetNextStage()
        {
            if (nextStageIndex < stages.Length)
            {
                return stages[nextStageIndex++];
            }
            else
                return null;
        }

        public void Restart()
        {
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].isFinished = false;
            }

            nextStageIndex = 0;
        }
    }
}


