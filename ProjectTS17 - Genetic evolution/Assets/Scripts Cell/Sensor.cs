using UnityEngine;

[System.Serializable]
public class Sensor
{

    //[Range(0f, 3.14159265359f * 2)]
    //[SerializeField] private float lookingDirection;
    [Range(2, 50)]
    [SerializeField] private int lookingRaysQty;
    [Range(0f, 3.14159265359f * 2)]
    [SerializeField] private float lookingConeSize;
    [Range(2f, 50f)]
    [SerializeField] private float lookingDistance;
    [SerializeField] private LayerMask visualLayers;


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
}