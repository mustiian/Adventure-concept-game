using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveActionController : MonoBehaviour
{
    public bool Loop;

    public ActivationCondition Condition;

    public List<GameAction> Actions;

    private bool activated = false;

    // Update is called once per frame
    void Update()
    {
        if ((!activated || Loop) && Condition.IsAccepted())
        {
            activated = true;
            Actions.ForEach(action =>
            {
                action.Activate();
            });
        }
    }
}
