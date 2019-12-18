
using UnityEngine;

public class Speed
{
    private static int speed;

    public static void InitializeStatic()
    {
        speed = 1;
    }

    public static int GetSpeed()
    {
        return speed;
    }

    public static void AddSpeed()
    {
        speed += 1;
    }
}
