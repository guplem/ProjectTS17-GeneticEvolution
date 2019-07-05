using System;
using UnityEngine;

#pragma warning disable 0649
[System.Serializable]
public class Sensor
{

    public float updateFrequency;
    [Range(2, 50)]
    [SerializeField] public int lookingRaysQty;
    [Range(0f, 3.14159265359f * 2)]
    [SerializeField] public float lookingConeSize;
    [Range(2f, 50f)]
    [SerializeField] public float lookingDistance;
    [SerializeField] private LayerMask foodLayer;
    [SerializeField] private LayerMask cellLayer;


    public Vector2[] GetPointsToLookAt(Vector2 startSearchPos)
    {
        Vector2[] returnVectors = new Vector2[lookingRaysQty];

        for (int i = 0; i < lookingRaysQty; i++)
        {
            float rayAngle = 3.14159265359f * 0.5f;
            rayAngle += lookingConeSize / 2;
            rayAngle -= i * (lookingConeSize / (lookingRaysQty-1));

            returnVectors[i] = startSearchPos + new Vector2(Mathf.Cos(rayAngle), Mathf.Sin(rayAngle)) * lookingDistance;
        }

        return returnVectors;
    }

    public Vector2 detectsFood(GameObject gameObject)
    {
        return GetDetectionPointWith(gameObject, foodLayer);
    }

    public Vector2 detectsDanger(GameObject gameObject)
    {
        //TODO: Check body size, ...
        return GetDetectionPointWith(gameObject, cellLayer);
    }

    private Vector2 GetDetectionPointWith(GameObject gameObject, LayerMask layer)
    {
        RaycastHit2D[] hit;
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layer);

        Vector2 pos = gameObject.transform.position;
        Vector2[] pointsToLookAt = GetPointsToLookAt(pos);

        for (var i = 0; i < pointsToLookAt.Length; i++)
        {
            hit = new RaycastHit2D[1];

            Vector2 direction = ((pointsToLookAt[i] - pos).normalized);
            Physics2D.Raycast(pos, direction, filter, hit, lookingDistance);
            if (hit[0].collider != null)
            {
                //Debug.Log(hit[0].point);
                return hit[0].point;
            }

        }
        //Debug.Log("NOT FOUND");
        return Vector2.zero;
    }


}