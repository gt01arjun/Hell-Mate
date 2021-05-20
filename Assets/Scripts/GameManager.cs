using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;
    public static bool GameOver;
    public static bool GameStarted;
    public static float LastArrowDirection;

    public static UnityEvent GameOverEvent = new UnityEvent();

    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _arrowGenerator;
    [SerializeField]
    private GameObject _logGenerator;
    [SerializeField]
    private Rigidbody _mainMenuHangerRigidbody;
    [SerializeField]
    private GameObject _gameplayPanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private TMP_Text _currentScoreText;
    [SerializeField]
    private TMP_Text _highScoreText;
    [SerializeField]
    private GameObject _mainCamera;
    [SerializeField]
    private AudioClip _deathSoundClip;
    [SerializeField]
    private AudioClip _powerJumpSoundClip;
    [SerializeField]
    private GameObject _gameLogo;
    [SerializeField]
    private GameObject _startText;

    private Rigidbody[] _playerRigidbodies;


    private AudioSource _audioSource;

    private int _currentScore;
    private int _highestScore;

    private void OnEnable()
    {
        GameOverEvent.AddListener(GameEnd);
        ArrowsHit = 0;
        GameOver = false;
        GameStarted = false;
        _currentScore = 0;
        _highestScore = 0;

        _audioSource = GetComponent<AudioSource>();

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
        if (Input.GetKeyDown(KeyCode.Space) && GameStarted == false)
        {
            GameStarted = true;

            foreach (Rigidbody r in _playerRigidbodies)
            {
                r.AddForce(Vector3.up * 70, ForceMode.Impulse);
            }

            foreach (Rigidbody r in _playerRigidbodies)
            {
                Destroy(r.gameObject.GetComponent<SpringJoint>());
            }

            _arrowGenerator.SetActive(true);

            DOTween.To(x => _mainCamera.GetComponent<CameraFollow>().Offset.y = x, 0, 4, 1f);

            _currentScoreText.gameObject.SetActive(true);
            _highScoreText.gameObject.SetActive(true);

            _audioSource.PlayOneShot(_powerJumpSoundClip);

            _startText.SetActive(false);

            StartCoroutine("DisableGameLogo");
        }

        if (GameStarted && GameOver == false)
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

        if (_currentScore > 70 && _logGenerator.activeSelf == false)
        {
            _logGenerator.SetActive(true);
        }


        //PlayerPrefs Tester
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private IEnumerator DisableGameLogo()
    {
        yield return new WaitForSeconds(2f);
        _gameLogo.SetActive(false);
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

        _audioSource.PlayOneShot(_deathSoundClip);

        PlayerPrefs.SetInt("SCORE", _highestScore);
        PlayerPrefs.Save();
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("Game");
    }
}