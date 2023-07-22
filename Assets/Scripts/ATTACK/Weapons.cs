using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    //Camera References
    private CameraMovement _cameras;
    private Camera _activeCamera;

    //Agent References
    public bool _isAI = false;
    [HideInInspector] public int _AIWeapon = 0;
    [HideInInspector] public Transform _AITarget;

    //Weapons Variables
    [SerializeField] private GameObject[] _projectiles; //Prefabs
    private List<GameObject> _weapons  = new();
    private int _weaponIndex = 0;
    [SerializeField] private float _launchForceBolt;
    [SerializeField] private float _launchForceBomb;
    [SerializeField] private float _launchForceShuriken;
    private int _boltAOE = 5;
    private int _bombAOE = 3;
    private int _shurikenAOE = 1;
    private float _angleOffSet = 30f;

    //Trajectory Variables
    private Vector2 _direction;
    private float _angle;
    private Transform _shotPoint;
    private GameObject[] _points;
    [SerializeField] private GameObject _point;
    [SerializeField] private int _numOfPoints;
    [SerializeField] private float _spaceBtwPoints;

    // Start is called before the first frame update
    void Start()
    {
        _cameras = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraMovement>();

        //Get all weapons
        foreach (Transform childWeapon in transform)
        {
            if (childWeapon.name == "ShotPoint")
            {
                _shotPoint = childWeapon;
                break; //Ignore the last child (used for projectile starting point)
            }

            _weapons.Add(childWeapon.gameObject);
        }

        //Get the trajectory variables
        _points = new GameObject[_numOfPoints];
        for (int i = 0; i < _numOfPoints; i++)
        {
            _points[i] = Instantiate(_point, _shotPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_activeCamera == null)
            GetCamera();

        if (!_activeCamera.isActiveAndEnabled)
            GetCamera();

        if (!_isAI)
        {
            //Calculating where the weapon should be targeting
            Vector2 weaponPosition = transform.position;
            Vector2 mousePosition = _activeCamera.ScreenToWorldPoint(Input.mousePosition);
            _direction = (mousePosition - weaponPosition).normalized;
            _angle = Mathf.Atan2(_direction.y, _direction.x);

            //Move weapon
            transform.right = _direction;

            //Change weapons
            if (Input.GetKeyDown(KeyCode.F))
            {
                _weapons[_weaponIndex].SetActive(false);
                _weaponIndex = (_weaponIndex + 1) % _weapons.Count;
                _weapons[_weaponIndex].SetActive(true);
            }

            //Fire weapon
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(_weaponIndex);
            }

            //Draw Projectile Trajectory
            for (int i = 0; i < _numOfPoints; i++)
            {
                _points[i].transform.position = PointPosition(i * _spaceBtwPoints);
            }
        }
    }

    public void Shoot(int weaponIndex)
    {
        GameObject newProjectile = Instantiate(_projectiles[weaponIndex], _shotPoint.position, _shotPoint.rotation);

        switch (weaponIndex)
        {
            case 0: //BOLT
                newProjectile.GetComponent<Projectile>().SetTrajectoryParameters(_angle, _launchForceBolt, _direction, false, _boltAOE, true);
                break;

            case 1: //BOMB
                newProjectile.GetComponent<Projectile>().SetTrajectoryParameters(_angle, _launchForceBomb, _direction, false, _bombAOE, true);
                break;

            case 2: //SHURIKEN
                GameObject newProjectile_1 = Instantiate(_projectiles[weaponIndex], _shotPoint.position, _shotPoint.rotation);
                GameObject newProjectile_2 = Instantiate(_projectiles[weaponIndex], _shotPoint.position, _shotPoint.rotation);
                
                //Calculate new angles
                Quaternion newAngle_1 = Quaternion.AngleAxis(_angle * Mathf.Rad2Deg, Vector3.forward);
                Quaternion newAngle_2 = Quaternion.AngleAxis(-_angle * Mathf.Rad2Deg, Vector3.forward);
                newProjectile_1.transform.rotation = Quaternion.RotateTowards(newProjectile_1.transform.rotation, newAngle_1, _angleOffSet);
                newProjectile_2.transform.rotation = Quaternion.RotateTowards(newProjectile_2.transform.rotation, newAngle_2, -_angleOffSet);

                newProjectile.GetComponent<Projectile>().SetTrajectoryParameters(_angle, _launchForceShuriken, _direction, true, _shurikenAOE, true);
                newProjectile_1.GetComponent<Projectile>().SetTrajectoryParameters(_angle, _launchForceShuriken, _direction, true, _shurikenAOE, false);
                newProjectile_2.GetComponent<Projectile>().SetTrajectoryParameters(_angle, _launchForceShuriken, _direction, true, _shurikenAOE, false);
                break;
        }
    }

    public void AIAttack()
    {
        _direction = (_AITarget.position - transform.position).normalized;
        _angle = Mathf.Atan2(_direction.y, _direction.x);
        transform.right = _direction;

        Shoot(_AIWeapon);
    }

    public void GetCamera()
    {
        _activeCamera = _cameras.GetActiveCamera();
    }

    //Draw Projectile trajectory
    public Vector2 PointPosition(float t)
    {
        Vector2 position = Vector2.zero;
        switch(_weaponIndex)
        {
            case 0: //BOLT
                if (_direction.x <= 0) t *= -1;

                position.x = _shotPoint.position.x + t;
                position.y = _shotPoint.position.y + (t * Mathf.Tan(_angle)) - ((9.81f * t * t)
                     / (2 * _launchForceBolt * _launchForceBolt * Mathf.Cos(_angle) * Mathf.Cos(_angle)));
                break;

            case 1: //BOMB
                if (_direction.x <= 0) t *= -1;

                position.x = _shotPoint.position.x + t;
                position.y = _shotPoint.position.y + (t * Mathf.Tan(_angle)) - ((9.81f * t * t)
                     / (2 * _launchForceBomb * _launchForceBomb * Mathf.Cos(_angle) * Mathf.Cos(_angle)));
                break;

            case 2: //SHURIKEN
                position = (Vector2)_shotPoint.position + (_direction * _launchForceShuriken * t);
                break;
        }
        return position;
    }

    public void SetAI(int weaponIndex, Transform target)
    {
        _AIWeapon = weaponIndex;
        _AITarget = target;
    }
}