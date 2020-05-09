using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponseButtonsController : MonoBehaviour
{
    public GameObject ButtonPrefab;

    public GameObject AnswerUI;

    private List<GameObject> ActiveButtons = new List<GameObject>();

    public Animator BackgroundPanelAnimator;

    public Vector2 Offset;
    public Vector2 Position;


    public void AddButton(Dialogue response, int index)
    {
        GameObject buttonObject = (GameObject)Instantiate (ButtonPrefab, Vector3.zero, AnswerUI.transform.rotation, AnswerUI.transform);
        Button button = buttonObject.GetComponent<Button> ();
        buttonObject.GetComponentInChildren<TextMeshProUGUI> ().SetText (response.ActiveSentence.Sentence);
        button.onClick.AddListener (response.ChooseDialogue);
        ActiveButtons.Add (button.gameObject);
    }

    public void UpdateButtons(List<Dialogue> responses)
    {
        BackgroundPanelAnimator.SetTrigger(BackgroundPanelAnimator.GetParameter(0).name);

        for (int i = 0; i < responses.Count; i++)
        {
            AddButton (responses[i], i);
        }
    }

    public void DeleteButtons()
    {
        BackgroundPanelAnimator.SetTrigger(BackgroundPanelAnimator.GetParameter(1).name);

        foreach (var item in ActiveButtons)
        {
            Destroy (item.gameObject);
        }
        
        ActiveButtons = new List<GameObject> ();
    }
}
