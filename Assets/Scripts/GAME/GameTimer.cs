using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    //Timer variables
    private float _gameTimer = 301f;
    private float _turnTimer = 21f;
    private float _timer = 21f;
    private bool _gameRunning = false;
    private bool _turnRunning = false;
    [SerializeField] private Text _timerText;
    [SerializeField] private Text _turnText;

    //Game Reference
    private TurnManager _turnManager;
    private GameControl _gameRef;

    // Start is called before the first frame update
    void Start()
    {
        _gameRunning = true;
        _turnRunning = true;
        _turnManager = gameObject.GetComponent<TurnManager>();
        _gameRef = gameObject.GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameRunning)
        {
            _gameTimer -= Time.deltaTime;
            RunGameTimer();

            if (_gameTimer < 0)
                _gameRunning = false;
        }
        else
        {
            _gameRef.ManualEndGame();
        }

        if (_turnRunning)
        {
            _timer -= Time.deltaTime;
            RunTurnTimer();

            if (_timer < 0)
            {
                NextTurnTimer();
            }
        }
    }

    public void RunGameTimer()
    {
        float minutes = Mathf.FloorToInt(_gameTimer / 60);
        float seconds = Mathf.FloorToInt(_gameTimer % 60);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RunTurnTimer()
    {
        float minutes = Mathf.FloorToInt(_timer / 60);
        float seconds = Mathf.FloorToInt(_timer % 60);

        _turnText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void NextTurnTimer()
    {
        _gameRef.UpdateAgents();
        _timer = _turnTimer;
        _turnManager.NextTurn();
    }
}
