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
    [HideInInspector] public float bodySize { get { return body.transform.localScale.x; } private set { body.transform.localScale = new Vector3(value, value, value); } }
    [SerializeField] private CellMovementController movementController;
    [SerializeField] public CellProperties cellProperties;

    public void Start()
    {
        Setup(cellProperties);

        InvokeRepeating("Sense", UnityEngine.Random.Range(0, cellProperties.sensor.updateFrequency), cellProperties.sensor.updateFrequency);
    }


    public void Setup(CellProperties cellProperties)
    {
        this.cellProperties = cellProperties;
        this.bodySize = cellProperties.bodySize;
        movementController.setup(GetComponent<Rigidbody2D>(), cellProperties.flagellums, this);
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
}