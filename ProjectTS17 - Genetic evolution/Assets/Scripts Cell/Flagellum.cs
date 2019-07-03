
[System.Serializable]
public class Flagellum
{
    public enum Position
    {
        Front,
        Front_Right,
        Right,
        Back_Right,
        Back,
        Back_Left,
        Left,
        Front_Left,
    }
    public Position position;
    public float size = 1;
    public float impulseFrequency = 0.5f;
}