using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class CellMovementController : MonoBehaviour
{
    [SerializeField] private GameObject Front, Front_Right, Right, Back_Right, Back, Back_Left, Left, Front_Left;
    private Flagellum fFront, fFront_Right, fRight, fBack_Right, fBack, fBack_Left, fLeft, fFront_Left;
    private Flagellum[] flagellums;
    private Rigidbody2D rb2d;
    private float degreeToObjective;
    private Cell cell;

    public void setup(Rigidbody2D rb2d, Flagellum[] flagellums, Cell cell)
    {
        Front.SetActive(false);
        Front_Right.SetActive(false);
        Right.SetActive(false);
        Back_Right.SetActive(false);
        Back.SetActive(false);
        Back_Left.SetActive(false);
        Left.SetActive(false);
        Front_Left.SetActive(false);

        foreach (Flagellum flagellum in flagellums)
        {
            switch (flagellum.position)
            {
                case Flagellum.Position.Front:
                    Front.SetActive(true);
                    fFront = flagellum;
                    break;
                case Flagellum.Position.Front_Right:
                    Front_Right.SetActive(true);
                    fFront_Right = flagellum;
                    break;
                case Flagellum.Position.Right:
                    Right.SetActive(true);
                    fRight = flagellum;
                    break;
                case Flagellum.Position.Back_Right:
                    Back_Right.SetActive(true);
                    fBack_Right = flagellum;
                    break;
                case Flagellum.Position.Back:
                    Back.SetActive(true);
                    fBack = flagellum;
                    break;
                case Flagellum.Position.Back_Left:
                    Back_Left.SetActive(true);
                    fBack_Left = flagellum;
                    break;
                case Flagellum.Position.Left:
                    Left.SetActive(true);
                    fLeft = flagellum;
                    break;
                case Flagellum.Position.Front_Left:
                    Front_Left.SetActive(true);
                    fFront_Left = flagellum;
                    break;
                default:
                    Debug.LogError("Unnexpected value");
                    break;
            }
        }

        this.flagellums = flagellums;
        this.rb2d = rb2d;
        this.degreeToObjective = UnityEngine.Random.Range(0, 360);
        this.cell = cell;

        if (!Application.isPlaying)
            return;

        StartCoroutine(DoFlagellumPropulsion(Front, fFront));
        StartCoroutine(DoFlagellumPropulsion(Front_Right, fFront_Right));
        StartCoroutine(DoFlagellumPropulsion(Right, fRight));
        StartCoroutine(DoFlagellumPropulsion(Back_Right, fBack_Right));
        StartCoroutine(DoFlagellumPropulsion(Back, fBack));
        StartCoroutine(DoFlagellumPropulsion(Back_Left, fBack_Left));
        StartCoroutine(DoFlagellumPropulsion(Left, fLeft));
        StartCoroutine(DoFlagellumPropulsion(Front_Left, fFront_Left));
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

    private IEnumerator DoFlagellumPropulsion(GameObject flagellumGo, Flagellum flagellumF)
    {
        if (!flagellumGo.activeSelf) yield return "success";

        while (true)
        {
            float flagellumDegree = flagellumGo.transform.rotation.eulerAngles.z;
            float degreeDif = Mathf.Abs(Mathf.DeltaAngle(flagellumDegree, degreeToObjective));
            float dedication = (180 - degreeDif)/180;

            Vector2 flagellumVector = flagellumGo.transform.position - flagellumGo.transform.GetChild(0).transform.position;

            Debug.DrawRay(flagellumGo.transform.position, flagellumVector * dedication * 10, Color.green, 0.5f);

            float force = dedication * flagellumGo.transform.localScale.y;

            rb2d.AddForce(flagellumVector*force, ForceMode2D.Impulse);
            rb2d.velocity = new Vector3(
                Mathf.Clamp(rb2d.velocity.x, -cell.cellProperties.maxVelocity, cell.cellProperties.maxVelocity),
                Mathf.Clamp(rb2d.velocity.y, -cell.cellProperties.maxVelocity, cell.cellProperties.maxVelocity), 0);

            /*
            Vector2 finalMove = flagellumVector * force;
            rb2d.transform.position += new Vector3(finalMove.x, finalMove.y, 0);
            */

            cell.energy.Modify(-force);


            yield return new WaitForSeconds(flagellumF.impulseFrequency);
        }
    }

}