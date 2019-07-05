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

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
            Debug.LogWarning("Multiple game managers have been created");

        Instance = this;
    }
    void Start()
    {
        StartSpawning();
        InvokeRepeating("SpawnFood", 1f, foodSpawnPerSecond);
    }

    private void StartSpawning()
    {
        for (int c = 0; c < startCellCount; c++)
        {
            GameObject child = Instantiate(defaultCell, GetRandomLocationInsideSpawn(), Quaternion.identity);
            child.GetComponent<Cell>().Setup();
        }

        for (int f = 0; f < startFoodCount; f++)
        {
            Instantiate(defaultFood, GetRandomLocationInsideSpawn(), Quaternion.identity);
        }
    }

    private void SpawnFood()
    {
        Instantiate(defaultFood, GetRandomLocationInsideSpawn(), Quaternion.identity);
    }

    public Vector3 GetRandomLocationInsideSpawn()
    {
        return new Vector3(
            UnityEngine.Random.Range(spawnZoneStart.x, spawnZoneEnd.x),
            UnityEngine.Random.Range(spawnZoneStart.y, spawnZoneEnd.y), 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 0.2f;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale -= 0.2f;

            if (Time.timeScale < 0)
                Time.timeScale = 0;
        }
    }
}
