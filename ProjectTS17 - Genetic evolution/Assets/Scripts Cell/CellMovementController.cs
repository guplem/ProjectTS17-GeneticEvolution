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

    public void Setup(Rigidbody2D rb2d, Flagellum[] flagellums, Cell cell)
    {
        Debug.Log("Setup MovementController: ");
        foreach (Flagellum flagellum in flagellums)
            Debug.Log(flagellum.ToString());

        foreach (Flagellum flagellum in flagellums)
        {
            bool state = (flagellum.impulseFrequency > 0);
                switch (flagellum.position)
            {
                case Flagellum.Position.Front:
                        Front.SetActive(state);
                    //fFront = flagellum;
                    break;
                case Flagellum.Position.Front_Right:
                    Front_Right.SetActive(state);
                    //fFront_Right = flagellum;
                    break;
                case Flagellum.Position.Right:
                    Right.SetActive(state);
                    //fRight = flagellum;
                    break;
                case Flagellum.Position.Back_Right:
                    Back_Right.SetActive(state);
                    //fBack_Right = flagellum;
                    break;
                case Flagellum.Position.Back:
                    Back.SetActive(state);
                    //fBack = flagellum;
                    break;
                case Flagellum.Position.Back_Left:
                    Back_Left.SetActive(state);
                    //fBack_Left = flagellum;
                    break;
                case Flagellum.Position.Left:
                    Left.SetActive(state);
                    //fLeft = flagellum;
                    break;
                case Flagellum.Position.Front_Left:
                    Front_Left.SetActive(state);
                    //fFront_Left = flagellum;
                    break;
                default:
                    Debug.LogError("Unnexpected value");
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
        //if (flagellumF == null) yield return "success";

        while (true)
        {

            float flagellumDegree = flagellumGo.transform.rotation.eulerAngles.z;
            float degreeDif = Mathf.Abs(Mathf.DeltaAngle(flagellumDegree, degreeToObjective));
            float dedication = (180 - degreeDif)/180;

            Vector2 flagellumVector = flagellumGo.transform.position - flagellumGo.transform.GetChild(0).transform.position;

            Debug.DrawRay(flagellumGo.transform.position, flagellumVector * dedication * 10, Color.green, flagellumGo.GetComponent<Flagellum>().impulseFrequency);

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


            yield return new WaitForSeconds(flagellumGo.GetComponent<Flagellum>().impulseFrequency);
        }
    }

}