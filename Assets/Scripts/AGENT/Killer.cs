using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    private GameTimer _gameTimerRef;

    // Start is called before the first frame update
    void Start()
    {
        _gameTimerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        _gameTimerRef.NextTurnTimer();
    }
}
