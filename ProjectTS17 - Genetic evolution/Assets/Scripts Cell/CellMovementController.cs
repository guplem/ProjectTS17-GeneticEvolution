using System;
using UnityEngine;

#pragma warning disable CS0649
public class CellMovementController : MonoBehaviour
{
    [SerializeField] private GameObject Front, Front_Right, Right, Back_Right, Back, Back_Left, Left, Front_Left;
    private Flagellum[] flagellums;
    private Rigidbody2D rb2d;

    internal void setup(Rigidbody2D rb2d, Flagellum[] flagellums)
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
                    break;
                case Flagellum.Position.Front_Right:
                    Front_Right.SetActive(true);
                    break;
                case Flagellum.Position.Right:
                    Right.SetActive(true);
                    break;
                case Flagellum.Position.Back_Right:
                    Back_Right.SetActive(true);
                    break;
                case Flagellum.Position.Back:
                    Back.SetActive(true);
                    break;
                case Flagellum.Position.Back_Left:
                    Back_Left.SetActive(true);
                    break;
                case Flagellum.Position.Left:
                    Left.SetActive(true);
                    break;
                case Flagellum.Position.Front_Left:
                    Front_Left.SetActive(true);
                    break;
                default:
                    Debug.LogError("Unnexpected value");
                    break;
            }
        }

        this.flagellums = flagellums;
        this.rb2d = rb2d;
    }

    public void MoveTowards(Vector2 objectivePosition)
    {
        Vector2 p2 = objectivePosition;
        Vector2 p1 = transform.position;
        float degree = (Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI)-90;
        if (degree < 0) degree = 360 + degree;

        DoFlagellumPropulsion(degree, Front);
        DoFlagellumPropulsion(degree, Front_Right);
        DoFlagellumPropulsion(degree, Right);
        DoFlagellumPropulsion(degree, Back_Right);
        DoFlagellumPropulsion(degree, Back);
        DoFlagellumPropulsion(degree, Back_Left);
        DoFlagellumPropulsion(degree, Left);
        DoFlagellumPropulsion(degree, Front_Left);
    }

    private void DoFlagellumPropulsion(float degreeToObjective, GameObject flagellum)
    {
        if (!flagellum.activeSelf) return;

        float flagellumDegree = flagellum.transform.rotation.eulerAngles.z;
        float degreeDif = Mathf.Abs(Mathf.DeltaAngle(flagellumDegree, degreeToObjective));
        float dedication = (180 - degreeDif)/180;

        float force = dedication * flagellum.transform.localScale.y;

        
        //Canviar l'ordre de la resta fa que s'allunyi en comptes de que s'apropi
        Vector2 flagellumVector = flagellum.transform.position - flagellum.transform.GetChild(0).transform.position;

        //Debug.DrawRay(flagellum.transform.position, flagellumVector * force * 10, Color.green, 0.5f);

        rb2d.AddForce(flagellumVector*force, ForceMode2D.Impulse);
    }

    public void Avoid(Vector2 avoidingPosition)
    {

    }

    private void DoFlagellumPropulsion(GameObject gameObject, object flagellum)
    {
        throw new NotImplementedException();
    }

}