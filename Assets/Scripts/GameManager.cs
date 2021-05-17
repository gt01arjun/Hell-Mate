using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;

    public static bool GameOver;

    private void Start()
    {
        ArrowsHit = 0;
        GameOver = false;
    }

    private void Update()
    {
        if(ArrowsHit >= 3)
        {
            GameOver = true;
        }
    }
}