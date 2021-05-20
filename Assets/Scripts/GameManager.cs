using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

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
    private GameObject _gameplayPanel;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private TMP_Text _currentScoreText;
    [SerializeField]
    private TMP_Text _highScoreText;

    private Rigidbody[] _playerRigidbodies;

    private bool _gameStarted;

    private int _currentScore;
    private int _highestScore;

    private void OnEnable()
    {
        GameOverEvent.AddListener(GameEnd);
        ArrowsHit = 0;
        GameOver = false;
        _gameStarted = false;
        _currentScore = 0;
        _highestScore = 0;

        if (PlayerPrefs.HasKey("SCORE"))
        {
            _highestScore = PlayerPrefs.GetInt("SCORE");
            _highScoreText.text = $"HIGH SCORE: {_highestScore}";
        }
        else
        {
            PlayerPrefs.SetInt("SCORE", _highestScore);
        }

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
            _gameplayPanel.SetActive(false);
        }

        if (_gameStarted && GameOver == false)
        {
            _currentScore = (int)_player.transform.position.y;
            if (_currentScore < 0)
            {
                _currentScore = 0;
            }

            if (_currentScore >= _highestScore)
            {
                _highestScore = _currentScore;
            }

            _currentScoreText.text = $"CURRENT SCORE: {_currentScore}";
            _highScoreText.text = $"HIGH SCORE: {_highestScore}";
        }


        //PlayerPrefs Tester
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
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

        PlayerPrefs.SetInt("SCORE", _highestScore);
        PlayerPrefs.Save();
    }

    public void OnResume()
    {
        _pausePanel.SetActive(false);
        _gameplayPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("Game");
    }
}