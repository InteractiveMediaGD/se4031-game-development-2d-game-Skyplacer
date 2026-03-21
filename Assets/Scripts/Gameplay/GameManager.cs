using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float GlobalSpeed = 5f;
    public float acceleration = 0.1f;

    void Update()
    {
        // Gradually increase speed (Requirement 6)
        GlobalSpeed += acceleration * Time.deltaTime;
    }
}
