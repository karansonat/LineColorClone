using Dreamteck.Splines;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

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
    private ParticleSystem _particles;
    private Color _playerColor;

    #endregion //Fields

    #region Events

    public Action<bool, int> GameOver;

    #endregion //Events

    #region Public Methods

    public void Init(SplineComputer splineComputer)
    {
        InstantiateAvatar();
        _explosionRigidbodies = _player.transform.GetChild(0).GetComponentsInChildren<Rigidbody>(true);
        _particles = _player.GetComponentInChildren<ParticleSystem>();
        SetColors();
        SubscribeTouchEvents();
        SetupFollower(splineComputer);
        _player.GetComponent<OnTriggerEnterHandler>().HitToObstacle += OnHitToObstacle;
        _player.GetComponent<OnTriggerEnterHandler>().FinishLinePassed += OnFinishLinePassed;
        Stop();
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
    }

    private void SetupFollower(SplineComputer splineComputer)
    {
        _splineFollower = _player.GetComponent<SplineFollower>();
        _splineFollower.spline = splineComputer;

        //To limit player's path to finish line ( last-1th point ) Last point setted for the path between Finish line and chest.
        var clipToPercent = splineComputer.GetPointPercent(splineComputer.GetPoints().Length - 2);
        _splineFollower.clipTo = clipToPercent;

        _splineFollower.onMotionApplied += OnPlayerMoved;
    }

    private void SetColors()
    {
        var color = new Color(
          UnityEngine.Random.Range(0f, 1f),
          UnityEngine.Random.Range(0f, 1f),
          UnityEngine.Random.Range(0f, 1f)
        );

        _player.GetComponent<MeshRenderer>().sharedMaterial.color = color;
        var main = _particles.main;
        main.startColor = new Color(color.r - 0.2f, color.g - 0.2f, color.b - 0.2f);
        _player.GetComponent<LineDrawer>().SetColor(color);
    }

    private void OnPlayerMoved()
    {
        UIController.Instance.SetPercentage(_splineFollower.result.percent);
    }

    private void SubscribeTouchEvents()
    {
        TouchController.Instance.TouchBegan += Move;
        TouchController.Instance.TouchEnded += Stop;
    }

    private void UnSubscribeTouchEvents()
    {
        TouchController.Instance.TouchBegan -= Move;
        TouchController.Instance.TouchEnded -= Stop;
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

    private void OnFinishLinePassed()
    {
        _movementTween.Kill();
        UnSubscribeTouchEvents();
        _splineFollower.onMotionApplied -= OnPlayerMoved;
        _player.GetComponent<OnTriggerEnterHandler>().FinishLinePassed -= OnFinishLinePassed;
        StartFinishSequence();
    }

    private void StartFinishSequence()
    {
        var prevClipTo = _splineFollower.clipTo;
        _splineFollower.clipFrom = prevClipTo;
        _splineFollower.clipTo = 1;
        _splineFollower.SetPercent(0);
        _splineFollower.followSpeed = 5;
        _splineFollower.follow = true;
        _splineFollower.onEndReached += PlayerReachedEndPoint;

        FollowCamera.Instance.StartLevelCompletedAnimation();
    }

    private void PlayerReachedEndPoint()
    {
        if (GameOver != null)
            GameOver.Invoke(true, 100);
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
            GameOver.Invoke(false, (int)(_splineFollower.result.percent * 100f));

        UnSubscribeTouchEvents();
    }

    #endregion //Private Methods
}
