using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class CellMovementController : MonoBehaviour
{
    [SerializeField] private GameObject Front, Front_Right, Right, Back_Right, Back, Back_Left, Left, Front_Left;
    private Rigidbody2D rb2d;
    private float degreeToObjective;
    private Cell cell;

    public void Setup(Rigidbody2D rb2d, Cell cell)
    {
        foreach (Flagellum flagellum in cell.cellProperties.flagellums)
        {
            switch (flagellum.position)
            {
                case Flagellum.Position.Front:
                    Front.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Front.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Front.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Front_Right:
                    Front_Right.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Front_Right.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Front_Right.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Right:
                    Right.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Right.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Right.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Back_Right:
                    Back_Right.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Back_Right.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Back_Right.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Back:
                    Back.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Back.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Back.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Back_Left:
                    Back_Left.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Back_Left.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Back_Left.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Left:
                    Left.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Left.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Left.SetActive(flagellum.impulseFrequency > 0);
                    break;
                case Flagellum.Position.Front_Left:
                    Front_Left.transform.localScale = new Vector3(Front.transform.localScale.x, flagellum.size, Front.transform.localScale.z);
                    Front_Left.GetComponent<FlagellumImpulseFrequency>().value = flagellum.impulseFrequency;
                    Front_Left.SetActive(flagellum.impulseFrequency > 0);
                    break;

            }
        }

        this.rb2d = rb2d;
        this.degreeToObjective = UnityEngine.Random.Range(0, 360);
        this.cell = cell;

        if (!Application.isPlaying)
            return;

        StartCoroutine(DoFlagellumPropulsion(Front));
        StartCoroutine(DoFlagellumPropulsion(Front_Right));
        StartCoroutine(DoFlagellumPropulsion(Right));
        StartCoroutine(DoFlagellumPropulsion(Back_Right));
        StartCoroutine(DoFlagellumPropulsion(Back));
        StartCoroutine(DoFlagellumPropulsion(Back_Left));
        StartCoroutine(DoFlagellumPropulsion(Left));
        StartCoroutine(DoFlagellumPropulsion(Front_Left));
    }

    public void MoveTowards(Vector2 objectivePosition)
    {
        Vector2 p2 = objectivePosition;
        Vector2 p1 = transform.position;
        float degree = (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI)-90;
        if (degree < 0) degree = 360 + degree;
        degreeToObjective = degree;
    }

    public void Avoid(Vector2 avoidingPosition)
    {
        Vector2 vToAvoidingPosition = avoidingPosition - (Vector2) transform.position;
        Vector2 p2 = vToAvoidingPosition * -1;
        Vector2 p1 = transform.position;
        float degree = (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI) - 90;
        if (degree < 0) degree = 360 + degree;
        degreeToObjective = degree;
    }

    private IEnumerator DoFlagellumPropulsion(GameObject flagellumGo)
    {
        if (!flagellumGo.activeSelf) yield return "success";

        while (true)
        {

            float flagellumDegree = flagellumGo.transform.rotation.eulerAngles.z;
            float degreeDif = Mathf.Abs(Mathf.DeltaAngle(flagellumDegree, degreeToObjective));
            float dedication = (180 - degreeDif)/180;

            Vector2 flagellumVector = flagellumGo.transform.position - flagellumGo.transform.GetChild(0).transform.position;

            Debug.DrawRay(flagellumGo.transform.position, flagellumVector * dedication * 10, Color.green, flagellumGo.GetComponent<FlagellumImpulseFrequency>().value);

            float force = dedication * flagellumGo.transform.localScale.y;

            rb2d.AddForce(flagellumVector*force, ForceMode2D.Impulse);
            rb2d.velocity = new Vector3(
                Mathf.Clamp(rb2d.velocity.x, -cell.cellProperties.maxVelocity, cell.cellProperties.maxVelocity),
                Mathf.Clamp(rb2d.velocity.y, -cell.cellProperties.maxVelocity, cell.cellProperties.maxVelocity), 0);

            //Debug.Log("Energy consumtion by flagel movement = " + force);
            cell.energy.Modify(-force);


            yield return new WaitForSeconds(flagellumGo.GetComponent<FlagellumImpulseFrequency>().value);
        }
    }

}