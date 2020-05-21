using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseToObjectCondition : ActivationCondition
{
    public GameObject Object;

    public float Distance;

    public override bool IsAccepted()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsCloseToObject (Object, Distance))
            return true;
        else
            return false;
    }

    private bool IsCloseToObject(GameObject target, float minDistance = 2)
    {
        Vector3 dir = target.transform.position - transform.position;
        float distance = dir.sqrMagnitude;

        if (distance < minDistance * minDistance)
        {
            return true;
        }

        return false;
    }
}
