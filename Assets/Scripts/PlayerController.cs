using Dreamteck.Splines;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerController
{
    #region Fields

    private readonly string AVATAR_PATH = "Avatars/Avatar ";
    private const float ACCELERATION_DURATION = 0.5f;
    private const float SLOWDOWN_DURATION = 0.2f;

    private GameObject _player;
    private SplineFollower _splineFollower;
    private Tween _movementTween;
    private Rigidbody[] _explosionRigidbodies;

    #endregion //Fields

    #region Events

    public Action<bool> GameOver;

    #endregion //Events

    #region Public Methods

    public void Init(SplineComputer splineComputer)
    {
        InstantiateAvatar();
        SubscribeTouchEvents();
        _splineFollower = _player.GetComponent<SplineFollower>();
        _splineFollower.spline = splineComputer;
        _player.GetComponent<OnTriggerEnterHandler>().HitToObstacle += OnHitToObstacle;
        _player.GetComponent<OnTriggerEnterHandler>().FinishLinePassed += OnFinishLinePassed;
        Stop();
    }

    private void OnFinishLinePassed()
    {
        //TODO: Start Finish sequence
        _player.GetComponent<OnTriggerEnterHandler>().FinishLinePassed -= OnFinishLinePassed;

        if (GameOver != null)
            GameOver.Invoke(true);
    }

    private void OnHitToObstacle()
    {
        Kill();
        OnPlayerKilled();
    }

    private void OnPlayerKilled()
    {
        _player.GetComponent<OnTriggerEnterHandler>().HitToObstacle -= OnHitToObstacle;

        if (GameOver != null)
            GameOver.Invoke(false);
    }

    public Transform GetPlayerTransform()
    {
        if (_player != null)
            return _player.transform;

        return null;
    }
        
    #endregion //Public Methods

    #region Private Methods

    private void InstantiateAvatar()
    {
        if (_player != null)
            UnityEngine.Object.Destroy(_player);

        var prefab = Resources.Load<GameObject>(AVATAR_PATH + GameController.Instance.Config.Avatar);
        _player = UnityEngine.Object.Instantiate(prefab);

        _explosionRigidbodies = _player.transform.GetChild(0).GetComponentsInChildren<Rigidbody>(true);
    }

    private void SubscribeTouchEvents()
    {
        TouchController.Instance.TouchBegan += Move;
        TouchController.Instance.TouchEnded += Stop;
    }

    private void Move()
    {
        _splineFollower.follow = true;

        if (_movementTween != null)
        {
            _movementTween.Kill();
            _movementTween = null;
        }

        _movementTween = DOTween.To(
            () => _splineFollower.followSpeed,
            x => _splineFollower.followSpeed = x,
            GameController.Instance.Config.MaxPlayerSpeed, ACCELERATION_DURATION)
            .OnComplete(() => { _movementTween = null; });
    }

    private void Stop()
    {
        if (_movementTween != null)
        {
            _movementTween.Kill();
            _movementTween = null;
        }

        _movementTween = DOTween.To(
            () => _splineFollower.followSpeed,
            x => _splineFollower.followSpeed = x, 0, SLOWDOWN_DURATION)
            .OnComplete(() => {
                _splineFollower.follow = false;
                _movementTween = null;
            });
    }

    private void Kill()
    {
        _player.GetComponent<MeshRenderer>().enabled = false;
        Explode();
    }

    private void Explode()
    {
        var cubesRoot = _player.transform.GetChild(0);
        cubesRoot.gameObject.SetActive(true);

        var explosionForce = 900f;
        var explosionRadius = 10f;
        var explosionUpward = 1f;

        foreach (var rb in _explosionRigidbodies)
            rb.AddExplosionForce(explosionForce, _player.transform.position, explosionRadius, explosionUpward);
    }

    #endregion //Private Methods
}
