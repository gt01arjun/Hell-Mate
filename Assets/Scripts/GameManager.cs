using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;
    public static bool GameOver;
    public static bool GameStarted;
    public static float LastArrowDirection;

    public static UnityEvent GameOverEvent = new UnityEvent();

    public static UnityEvent PlayerHitEvent = new UnityEvent();

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
    [SerializeField]
    private Material[] _PlayerHitMaterials;
    [SerializeField]
    private Button _retryButton;

    private Rigidbody[] _playerRigidbodies;


    private AudioSource _audioSource;

    private int _currentScore;
    private int _highestScore;

    private void OnEnable()
    {
        GameOverEvent.AddListener(GameEnd);
        PlayerHitEvent.AddListener(PlayerHit);
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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameStarted == false)
        {
            GameStarted = true;

            foreach (Rigidbody r in _playerRigidbodies)
            {
                r.AddForce(Vector3.up * 85, ForceMode.Impulse);
            }

            foreach (Rigidbody r in _playerRigidbodies)
            {
                Destroy(r.gameObject.GetComponent<SpringJoint>());
            }

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

        if (_currentScore > 150 && _logGenerator.activeSelf == false)
        {
            _logGenerator.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && GameOver == true)
        {
            Graphic graphic = _retryButton.GetComponent<Graphic>();
            graphic.CrossFadeColor(_retryButton.colors.pressedColor, _retryButton.colors.fadeDuration, true, true);
            _retryButton.onClick.Invoke();
            OnMainMenu();
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
        _arrowGenerator.SetActive(true);
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

    private void PlayerHit()
    {
        switch (GameManager.ArrowsHit)
        {
            case 1:
                _player.transform.root.GetComponentInChildren<SkinnedMeshRenderer>().material = _PlayerHitMaterials[0];
                break;

            case 2:
                _player.transform.root.GetComponentInChildren<SkinnedMeshRenderer>().material = _PlayerHitMaterials[1];
                break;

            case 3:
                _player.transform.root.GetComponentInChildren<SkinnedMeshRenderer>().material = _PlayerHitMaterials[2];
                break;
            default:
                break;
        }
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("Game");
    }
}