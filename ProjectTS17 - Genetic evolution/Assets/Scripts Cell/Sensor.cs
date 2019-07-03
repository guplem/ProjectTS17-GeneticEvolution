using UnityEngine;

[System.Serializable]
public class Sensor
{

    [SerializeField] private Vector2 lookingStartPoint;
    [Range(0f, 3.14159265359f * 2)]
    [SerializeField] private float lookingDirection;
    [Range(2, 50)]
    [SerializeField] private int lookingRaysQty;
    [Range(0f, 3.14159265359f * 2)]
    [SerializeField] private float lookingConeSize;
    [Range(2f, 50f)]
    [SerializeField] private float lookingDistance;
    [SerializeField] private LayerMask visualLayers;


}