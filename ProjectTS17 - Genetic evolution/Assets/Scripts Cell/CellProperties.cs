using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
[System.Serializable]
public class CellProperties
{
    [Header("BASIC")]
    public Vector3 bodySize;
    public Cell.DigestiveSystem digestiveSystem;
    public float maxVelocity;

    [Header("FLAGELLUMS")]
    [Space(20)]
    public Flagellum[] flagellums; //Up to 8 --> [1, 8]

    [Header("SENSOR")]
    [Space(20)]
    public Sensor sensor;

    [Header("REPRODUCTION")]
    [Space(20)]
    public float startEnergy;
    public float minRemainingEnergyAtReproduction;


    [Header("MUTATION")]
    [Space(20)]
    public float mutationProbability;
    public float mutationPercentage;


    public CellProperties()
    {
        this.bodySize = Vector3.one;
        sensor = new Sensor();
    }

    public void Mutate()
    {
        if (DoMutate())
            bodySize = new Vector3(getMutation(bodySize.x, mutationPercentage), getMutation(bodySize.y, mutationPercentage), getMutation(bodySize.z, mutationPercentage));
        if (DoMutate())
            maxVelocity = getMutation(maxVelocity, mutationPercentage);
        if (DoMutate() && DoMutate()) {//Double
            Array values = Enum.GetValues(typeof(Cell.DigestiveSystem));
            System.Random random = new System.Random();
            digestiveSystem = (Cell.DigestiveSystem)values.GetValue(random.Next(values.Length));
        }
        if (DoMutate())
            startEnergy = getMutation(startEnergy, mutationPercentage);
        if (DoMutate())
            minRemainingEnergyAtReproduction = getMutation(minRemainingEnergyAtReproduction, mutationPercentage);


        if (DoMutate())
            sensor.updateFrequency = getMutation(sensor.updateFrequency, mutationPercentage);
        if (DoMutate())
            sensor.lookingRaysQty = getMutationInt(sensor.lookingRaysQty, mutationPercentage);
        if (DoMutate())
            sensor.lookingConeSize = getMutation(sensor.lookingConeSize, mutationPercentage);
        if (DoMutate())
            sensor.lookingDistance = getMutation(sensor.lookingDistance, mutationPercentage);

        if (DoMutate()) //Changes flagellums quantity
        {
            int newQttyFlag = getMutationInt(flagellums.Length, mutationPercentage);
            Flagellum[] newFlagellums = new Flagellum[newQttyFlag];
            for (int f = 0; f < newFlagellums.Length; f++)
            {
                try
                {
                    newFlagellums[f] = flagellums[f];
                }
                catch (System.IndexOutOfRangeException)
                {
                    newFlagellums[f] = new Flagellum();
                }
            }
            flagellums = newFlagellums;
        }

        foreach (Flagellum flagellum in flagellums)
        {
            if (DoMutate())
            {
                Array values = Enum.GetValues(typeof(Flagellum.Position));
                System.Random random = new System.Random();
                flagellum.position = (Flagellum.Position)values.GetValue(random.Next(values.Length));
            }

            if (DoMutate())
                flagellum.size = getMutation(flagellum.size, mutationPercentage);

            if (DoMutate())
                flagellum.impulseFrequency = getMutation(flagellum.impulseFrequency, mutationPercentage);
        }

    }

    private bool DoMutate()
    {
        return mutationProbability < UnityEngine.Random.Range(0f, 100f);
    }

    private int getMutationInt(int originalValue, float mutationPercentage)
    {
        float mutationVal = getMutation(originalValue, mutationPercentage);

        if (UnityEngine.Random.value > 0.5f)
            return Mathf.FloorToInt(mutationVal);

        return Mathf.CeilToInt(mutationVal);
    }


    private float getMutation(float originalValue, float mutationPercentage)
    {
        float extremes = Mathf.Abs ( (originalValue / 100) * mutationPercentage ) ;
        float addition = UnityEngine.Random.Range(-extremes, extremes);
        return originalValue + addition;
    }
}