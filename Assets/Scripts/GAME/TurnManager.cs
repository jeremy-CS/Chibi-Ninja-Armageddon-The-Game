using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    //Player References
    private GameObject[] _players;
    private GameObject[] _AIPlayers;
    [HideInInspector] public GameObject[] _agents;
    [HideInInspector] public int _currentAgentIndex = 0;

    //Script References
    private CameraMovement _cameraRef;

    // Start is called before the first frame update
    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        _AIPlayers = GameObject.FindGameObjectsWithTag("AI");
        _agents = AddAgents(_players, _AIPlayers);
        _cameraRef = GameObject.FindGameObjectWithTag("CameraController").GetComponent<CameraMovement>();

        foreach(GameObject agent in _agents)
        {
            if (agent.GetComponent<Player>() != null)
            {
                agent.GetComponent<Player>()._isTurn = false;
            }
            else
            {
                agent.GetComponent<AI>()._isTurn = false;
            }
        }

        _agents[_currentAgentIndex].GetComponent<Player>()._isTurn = true;
    }

    public void NextTurn()
    {
        //Check if dead
        if (_agents[_currentAgentIndex] == null)
        {
            ChangeTurn();
        }
        else
        {
            if (_agents[_currentAgentIndex].GetComponent<Player>() != null)
            {
                _agents[_currentAgentIndex].GetComponent<Player>()._isTurn = false;
                GetNextAgent();
                ChangeTurn();
            }
            else
            {
                _agents[_currentAgentIndex].GetComponent<AI>()._isTurn = false;
                GetNextAgent();
                ChangeTurn();
            }
        }

        _cameraRef.GetNextAgent();
    }

    public void GetNextAgent()
    {
        _currentAgentIndex = (_currentAgentIndex + 1) % _agents.Length;
    }

    public void ChangeTurn()
    {
        while (_agents[_currentAgentIndex] == null)
        {
            GetNextAgent();
        }

        if (_agents[_currentAgentIndex].GetComponent<AI>() != null)
        {
            _agents[_currentAgentIndex].GetComponent<AI>()._isTurn = true;
        }
        else
        {
            _agents[_currentAgentIndex].GetComponent<Player>()._isTurn = true;
        }
    }

    public GameObject[] AddAgents(GameObject[] players, GameObject[] AIs)
    {
        GameObject[] agents = new GameObject[players.Length+AIs.Length];

        for (int i = 0; i < players.Length; i++)
        {
            agents[2 * i] = players[i];
            agents[2 * i + 1] = AIs[i];
        }

        return agents;
    }

    public int GetCurrentAgentIndex()
    {
        return _currentAgentIndex;
    }
}
