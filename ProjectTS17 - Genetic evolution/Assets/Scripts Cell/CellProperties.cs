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
            bodySize = new Vector3(getMutationPositive(bodySize.x, mutationPercentage), getMutationPositive(bodySize.y, mutationPercentage), getMutationPositive(bodySize.z, mutationPercentage));
        if (DoMutate())
            maxVelocity = getMutationPositive(maxVelocity, mutationPercentage);
        if (DoMutate() && DoMutate()) {//Double
            Array values = Enum.GetValues(typeof(Cell.DigestiveSystem));
            System.Random random = new System.Random();
            digestiveSystem = (Cell.DigestiveSystem)values.GetValue(random.Next(values.Length));
        }
        if (DoMutate())
            startEnergy = getMutationPositive(startEnergy, mutationPercentage);
        if (DoMutate())
            minRemainingEnergyAtReproduction = getMutationPositive(minRemainingEnergyAtReproduction, mutationPercentage);


        if (DoMutate())
            sensor.updateFrequency = getMutationPositive(sensor.updateFrequency, mutationPercentage);
        if (DoMutate())
            sensor.lookingRaysQty = getMutationInt(sensor.lookingRaysQty, mutationPercentage);
        if (DoMutate())
            sensor.lookingConeSize = getMutationPositive(sensor.lookingConeSize, mutationPercentage);
        if (DoMutate())
            sensor.lookingDistance = getMutationPositive(sensor.lookingDistance, mutationPercentage);

        if (DoMutate() && DoMutate()) //Changes flagellums quantity or radical change in impulse freuqnecy
        {
            flagellums[UnityEngine.Random.Range(0, flagellums.Length - 1)].impulseFrequency = UnityEngine.Random.Range(-2f, 2f);
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
                flagellum.size = getMutationPositive(flagellum.size, mutationPercentage);

            if (DoMutate())
                flagellum.impulseFrequency = getMutationPositive(flagellum.impulseFrequency, mutationPercentage);
        }

    }

    private bool DoMutate()
    {
        return mutationProbability < UnityEngine.Random.Range(0f, 100f);
    }

    private int getMutationInt(int originalValue, float mutationPercentage)
    {
        float mutationVal = getMutationPositive(originalValue, mutationPercentage);

        if (UnityEngine.Random.value > 0.5f)
            return Mathf.FloorToInt(mutationVal);

        return Mathf.CeilToInt(mutationVal);
    }


    private float getMutationPositive(float originalValue, float mutationPercentage)
    {
        float extremes = Mathf.Abs ( (originalValue / 100) * mutationPercentage ) ;
        float addition = UnityEngine.Random.Range(-extremes, extremes);
        float returnVal = originalValue + addition;

        return (returnVal <= 0) ? getMutationPositive(originalValue, mutationPercentage) : returnVal;
    }
}