using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private GameObject[] _players;
    private GameObject[] _AIs;
    [SerializeField] private MenuManager _menuRef;
    public Text winnerText;

    // Start is called before the first frame update
    void Start()
    {
        //Start time again if frozen
        if (Time.timeScale == 0) Time.timeScale = 1;

        _players = GameObject.FindGameObjectsWithTag("Player");
        _AIs = GameObject.FindGameObjectsWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAgents();

        if (_players.Length == 0 && _AIs.Length == 0)
        {
            EndGame(true, true);
        }
        else if (_players.Length == 0)
        {
            EndGame(true, false);
        }
        else if (_AIs.Length == 0)
        {
            EndGame(false, false);
        }
    }

    public void UpdateAgents()
    {
        _players = null;
        _AIs = null;
        _players = GameObject.FindGameObjectsWithTag("Player");
        _AIs = GameObject.FindGameObjectsWithTag("AI");

        if (_players.Length == 0 || _AIs.Length == 0) ManualEndGame();
    }

    public void ManualEndGame()
    {
        UpdateAgents();

        if (_players.Length == _AIs.Length)
        {
            EndGame(true, true);
        }
        else if (_players.Length > _AIs.Length)
        {
            EndGame(true, false);
        }
        else
        {
            EndGame(false, false);
        }
    }

    public void EndGame(bool playerWin, bool isDraw)
    {
        Time.timeScale = 0;
        _menuRef.EndScreen();

        if (isDraw)
        {
            winnerText.text = "draw...";
        }
        else
        {
            if (playerWin)
            {
                winnerText.text = "player wins!!";
            }
            else
            {
                winnerText.text = "you lost...";
            }
        }
    }
}
