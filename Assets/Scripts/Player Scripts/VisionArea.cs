using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisionArea : MonoBehaviour
{

    public List<Transform> visibleTargets = new List<Transform>();

    public float visionRadius;
    [Range(0f, 360f)]
    public float visionAngle;


    public LayerMask targetMask;
    public LayerMask obstacleMask;


    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }

    }


    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInVisionRadius = Physics.OverlapSphere(gameObject.transform.position, visionRadius, targetMask);

        for (int i = 0; i < targetsInVisionRadius.Length; i++)
        {

            Transform target = targetsInVisionRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < visionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask))
                {

                    visibleTargets.Add(target);

                }

            }

        }

    }




    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

    }







}