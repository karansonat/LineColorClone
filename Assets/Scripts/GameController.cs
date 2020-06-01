using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Fields

    public static GameController Instance { get; private set; }

    public GameConfig Config;
    private LevelController _levelController;
    private PlayerController _playerController;

    #endregion //Fields

    #region Events

    public Action LevelPassed;
    public Action<int> CoinAmountUpdated;
    public Action<int> AvatarUpdated;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
        }

        #endregion //Singleton

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Initialize();
        SubscribeUIEvents();
        InitializeGame();   
    }

    private void Update()
    {
#if UNITY_EDITOR
        #region Debug Methods

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Level Completed");
            GameOver(true);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Failed");
            GameOver();
        }

        #endregion //Debug Methods
#endif
    }

    #endregion //Unity Methods

    #region Private Methods

    private void Initialize()
    {
        Config.Initialize();
        _levelController = new LevelController();
        _playerController = new PlayerController();

        Physics.gravity = new Vector3(0, -50, 0);
    }

    private void InitializeGame()
    {
        _levelController.LoadLevel();
        InitializePlayer();
        UIController.Instance.ShowMenuUI();
    }

    private void SubscribeUIEvents()
    {
        UIController.Instance.PlayButtonPressed += StartGame;
        UIController.Instance.RestartButtonPressed += RestartGame;
        UIController.Instance.ClaimButtonPressed += InitializeGame;
    }

    private void InitializePlayer()
    {
        _playerController.Init(_levelController.GetSplineComputer());
        FollowCamera.Instance.SetTarget(_playerController.GetPlayerTransform());
        _playerController.GameOver += OnGameOver;
    }

    private void RestartGame()
    {
        InitializeGame();
    }

    private void StartGame()
    {
        UIController.Instance.ShowInGameUI();
        TouchController.Instance.EnableInput();
    }

    private void GameOver(bool levelPassed = false, int percentage = 100)
    {
        TouchController.Instance.DisableInput();
        UIController.Instance.ShowEndGameUI(levelPassed, percentage);

        if (levelPassed)
        {
            LevelPassed.Invoke();
            CoinAmountUpdated.Invoke(Config.Coin + 50);
            _levelController.OpenChest();
        }
    }

    private void OnGameOver(bool levelPassed, int percentage)
    {
        GameOver(levelPassed, percentage);
    }

    #endregion //Private Methods
}
