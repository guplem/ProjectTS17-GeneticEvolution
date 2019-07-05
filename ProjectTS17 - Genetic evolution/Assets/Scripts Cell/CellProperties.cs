using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
[System.Serializable]
public class CellProperties
{
    [Header("BASIC")]
    public float bodySize;
    public Cell.DigestiveSystem digestiveSystem;
    public float maxVelocity;

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