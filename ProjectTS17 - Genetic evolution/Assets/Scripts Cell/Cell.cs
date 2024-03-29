﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
#pragma warning disable CS0649
public class Cell : MonoBehaviour
{
    public enum DigestiveSystem
    {
        hervivouros,
        carinvorous,
        homnivorous,
    }

    [SerializeField] private GameObject body;
    [HideInInspector] public Vector3 bodySize { get { return body.transform.localScale; } private set { body.transform.localScale = value; } }
    [SerializeField] private CellMovementController movementController;
    [SerializeField] public CellProperties cellProperties;
    [HideInInspector] public Energy energy;
    private Vector3 defaultPos;
    [Space (25)]
    [SerializeField] private Image EnergyIndicator;

    public void Setup(CellProperties cp)
    {
        cellProperties = cp;

        ApplyCellProperties(cp);
        energy = GetComponent<Energy>();

        defaultPos = GameManager.Instance.GetRandomLocationInsideSpawn();

        InvokeRepeating("Sense", UnityEngine.Random.Range(0, cp.sensor.updateFrequency), cp.sensor.updateFrequency);
        InvokeRepeating("EnergyConsumptionByBody", UnityEngine.Random.Range(0, 1), 1);
        InvokeRepeating("Reproduction", UnityEngine.Random.Range(0, 5), 5);


        switch (cellProperties.digestiveSystem)
        {
            case DigestiveSystem.hervivouros:
                body.GetComponent<Renderer>().material.color = Color.green;
                break;
            case DigestiveSystem.carinvorous:
                body.GetComponent<Renderer>().material.color = Color.red;
                break;
            case DigestiveSystem.homnivorous:
                body.GetComponent<Renderer>().material.color = Color.yellow;
                break;
        }
        
    }

    private void Update()
    {
        EnergyIndicator.fillAmount = energy.current / (cellProperties.startEnergy + cellProperties.minRemainingEnergyAtReproduction);
    }



    public void ApplyCellProperties(CellProperties cp)
    {
        this.bodySize = cp.bodySize;

        movementController.Setup(GetComponent<Rigidbody2D>(), this);

        cp.sensor.setup(this);
    }

    public void EnergyConsumptionByBody()
    {
        float consumption = body.transform.localScale.x + body.transform.localScale.y + body.transform.localScale.z;
        //Debug.Log("Energy consumtion by body size = " + consumption);
        energy.Modify(-(Mathf.Pow(consumption,2)));
    }


    protected void OnDrawGizmosSelected()
    {
        Vector2 startSearchPos = transform.position;
        //Gizmos.DrawSphere(pos, 0.2f);

        Vector2[] pointsToLookAt = cellProperties.sensor.GetPointsToLookAt(startSearchPos);
        for (int i = 0; i < pointsToLookAt.Length; i++)
            Gizmos.DrawLine(startSearchPos, pointsToLookAt[i]);

    }

    private void Sense()
    {
        float energyConsumption = cellProperties.sensor.lookingRaysQty * cellProperties.sensor.lookingDistance / 100;
        //Debug.Log("Energy consumtion by all sensors in cell = " + energyConsumption);
        energy.Modify(-energyConsumption);

        Vector2 position;

        position = cellProperties.sensor.detectsDanger(this.gameObject);
        if (position != Vector2.zero)
        {
            movementController.Avoid(position);
            return;
        }


        position = cellProperties.sensor.detectsFood(this.gameObject);
        if (position != Vector2.zero)
        {
            movementController.MoveTowards(position);
            return;
        }

        movementController.MoveTowards(defaultPos);
        return;
    }

    public void Reproduction()
    {
        if (energy.current < cellProperties.startEnergy + cellProperties.minRemainingEnergyAtReproduction)
            return;

        GameManager.GiveBirth(this);

        //Debug.Log("Energy consumtion by reproduction = " + cellProperties.startEnergy);
        energy.Modify(-cellProperties.startEnergy);
    }

    /*private void GiveBirth()
    {
        Debug.Log("GiveBirth. " + energy.current + " will be splited in " + cellProperties.startEnergy + " to the child and " + cellProperties.minRemainingEnergyAtReproduction + " to the father.");

        GameObject child = Instantiate(GameManager.Instance.defaultCell, transform.position, this.transform.rotation);


        Cell newCell = child.GetComponent<Cell>();

        newCell.cellProperties = cellProperties.Clone().Mutate();
        newCell.Setup();
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cell")
        {
            if (cellProperties.digestiveSystem == DigestiveSystem.hervivouros)
                return;

            Cell colCell = collision.gameObject.GetComponent<Cell>();

            if (!isSizeEdable(bodySize, colCell.bodySize))
                return;

            energy.Modify(colCell.energy.current);
            Destroy(colCell.gameObject);

        }
        else if (collision.gameObject.tag == "Food")
        {
            if (cellProperties.digestiveSystem == DigestiveSystem.carinvorous)
                return;

            if (!isSizeEdable(bodySize, collision.transform.localScale))
                return;

            Energy foodEnergy = collision.gameObject.GetComponent<Energy>();
            energy.Modify(foodEnergy.current);
            Destroy(foodEnergy.gameObject);
        }
    }

    public static bool isSizeEdable(Vector3 mySize, Vector3 otherSize)
    {
        float other = otherSize.x + otherSize.y + otherSize.z;
        float my = mySize.x + mySize.y + mySize.z;

        return my > other;
    }
}