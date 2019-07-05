﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class Cell : MonoBehaviour
{
    public enum DigestiveSystem
    {
        carinvorous,
        hervivouros,
        homnivorous,
    }

    [SerializeField] private GameObject body;
    [HideInInspector] public Vector3 bodySize { get { return body.transform.localScale; } private set { body.transform.localScale = value; } }
    [SerializeField] private CellMovementController movementController;
    [SerializeField] public CellProperties cellProperties;
    [HideInInspector] public Energy energy;

    public void Start()
    {
        Setup(cellProperties);

        InvokeRepeating("Sense", UnityEngine.Random.Range(0, cellProperties.sensor.updateFrequency), cellProperties.sensor.updateFrequency);

        InvokeRepeating("EnergyConsumptionByBody", UnityEngine.Random.Range(0, 1), 1);
        InvokeRepeating("Reproduction", UnityEngine.Random.Range(0, 5), 5);
    }




    public void Setup(CellProperties cellProperties)
    {
        this.cellProperties = cellProperties;
        this.bodySize = cellProperties.bodySize;
        movementController.setup(GetComponent<Rigidbody2D>(), cellProperties.flagellums, this);
        energy = GetComponent<Energy>();
    }

    public void EnergyConsumptionByBody()
    {
        float consumption = body.transform.localScale.x + body.transform.localScale.y + body.transform.localScale.z;
        energy.Modify(-consumption);
    }


    protected void OnDrawGizmosSelected()
    {
        Vector2 startSearchPos = transform.position;
        //Gizmos.DrawSphere(pos, 0.2f);

        Vector2[] pointsToLookAt = cellProperties.sensor.GetPointsToLookAt(startSearchPos);
        for (int i = 0; i < pointsToLookAt.Length; i++)
            Gizmos.DrawLine(startSearchPos, pointsToLookAt[i]);

    }

    private void Update()
    {
        //UpdateMovement();
    }

    private void Sense()
    {
        float energyConsumption = cellProperties.sensor.lookingRaysQty * cellProperties.sensor.lookingDistance / 1000;
        energy.Modify(-energyConsumption);

        Vector2 position = Vector2.zero;

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

    }

    public void Reproduction()
    {
        if (energy.current < cellProperties.energyToChild + cellProperties.minRemainingEnergyAtReproduction)
            return;

        GiveBirth();

        energy.Modify(-cellProperties.energyToChild);
    }

    private void GiveBirth()
    {
        GameObject child = Instantiate(this.gameObject, transform.position, this.transform.rotation);

        child.GetComponent<Energy>().SetEnergy(cellProperties.energyToChild);

        child.GetComponent<Cell>().cellProperties.Mutate();
    }
}