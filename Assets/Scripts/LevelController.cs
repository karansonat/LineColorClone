using Dreamteck.Splines;
using UnityEngine;

public class LevelController
{
    private readonly string LEVEL_PATH = "Levels/Level ";

    private GameObject _level;
    private Animator _animator;

    public void LoadLevel()
    {
        if (_level != null)
            UnloadLevel();

        var prefab = Resources.Load<GameObject>(LEVEL_PATH + GameController.Instance.Config.Level);
        _level = UnityEngine.Object.Instantiate(prefab);
        _animator = _level.transform.Find("Chest").GetComponent<Animator>();
    }

    public SplineComputer GetSplineComputer()
    {
        return _level.GetComponent<SplineComputer>();
    }

    public void OpenChest()
    {
        _animator.SetTrigger("Open");
    }

    private void UnloadLevel()
    {
        UnityEngine.Object.Destroy(_level);
        _level = null;
    }
}
