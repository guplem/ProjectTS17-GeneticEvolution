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

    public CellProperties(Vector3 bodySize, Cell.DigestiveSystem digestiveSystem, float maxVelocity, Flagellum[] flagellums, Sensor sensor, float startEnergy, float minRemainingEnergyAtReproduction, float mutationProbability, float mutationPercentage)
    {
        this.bodySize = bodySize;
        this.digestiveSystem = digestiveSystem;
        this.maxVelocity = maxVelocity;
        this.flagellums = flagellums;
        this.sensor = sensor;
        this.startEnergy = startEnergy;
        this.minRemainingEnergyAtReproduction = minRemainingEnergyAtReproduction;
        this.mutationProbability = mutationProbability;
        this.mutationPercentage = mutationPercentage;
    }

    public CellProperties Mutate()
    {
        if (DoMutate())
            bodySize = new Vector3(getMutationPositive(bodySize.x, mutationPercentage), getMutationPositive(bodySize.y, mutationPercentage), getMutationPositive(bodySize.z, mutationPercentage));
        if (DoMutate())
            maxVelocity = getMutationPositive(maxVelocity, mutationPercentage);
        if (DoMutate() && DoMutate() && DoMutate()) {
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

        foreach (Flagellum flagellum in flagellums)
        {
            if (DoMutate())
                flagellum.size = getMutationPositive(flagellum.size, mutationPercentage);

            if (DoMutate())
                if (flagellum.impulseFrequency>0)
                    flagellum.impulseFrequency = getMutationPositive(flagellum.impulseFrequency, mutationPercentage);

            if (DoMutate())
            {
                Array values = Enum.GetValues(typeof(Flagellum.Position));
                System.Random random = new System.Random();
                flagellum.position = (Flagellum.Position)values.GetValue(random.Next(values.Length));
            }
        }

        if (DoMutate() && DoMutate()) //Changes flagellums quantity or radical change in impulse freuqnecy
        {
            flagellums[UnityEngine.Random.Range(0, flagellums.Length - 1)].impulseFrequency = UnityEngine.Random.Range(-2f, 2f);
        }

        return this;
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
        float posExtreme = Mathf.Abs ( (originalValue / 100) * mutationPercentage ) ;
        float minExtreme = (originalValue - posExtreme) <= 0 ? originalValue-0.05f : posExtreme;
        float addition = UnityEngine.Random.Range(minExtreme, posExtreme);
        float returnVal = originalValue + addition;

        if (returnVal <= 0)
            Debug.LogWarning("Not proper mutation value = " + returnVal + ", originalValue = " + originalValue + ", posExtreme = " + posExtreme + ", minExtreme = " + minExtreme + ", addition = " + addition );

        return returnVal;//(returnVal <= 0) ? getMutationPositive(originalValue, mutationPercentage) : returnVal;
    }


    public CellProperties Clone()
    {
        /*
        Flagellum[] newFlagellums = new Flagellum[flagellums.Length];
        for (int f = 0; f < flagellums.Length; f++)
        {
            if (flagellums[f] != null)
                newFlagellums[f] = flagellums[f].Clone();
        }
        */

        return new CellProperties(bodySize, digestiveSystem, maxVelocity, flagellums, sensor.Clone(), startEnergy, minRemainingEnergyAtReproduction, mutationProbability, mutationPercentage);
    }
}