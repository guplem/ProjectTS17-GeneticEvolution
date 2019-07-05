using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject defaultCell;
    [SerializeField] private int startCellCount;
    [SerializeField] private GameObject defaultFood;
    [SerializeField] private int startFoodCount;
    [SerializeField] private int foodSpawnPerSecond;

    [SerializeField] private Vector2 spawnZoneStart;
    [SerializeField] private Vector2 spawnZoneEnd;


    void Start()
    {
        StartSpawning();
        InvokeRepeating("SpawnFood", 1f, foodSpawnPerSecond);
    }

    private void StartSpawning()
    {
        for (int c = 0; c < startCellCount; c++)
        {
            Instantiate(defaultCell, GetRandomSpawnLocation(), Quaternion.identity);

        }

        for (int f = 0; f < startFoodCount; f++)
        {
            Instantiate(defaultFood, GetRandomSpawnLocation(), Quaternion.identity);
        }
    }

    private void SpawnFood()
    {
        Instantiate(defaultFood, GetRandomSpawnLocation(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnLocation()
    {
        return new Vector3(
            UnityEngine.Random.Range(spawnZoneStart.x, spawnZoneEnd.x),
            UnityEngine.Random.Range(spawnZoneStart.y, spawnZoneEnd.y), 0);
    }
}
