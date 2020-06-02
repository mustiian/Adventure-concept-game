using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using StageManager;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class SceneLoaderSuit
    {
        [UnityTest]
        public IEnumerator SceneLoaderSuitWithEnumeratorPasses()
        {
            SceneManager.LoadScene("TestScene");

            yield return null;

            var sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneChangerManager>();

            Assert.IsNotNull(sceneLoader);

            yield return new WaitForSeconds(4f);

            for (int i = 0; i < sceneLoader.Stages.transform.childCount; i++)
            {
                var stage = sceneLoader.Stages.transform.GetChild(i).GetComponent<Stage>();
                Assert.AreEqual(stage.isFinished, true);
            }
            yield return null;
        }
    }
}
