using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(fileName = "New Cell Properties", menuName = "Models/Properties/Cell")]
public class CellProperties
{
    [Header("BASIC")]
    public float bodySize;
    public Cell.DigestiveSystem digestiveSystem;

    [Header("FLAGELLUMS")]
    [Space(20)]
    public Flagellum[] flagellums; //Up to 8 --> [1, 8]

    [Header("SENSOR")]
    [Space(20)]
    public Sensor sensor;

    public CellProperties()
    {
        this.bodySize = 1f;
        sensor = new Sensor();
    }

}