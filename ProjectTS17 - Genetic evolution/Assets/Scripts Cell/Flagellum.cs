﻿
using System;
using UnityEngine;

[System.Serializable]
public class Flagellum
{
    public enum Position
    {
        Front,
        Front_Right,
        Right,
        Back_Right,
        Back,
        Back_Left,
        Left,
        Front_Left,
    }
    public Position position;
    public float size;
    public float impulseFrequency;

    public void Setup(Position position, float size, float impulseFrequency)
    {
        this.position = position;
        this.size = size;
        this.impulseFrequency = impulseFrequency;
    }

    /*public Flagellum()
    {
        Array values = Enum.GetValues(typeof(Position));
        System.Random random = new System.Random();
        this.position = (Position)values.GetValue(random.Next(values.Length));

        System.Random r = new System.Random();
        this.size = 0.5f + (float)r.NextDouble();
        this.impulseFrequency = 0.1f + ((float)r.NextDouble()) * 1.8f;
    }*/

    public Flagellum(Position position, float size, float impulseFrequency)
    {
        this.position = position;
        this.size = size;
        this.impulseFrequency = impulseFrequency;
    }

    public override string ToString()
    {
        return "position = " + position + ", size = " + size + ", impulseFrequency =" + impulseFrequency;
    }

    public Flagellum Clone()
    {
        return new Flagellum(position, size, impulseFrequency);
    }
}