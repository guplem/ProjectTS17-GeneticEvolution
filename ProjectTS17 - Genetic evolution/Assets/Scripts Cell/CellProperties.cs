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

    public static CellProperties Mutate(CellProperties cp)
    {
        if (cp.DoMutate())
            cp.bodySize = new Vector3(getMutationPositive("bodySize.x", cp.bodySize.x, cp.mutationPercentage), getMutationPositive("bodySize.y", cp.bodySize.y, cp.mutationPercentage), getMutationPositive("bodySize.z", cp.bodySize.z, cp.mutationPercentage));
        if (cp.DoMutate())
            cp.maxVelocity = getMutationPositive("maxVelocity", cp.maxVelocity, cp.mutationPercentage);
        if (cp.DoMutate() && cp.DoMutate() && cp.DoMutate()) {
            Array values = Enum.GetValues(typeof(Cell.DigestiveSystem));
            System.Random random = new System.Random();
            cp.digestiveSystem = (Cell.DigestiveSystem)values.GetValue(random.Next(values.Length));
        }
        if (cp.DoMutate())
            cp.startEnergy = getMutationPositive("startEnergy", cp.startEnergy, cp.mutationPercentage);
        if (cp.DoMutate())
            cp.minRemainingEnergyAtReproduction = getMutationPositive("minRemainingEnergyAtReproduction", cp.minRemainingEnergyAtReproduction, cp.mutationPercentage);


        if (cp.DoMutate())
            cp.sensor.updateFrequency = getMutationPositive("sensor.updateFrequency", cp.sensor.updateFrequency, cp.mutationPercentage);
        if (cp.DoMutate())
            cp.sensor.lookingRaysQty = getMutationInt("sensor.lookingRaysQty", cp.sensor.lookingRaysQty, cp.mutationPercentage);
        if (cp.DoMutate())
            cp.sensor.lookingConeSize = getMutationPositive("sensor.lookingConeSize", cp.sensor.lookingConeSize, cp.mutationPercentage);
        if (cp.DoMutate())
            cp.sensor.lookingDistance = getMutationPositive("sensor.lookingDistance", cp.sensor.lookingDistance, cp.mutationPercentage);

        foreach (Flagellum flagellum in cp.flagellums)
        {
            if (cp.DoMutate())
                flagellum.size = getMutationPositive("flagellum.size", flagellum.size, cp.mutationPercentage);

            if (cp.DoMutate())
                if (flagellum.impulseFrequency>0)
                    flagellum.impulseFrequency = getMutationPositive("flagellum.impulseFrequency", flagellum.impulseFrequency, cp.mutationPercentage);

            if (cp.DoMutate())
            {
                Array values = Enum.GetValues(typeof(Flagellum.Position));
                System.Random random = new System.Random();
                flagellum.position = (Flagellum.Position)values.GetValue(random.Next(values.Length));
            }
        }

        if (cp.DoMutate() && cp.DoMutate()) //Changes flagellums quantity or radical change in impulse freuqnecy
        {
            int mutatedFlagellum = UnityEngine.Random.Range(0, cp.flagellums.Length - 1);
            cp.flagellums[mutatedFlagellum].impulseFrequency = UnityEngine.Random.Range(-2f, 2f);
            Debug.Log(" Radical change in impulse frequency for: " + cp.flagellums[mutatedFlagellum].ToString());
        }

        return cp;
    }

    private bool DoMutate()
    {
        return mutationProbability < UnityEngine.Random.Range(0f, 100f);
    }

    private static int getMutationInt(string mutationName, int originalValue, float mutationPercentage)
    {
        float mutationVal = getMutationPositive(mutationName, originalValue, mutationPercentage);

        if (UnityEngine.Random.value > 0.5f)
            return Mathf.FloorToInt(mutationVal);

        return Mathf.CeilToInt(mutationVal);
    }


    private static float getMutationPositive(string mutationName, float originalValue, float mutationPercentage)
    {
        float posExtreme = Mathf.Abs ( (originalValue / 100) * mutationPercentage ) ;
        float minExtreme = (originalValue - posExtreme) <= 0 ? originalValue-0.05f : posExtreme;
        float addition = UnityEngine.Random.Range(minExtreme, posExtreme);
        float returnVal = originalValue + addition;

        if (returnVal <= 0)
            Debug.LogWarning("Mutating " + mutationName + ". Not proper mutation value = " + returnVal + ", originalValue = " + originalValue + ", posExtreme = " + posExtreme + ", minExtreme = " + minExtreme + ", addition = " + addition );

        return returnVal;//(returnVal <= 0) ? getMutationPositive(originalValue, mutationPercentage) : returnVal;
    }


    public CellProperties Clone()
    {
        
        Flagellum[] newFlagellums = new Flagellum[flagellums.Length];
        for (int f = 0; f < flagellums.Length; f++)
        {
            if (flagellums[f] != null)
                newFlagellums[f] = flagellums[f].Clone();
        }
        

        return new CellProperties(bodySize, digestiveSystem, maxVelocity, newFlagellums, sensor.Clone(), startEnergy, minRemainingEnergyAtReproduction, mutationProbability, mutationPercentage);
    }
}