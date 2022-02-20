using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Collections;
using PlayFab.ClientModels;
using PlayFab.Utils;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int ArrowsHit;
    public static bool GameOver;
    public static bool GameStarted;
    public static float LastArrowDirection;

    public static UnityEvent GameOverEvent = new UnityEvent();
    public static UnityEvent PlayerHitEvent = new UnityEvent();
    public static UnityEvent DisableMainMenuUI = new UnityEvent();

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _arrowGenerator;
    [SerializeField] private GameObject _logGenerator;
    [SerializeField] private Rigidbody _mainMenuHangerRigidbody;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _currentScoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private AudioClip _deathSoundClip;
    [SerializeField] private AudioClip _powerJumpSoundClip;
    [SerializeField] private GameObject _gameLogo;
    [SerializeField] private GameObject _startText;
    [SerializeField] private Material[] _PlayerHitMaterials;
    [SerializeField] private Button _retryButton;

    [SerializeField] private float _difficultyChangeLevel = 1000;
    private Rigidbody[] _playerRigidbodies;

    private AudioSource _audioSource;

    private int _currentScore;
    private int _highestScore;
    private float _gameStartTime;
    private int _dailyBoardVersion = -1;

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


        PlayFabPlayerDataController.PullPlayerData(OnUserDataPulled);
        _highScoreText.text = $"HIGH SCORE: {_highestScore}";

        _playerRigidbodies = _player.transform.root.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody r in _playerRigidbodies)
        {
            SpringJoint sj = r.gameObject.AddComponent<SpringJoint>();
            sj.connectedBody = _mainMenuHangerRigidbody;
        }

        _gameStartTime = Time.time;
        CheckForDailyReward();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (_highestScore % _difficultyChangeLevel == 0 )
        {
            _arrowGenerator.GetComponent<ArrowGenerator>().ArrowSpawnRate -= 0.1f;
        }
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

            StartCoroutine(nameof(DisableGameLogo));
        }

        if (GameStarted && GameOver == false)
        {
            _currentScore = (int) _player.transform.position.y;
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
        DisableMainMenuUI.Invoke();
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

        PlayFabPlayerDataController.PushPlayerData(PlayFabPlayerData.HIGH_SCORE, _highestScore.ToString());
        PlayFabLeaderboardController.SendLeaderboardStat(PlayFabLeaderboards.HIGHEST_SCORE, _highestScore);
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

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus != false) return;

        PlayFabLeaderboardController.SendLeaderboardStat(PlayFabLeaderboards.DAILY_TIME, (int) (Time.time - _gameStartTime));
    }

    private void OnApplicationQuit()
    {
        PlayFabLeaderboardController.SendLeaderboardStat(PlayFabLeaderboards.DAILY_TIME, (int) _gameStartTime);
    }

    private void OnUserDataPulled(GetUserDataResult result)
    {
        if (result.Data == null) return;
        var data = result.Data;
        if (data.ContainsKey(PlayFabPlayerData.HIGH_SCORE)) _highestScore = Int32.Parse(data[PlayFabPlayerData.HIGH_SCORE].Value);
        if (data.ContainsKey(PlayFabPlayerData.DAILY_BOARD_VER)) _dailyBoardVersion = Int32.Parse(data[PlayFabPlayerData.DAILY_BOARD_VER].Value);
    }

    private void CheckForDailyReward()
    {
        PlayFabLeaderboardController.GetLeaderboard(PlayFabLeaderboards.DAILY_TIME, OnGetLeaderBoard);
    }

    private void OnGetLeaderBoard(GetLeaderboardResult result)
    {
        var version = result.Version;
        if (version == _dailyBoardVersion) return;

        PlayFabLeaderboardController.GetLeaderboardByVersion(PlayFabLeaderboards.DAILY_TIME, _dailyBoardVersion, OnGetVersionedBoard);
        _dailyBoardVersion = version;
    }

    private void OnGetVersionedBoard(GetLeaderboardResult result)
    {
        //TODO: Give top player a reward.
        // if (result.Leaderboard[0].PlayFabId == PlayFabPlayerInfo.PlayFabID)
        // {
        // }
    }
}