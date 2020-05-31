using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    #region Fields

    public static FollowCamera Instance { get; private set; }

    [SerializeField] private Transform _target;
    public Vector3 Offset;

    #endregion //Fields

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

    private void LateUpdate()
    {
        if (_target != null)
            FollowTarget();
    }

    #endregion //Unity Methods

    #region Public Methods

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    #endregion //Public Methods

    #region Private Methods

    private void FollowTarget()
    {
        transform.position = _target.position + Offset;
    }

    #endregion //Private Methods
}
