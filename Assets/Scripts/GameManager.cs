using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;
    public static bool GameOver;
    public static Vector2 LastArrowDirection;

    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        ArrowsHit = 0;
        GameOver = false;
    }

    private void Update()
    {
        if (ArrowsHit >= 3)
        {
            GameOver = true;
            if (LastArrowDirection.x >= 0)
            {
                _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                _player.GetComponent<Rigidbody>().velocity = new Vector3(40, _player.GetComponent<Rigidbody>().velocity.y, 0);
            }
            else if (LastArrowDirection.x < 0)
            {
                _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                _player.GetComponent<Rigidbody>().velocity = new Vector3(-40, _player.GetComponent<Rigidbody>().velocity.y, 0);
            }
        }
    }
}