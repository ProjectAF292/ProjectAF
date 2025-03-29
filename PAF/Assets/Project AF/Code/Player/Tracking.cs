using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    public float trackingRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform closeTargets;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, trackingRange, Vector2.zero, 0, targetLayer);
        closeTargets = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;

    }
}
