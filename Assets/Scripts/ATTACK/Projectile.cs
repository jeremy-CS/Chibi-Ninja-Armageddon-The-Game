using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _hasHit;
    private bool _straightTrajectory;

    //Movement Variables
    private float _angle;
    private float _velocity;
    private Vector2 _direction;
    private Vector2 _startPosition;
    private float _timeStep = 0f;
    private float _timer = 11f;

    //Tile destruction variable
    [HideInInspector] public int _AOE;

    //Out of Bounds references
    private GameObject[] _outOfBounds;

    //Turn Manager ref
    private GameTimer _turnTimer;

    //Camera Reference
    private CameraMovement _cameraRef;
    private bool _follow;

    // Start is called before the first frame update
    void Awake()
    {
        _outOfBounds = GameObject.FindGameObjectsWithTag("DeadZone");
        _turnTimer = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTimer>();
        _cameraRef = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!_hasHit || _timer > 0f)
        {
            MoveProjectile();
            if (_follow)
            {
                _cameraRef.SetProjectileFollow(true);
                _cameraRef.MoveCamera(this.gameObject);
            }
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetTrajectoryParameters(float angle, float velocity, Vector2 direction, bool isStraight, int aoe, bool follow)
    {
        _angle = angle;
        _velocity = velocity;
        _direction = direction;
        _startPosition = transform.position;
        _straightTrajectory = isStraight;
        _AOE = aoe;
        _follow = follow;
    }

    void MoveProjectile()
    {
        if (_straightTrajectory)
        {
            transform.position = (Vector3)(_startPosition + (_direction * _velocity * _timeStep));
        }
        else
        {
            float direction = _direction.x <= 0 ? -1 : 1;

            float x = _startPosition.x + (direction * _timeStep);
            float y = _startPosition.y + (direction * _timeStep * Mathf.Tan(_angle)) - ((9.81f * direction * direction * _timeStep * _timeStep)) 
                / (2 * _velocity * _velocity * Mathf.Cos(_angle) * Mathf.Cos(_angle));
            transform.position = new Vector3(x, y, 0f);
        }
        _timeStep += 0.1f;
        _timer -= _timeStep;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hasHit = true;
        if (_follow)
        {
            _turnTimer.NextTurnTimer();
            _cameraRef.SetProjectileFollow(false);
        }

        Destroy(collision.gameObject);
        Destroy(this.gameObject);
    }

    public void DestroyProjectileRef()
    {
        _turnTimer.NextTurnTimer();
        _cameraRef.SetProjectileFollow(false);
    }

    public int GetAOE()
    {
        return _AOE;
    }

    public bool GetFollow()
    {
        return _follow;
    }
}
