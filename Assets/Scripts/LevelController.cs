using System;
using UnityEngine;

public class LevelController
{
    private readonly string LEVEL_PATH = "Levels/Level ";

    private GameObject _level;

    public void LoadLevel()
    {
        if (_level != null)
            UnloadLevel();

        var prefab = Resources.Load<GameObject>(LEVEL_PATH + GameController.Instance.Config.Level);
        _level = UnityEngine.Object.Instantiate(prefab);
    }

    private void UnloadLevel()
    {
        UnityEngine.Object.Destroy(_level);
        _level = null;
    }
}
