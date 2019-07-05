using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public float current { get; private set; }
    [SerializeField] public float startEnergy;

    private void Start()
    {
        Cell cell = GetComponent<Cell>();
        if (cell != null)
            startEnergy = cell.cellProperties.energyToChild;

        if (startEnergy > 0)
            SetEnergy(startEnergy);
    }

    public void SetEnergy(float val)
    {
        current = val;

        if (current <= 0)
            Destroy(gameObject);
    }

    public void Modify(float val)
    {
        SetEnergy(current + val);
    }
    
}
