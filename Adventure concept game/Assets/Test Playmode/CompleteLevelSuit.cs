using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

namespace Tests
{
    public class CompleteLevelSuit
    {
        private InteractiveActionController appleInter;
        private SpriteRenderer appleSprite;
        private DialogueActivation npcDialogueActivation;
        private ResponseButtonsController responsesButtons;

        public void Setup()
        {
            appleInter = GameObject.Find("Apple").GetComponent<InteractiveActionController>();
            appleSprite = appleInter.gameObject.GetComponentInChildren<SpriteRenderer>();
            npcDialogueActivation = GameObject.Find("NPC").GetComponent<DialogueActivation>();
            responsesButtons = GameObject.FindObjectOfType<ResponseButtonsController>();
        }

        public Button GetYesButton()
        {
            for (int i = 0; i < responsesButtons.AnswerUI.transform.childCount; i++)
            {
                var button = responsesButtons.AnswerUI.transform.GetChild(i).GetComponentInChildren<Button>();
                Assert.NotNull(button);

                var text = button.GetComponent<TextMeshProUGUI>().text;

                if (text == "Yes")
                {
                    return button;
                }
            }

            return null;
        }

        [UnityTest]
        public IEnumerator CompleteLevelSuitWithEnumeratorPasses()
        {
            SceneManager.LoadScene("TestScene");

            yield return null;

            Setup();

            appleInter.ActivateActions();

            yield return new WaitForSeconds(0.1f);

            Assert.AreEqual(appleSprite.gameObject.activeSelf, false);

            DialogueManager.instance.StartDialogueSequence(npcDialogueActivation.StartDialogue);

            while (responsesButtons.AnswerUI.transform.childCount == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            var yesButton = GetYesButton();

            Assert.NotNull(yesButton);

            yesButton.onClick.Invoke();

            yield return new WaitForSeconds(6f);

            Assert.AreEqual(SceneManager.GetSceneByName("EndScene"), SceneManager.GetActiveScene());

            yield return null;
        }
    }
}
