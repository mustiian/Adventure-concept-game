using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;
using UnityEngine.AI;

public class SheepWalkStage : Stage
{
    public float RadiusToWalk;
    public NavMeshAgent meshAgent;
    public Animator animator;

    private Vector3 target = Vector3.zero;

    public override bool ConditionToFinish()
    {
        return IsCloseToObject(target, 1f);
    }

    public override void InitStage()
    {
        target = Vector3.zero;

        if (RandomNavmeshLocation(transform.position, RadiusToWalk, out target))
            meshAgent.SetDestination(target);
        else
            ExitStage();

        var lookDirection = (target - meshAgent.transform.position).normalized;
        animator.SetTrigger("Walk");

        if (lookDirection.x > 0)
            animator.GetComponent<SpriteRenderer>().flipX = true;
        else
            animator.GetComponent<SpriteRenderer>().flipX = false;
    }

    public override void UpdateStage()
    {
        if (ConditionToFinish())
            ExitStage();
    }

    private bool RandomNavmeshLocation(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (target != Vector3.zero)
            Gizmos.DrawWireSphere(target, 0.5f);
    }
    private bool IsCloseToObject(Vector3 target, float minDistance = 2)
    {
        Vector3 dir = target - meshAgent.transform.position;
        float distance = dir.sqrMagnitude;

        if (distance < minDistance * minDistance)
        {
            return true;
        }

        return false;
    }
}
