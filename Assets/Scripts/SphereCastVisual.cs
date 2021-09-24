using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SphereCastVisual
{
    [SerializeField] bool debugSpherecast = true;
    [SerializeField] [Range(1, 10)] int debugIterations = 1;

    public void VisualizeCast(LayerMask groundLayer, Ray ray, float range = 3f, float castRadius = 5f)
    {
        if (!debugSpherecast)
            return;

        bool detectObstacle = Physics.SphereCast(ray.origin, castRadius, ray.direction, out RaycastHit hit, range, groundLayer);
        Gizmos.color = detectObstacle ? Color.red : Color.green;

        if (hit.transform)
        {
            float dist = Vector3.Distance(ray.origin, hit.point);
            Gizmos.DrawRay(ray.origin, ray.direction * dist);

            for (int i = debugIterations; i < dist; i += debugIterations)
                Gizmos.DrawWireSphere(ray.GetPoint(i), castRadius);
        }
        else
        {
            Gizmos.DrawRay(ray.origin, ray.direction * range);
            for (int i = debugIterations; i < range; i += debugIterations)
                Gizmos.DrawWireSphere(ray.GetPoint(i), castRadius);
        }
    }
}
