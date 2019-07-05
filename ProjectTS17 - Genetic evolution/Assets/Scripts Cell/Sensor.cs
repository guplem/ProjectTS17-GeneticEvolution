using System;
using UnityEngine;

#pragma warning disable 0649
[System.Serializable]
public class Sensor
{

    public float updateFrequency;
    [Range(2, 50)]
    [SerializeField] public int lookingRaysQty;
    [Range(0f, 3.14159265359f * 2)]
    [SerializeField] public float lookingConeSize;
    [Range(2f, 50f)]
    [SerializeField] public float lookingDistance;
    [SerializeField] public LayerMask foodLayer;
    [SerializeField] public LayerMask cellLayer;
    private Cell cell;


    public Vector2[] GetPointsToLookAt(Vector2 startSearchPos)
    {
        Vector2[] returnVectors = new Vector2[lookingRaysQty];

        for (int i = 0; i < lookingRaysQty; i++)
        {
            float rayAngle = 3.14159265359f * 0.5f;
            rayAngle += lookingConeSize / 2;
            rayAngle -= i * (lookingConeSize / (lookingRaysQty-1));

            returnVectors[i] = startSearchPos + new Vector2(Mathf.Cos(rayAngle), Mathf.Sin(rayAngle)) * lookingDistance;
        }

        return returnVectors;
    }

    public Vector2 detectsFood(GameObject gameObject)
    {
        GameObject objectSeen = null;

        if (cell.cellProperties.digestiveSystem == Cell.DigestiveSystem.carinvorous || cell.cellProperties.digestiveSystem == Cell.DigestiveSystem.homnivorous)
            objectSeen = ObjectSeen(gameObject, cellLayer);

        if (objectSeen == null)
            if (cell.cellProperties.digestiveSystem == Cell.DigestiveSystem.hervivouros || cell.cellProperties.digestiveSystem == Cell.DigestiveSystem.homnivorous)
                objectSeen = ObjectSeen(gameObject, foodLayer);

        if (objectSeen == null)
            return Vector2.zero;

        Cell cellSeen = objectSeen.GetComponent<Cell>();

        if (cellSeen != null)
        {
            if (!Cell.isSizeEdable(cell.bodySize, cellSeen.bodySize))
                return Vector2.zero;
        } else
        {
            if (!Cell.isSizeEdable(cell.bodySize, objectSeen.transform.position)) //NO me lo puedo comer
                return Vector2.zero;
        }

        return objectSeen.transform.position;
    }

    public Vector2 detectsDanger(GameObject gameObject)
    {
        GameObject objectSeen = ObjectSeen(gameObject, cellLayer);

        if (objectSeen == null)
            return Vector2.zero;

        Cell cellSeen = objectSeen.GetComponent<Cell>();

        if (cellSeen.cellProperties.digestiveSystem == Cell.DigestiveSystem.hervivouros)
            return Vector2.zero;

        if (Cell.isSizeEdable(cell.bodySize, cellSeen.bodySize)) //Me la puedo comer yo
            return Vector2.zero;

        return objectSeen.transform.position;
    }

    internal void setup(Cell cell)
    {
        this.cell = cell;
    }

    private GameObject ObjectSeen(GameObject gameObject, LayerMask layer)
    {
        RaycastHit2D[] hit;
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layer);

        Vector2 pos = gameObject.transform.position;
        Vector2[] pointsToLookAt = GetPointsToLookAt(pos);

        for (var i = 0; i < pointsToLookAt.Length; i++)
        {
            hit = new RaycastHit2D[1];

            Vector2 direction = ((pointsToLookAt[i] - pos).normalized);
            Physics2D.Raycast(pos, direction, filter, hit, lookingDistance);
            if (hit[0].collider != null)
            {
                //Debug.Log(hit[0].point);
                return hit[0].collider.gameObject;
            }

        }
        //Debug.Log("NOT FOUND");
        return null;
    }


}