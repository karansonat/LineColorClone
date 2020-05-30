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
    }

    private void Start()
    {
        Initialize();
        SubscribeUIEvents();
        InitializeGame();   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Level Completed");
            UIController.Instance.ShowEndGameUI(true);
            LevelPassed.Invoke();
            CoinAmountUpdated.Invoke(Config.Coin + 50);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Failed");
            UIController.Instance.ShowEndGameUI();
        }
    }

    #endregion //Unity Methods

    #region Private Methods

    private void Initialize()
    {
        Config.Initialize();
        _levelController = new LevelController();
        _playerController = new PlayerController();
    }

    private void InitializeGame()
    {
        _levelController.LoadLevel();
        UIController.Instance.ShowMenuUI();
    }

    private void SubscribeUIEvents()
    {
        UIController.Instance.PlayButtonPressed += StartGame;
        UIController.Instance.RestartButtonPressed += RestartGame;
    }

    private void RestartGame()
    {
        InitializeGame();
    }

    private void StartGame()
    {
        UIController.Instance.ShowInGameUI();
    }

    #endregion //Private Methods
}
