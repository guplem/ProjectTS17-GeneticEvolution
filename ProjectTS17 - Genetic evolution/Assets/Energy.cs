using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public float current { get; private set; }
    [SerializeField] public float startEnergyIfNotCell;
    private bool startedEnergyComponent;

    private void Start()
    {
        Cell cell = GetComponent<Cell>();
        if (cell != null)
            startEnergyIfNotCell = cell.cellProperties.startEnergy;

        if (startEnergyIfNotCell > 0)
            SetEnergy(startEnergyIfNotCell);

        startedEnergyComponent = true;
    }

    public void SetEnergy(float val)
    {
        current = val;
        if (current <= 0)
            if (startedEnergyComponent)
                Destroy(gameObject);
    }

    public void Modify(float val)
    {
        SetEnergy(current + val);
    }
    
}
