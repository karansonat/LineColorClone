using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    #region Fields

    public static FollowCamera Instance { get; private set; }

    private Animator _animator;

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

        _animator = GetComponent<Animator>();
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
        _animator.ResetTrigger("LevelCompleted");
        _animator.Play("Idle", -1, 0f);
    }

    public void StartLevelCompletedAnimation()
    {
        _animator.SetTrigger("LevelCompleted");
    }

    #endregion //Public Methods

    #region Private Methods

    private void FollowTarget()
    {
        transform.position = _target.position + Offset;
    }

    #endregion //Private Methods
}
