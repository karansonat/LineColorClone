using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Create New Game Config")]
public class GameConfig : ScriptableObject
{
    private const int LEVEL_COUNT = 2;

    [SerializeField] private int _level;
    public int Level { get { return _level; } }

    [SerializeField] private int _coin;
    public int Coin { get { return _coin; } }

    [SerializeField] private int _avatar;
    public int Avatar { get { return _avatar; } }

    #region Public Methods

    public void Initialize()
    {
        SubscribeEvents();
    }

    #endregion //Public Methods

    #region Private Methods

    private void SubscribeEvents()
    {
        GameController.Instance.LevelPassed += OnLevelPassed;
        GameController.Instance.CoinAmountUpdated += OnCoinAmountUpdated;
        GameController.Instance.AvatarUpdated += OnAvatarUpdated;
    }

    private void OnLevelPassed()
    {
        _level++;
        _level %= LEVEL_COUNT;
    }

    private void OnCoinAmountUpdated(int newAmount)
    {
        _coin = newAmount;
    }

    private void OnAvatarUpdated(int newAvatar)
    {
        _avatar = newAvatar;
    }

    #endregion //Private Methods
}
