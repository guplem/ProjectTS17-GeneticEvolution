using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public enum DigestiveSystem
    {
        carinvorous,
        hervivouros,
        homnivorous,
    }

    [SerializeField] private GameObject body;
    public float bodySize { get { return body.transform.localScale.x; } private set { body.transform.localScale = new Vector3(value, value, value); } }
    [SerializeField] private CellMovementController movementController;
    [SerializeField] private CellProperties cellProperties;



    public void Setup(CellProperties cellProperties)
    {
        this.cellProperties = cellProperties;
        this.bodySize = cellProperties.bodySize;
        movementController.setup(GetComponent<Rigidbody>(), cellProperties.flagellums);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
