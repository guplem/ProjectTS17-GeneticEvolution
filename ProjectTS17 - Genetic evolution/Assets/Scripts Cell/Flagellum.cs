
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
    public float size;
    public float impulseFrequency;

    public Flagellum(Position position, float size, float impulseFrequency)
    {
        this.position = position;
        this.size = size;
        this.impulseFrequency = impulseFrequency;
    }

    public Flagellum(Position position)
    {
        this.position = position;
        this.size = 1;
        this.impulseFrequency = 0.5f;
    }
}