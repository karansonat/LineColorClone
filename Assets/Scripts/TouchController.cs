using System;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    #region Fields

    public static TouchController Instance { get; private set; }

    private bool _inputEnabled;

    #endregion //Fields

    #region Events

    public Action TouchBegan;
    public Action TouchEnded;

    #endregion //Events

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

    private void Update()
    {
        if (!_inputEnabled)
            return;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
            OnTouchBegan();

        if (Input.GetKeyUp(KeyCode.M))
            OnTouchEnded();
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            OnTouchBegan();

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            OnTouchEnded();
#endif
    }

    #endregion //Unity Methods

    #region Public Methods

    public void EnableInput()
    {
        _inputEnabled = true;
    }

    public void DisableInput()
    {
        _inputEnabled = false;
        OnTouchEnded();
    }

    #endregion //Public Methods

    #region Private Methods

    private void OnTouchBegan()
    {
        if (TouchBegan != null)
            TouchBegan.Invoke();
    }

    private void OnTouchEnded()
    {
        if (TouchEnded != null)
            TouchEnded.Invoke();
    }

    #endregion // Private Methods
}
