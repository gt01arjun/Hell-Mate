using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;
    public static bool GameOver;
    public static float LastArrowDirection;

    public static UnityEvent GameOverEvent = new UnityEvent();

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _arrowGenerator;
    [SerializeField]
    private Rigidbody _mainMenuHangerRigidbody;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _gameOverPanel;

    private bool _gameStarted;

    private Rigidbody[] _playerRigidbodies;

    private void OnEnable()
    {
        GameOverEvent.AddListener(GameEnd);
        ArrowsHit = 0;
        GameOver = false;
        _gameStarted = false;

        _playerRigidbodies = _player.transform.root.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody r in _playerRigidbodies)
        {
            SpringJoint sj = r.gameObject.AddComponent<SpringJoint>();
            sj.connectedBody = _mainMenuHangerRigidbody;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _gameStarted == false)
        {
            _gameStarted = true;

            foreach (Rigidbody r in _playerRigidbodies)
            {
                r.AddForce(Vector3.up * 40, ForceMode.Impulse);
            }

            foreach (Rigidbody r in _playerRigidbodies)
            {
                Destroy(r.gameObject.GetComponent<SpringJoint>());
            }

            _arrowGenerator.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
    }

    private void GameEnd()
    {
        GameOver = true;
        if (LastArrowDirection >= 0)
        {
            _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _player.GetComponent<Rigidbody>().velocity = new Vector3(80, _player.GetComponent<Rigidbody>().velocity.y, 0);
        }
        else if (LastArrowDirection < 0)
        {
            _player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            _player.GetComponent<Rigidbody>().velocity = new Vector3(-80, _player.GetComponent<Rigidbody>().velocity.y, 0);
        }
        _gameOverPanel.SetActive(true);
        _arrowGenerator.SetActive(false);
    }

    public void OnResume()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("Game");
    }
}