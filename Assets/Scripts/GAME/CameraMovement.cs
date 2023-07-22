using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Different Camera References
    [SerializeField] private GameObject _mapCamera;
    [SerializeField] private GameObject _agentCamera;
    private GameObject _activeCamera;
    [HideInInspector] public bool _followProjectile;
    private float _cameraMoveSpeed = 2f;

    //Agent Variables
    private GameObject[] _agents;
    private int _nextAgentIndex = 0;

    //Turn Manager Ref
    private TurnManager _turnManager;

    // Start is called before the first frame update
    void Start()
    {
        _turnManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnManager>();
        _agents = _turnManager._agents;
        _activeCamera = _agentCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GetNextAgent();
            _followProjectile = false;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_mapCamera.activeSelf)
            {
                _mapCamera.SetActive(false);
                _agentCamera.SetActive(true);
                _activeCamera = _agentCamera;
            }
            else
            {
                _mapCamera.SetActive(true);
                _agentCamera.SetActive(false);
                _activeCamera = _mapCamera;
            }
        }

        if (_agents != null && !_followProjectile)
        {
            while (_agents[_nextAgentIndex] == null)
                GetNextAgent();

            MoveCamera(_agents[_nextAgentIndex]);
        }
    }

    //Move camera to next player
    public void MoveCamera(GameObject nextAgent)
    {
        if (nextAgent != null)
        {
            // Smooth move camera to the location of the currently active player or "agent"
            var camPosition = _agentCamera.transform.position;
            var agentPosition = nextAgent.transform.position;

            agentPosition.z = _agentCamera.transform.position.z;
            _agentCamera.transform.position = Vector3.Lerp(camPosition, agentPosition, _cameraMoveSpeed * Time.deltaTime);
        }
    }

    public void GetNextAgent()
    {
        _nextAgentIndex = _turnManager.GetCurrentAgentIndex();
    }

    public Camera GetActiveCamera()
    {
        Camera currentCamera = _activeCamera.GetComponent<Camera>();
        return currentCamera;
    }

    public void SetProjectileFollow(bool isFollowing)
    {
        _followProjectile = isFollowing;
    }
}
