using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Fields

    public static UIController Instance { get; private set; }

    [SerializeField] private Button _buttonMove;
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonSettings;
    [SerializeField] private Button _buttonClaim;
    [SerializeField] private Slider _sliderPseudoLevel;
    [SerializeField] private TextMeshProUGUI _textPseudoLevel;
    [SerializeField] private TextMeshProUGUI _textEndGamePercentage;

    #endregion //Fields

    #region Events

    public Action PlayButtonPressed;
    public Action RestartButtonPressed;
    public Action SettingsButtonPressed;
    public Action ClaimButtonPressed;

    #endregion // Events

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

        SubscribeButtonEvents();
    }

    #endregion //Unity Methods

    #region Private Methods

    private void SubscribeButtonEvents()
    {
        _buttonPlay.onClick.AddListener(OnPlayButtonPressed);
        _buttonRestart.onClick.AddListener(OnRestartButtonPressed);
        _buttonSettings.onClick.AddListener(OnSettingsButtonPressed);
        _buttonClaim.onClick.AddListener(OnClaimButtonPressed);
    }

    private void OnPlayButtonPressed()
    {
        if (PlayButtonPressed != null)
            PlayButtonPressed.Invoke();
    }

    private void OnRestartButtonPressed()
    {
        if (RestartButtonPressed != null)
            RestartButtonPressed.Invoke();
    }

    private void OnSettingsButtonPressed()
    {
        if (SettingsButtonPressed != null)
            SettingsButtonPressed.Invoke();
    }

    private void OnClaimButtonPressed()
    {
        if (ClaimButtonPressed != null)
            ClaimButtonPressed.Invoke();
    }

    #endregion //Private Methods

    #region Public Methods

    public void ShowMenuUI()
    {
        _buttonMove.gameObject.SetActive(false);
        _buttonPlay.gameObject.SetActive(true);
        _buttonRestart.gameObject.SetActive(false);
        _buttonSettings.gameObject.SetActive(true);
        _buttonClaim.gameObject.SetActive(false);
        _sliderPseudoLevel.gameObject.SetActive(true);
        _textEndGamePercentage.gameObject.SetActive(false);
    }

    public void ShowInGameUI()
    {
        _buttonMove.gameObject.SetActive(true);
        _buttonPlay.gameObject.SetActive(false);
        _buttonRestart.gameObject.SetActive(false);
        _buttonSettings.gameObject.SetActive(false);
        _buttonClaim.gameObject.SetActive(false);
        _sliderPseudoLevel.gameObject.SetActive(true);
        _textEndGamePercentage.gameObject.SetActive(false);
    }

    public void ShowEndGameUI(bool levelPassed = false)
    {
        _buttonMove.gameObject.SetActive(false);
        _buttonPlay.gameObject.SetActive(false);
        _buttonRestart.gameObject.SetActive(!levelPassed);
        _buttonSettings.gameObject.SetActive(false);
        _buttonClaim.gameObject.SetActive(levelPassed);
        _sliderPseudoLevel.gameObject.SetActive(true);
        _textEndGamePercentage.gameObject.SetActive(true);

        if (levelPassed)
            _textEndGamePercentage.text = "Level Completed!";
        else
            _textEndGamePercentage.text = "%0 Completed";
    }

    #endregion //Public Methods
}
