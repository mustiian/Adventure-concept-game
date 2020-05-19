using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveActionController : MonoBehaviour
{
    public bool Loop;

    public ActivationCondition Condition;

    public GameObject ActionsObject;

    private List<GameAction> actions = new List<GameAction>();

    private bool activated = false;

    private void Awake()
    {
        if (!Condition)
            Condition = GetComponent<ActivationCondition>();

        if (ActionsObject)
        {
            foreach (var action in ActionsObject.GetComponentsInChildren<GameAction>())
            {
                actions.Add(action);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((!activated || Loop) && Condition.IsAccepted())
        {
            activated = true;
            actions.ForEach(action =>
            {
                action.Activate();
            });
        }
    }
}
