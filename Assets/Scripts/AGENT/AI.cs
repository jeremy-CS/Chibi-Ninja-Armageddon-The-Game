using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    //Player References
    private GameObject[] _players;
    private GameObject _target;

    //Turn reference
    [HideInInspector] public bool _isTurn = false;
    private bool _waiting = true;
    private float _timer = 0;
    private float _waitTime = 4f;

    //Attack variables
    [SerializeField] private Weapons _weapon;

    // Start is called before the first frame update
    void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTurn)
        {
            if (_waiting)
            {                
                ToggleWeapons(true);
                _timer += Time.deltaTime;
                if (_timer > _waitTime)
                {
                    _timer = 0f;
                    _waiting = false;
                }
            }
            else
            {

                UpdatePlayers();

                if (_players != null)
                {
                    _target = GetNearestPlayer();

                    if (_target != null)
                        AttackPlayer(_target);

                    _isTurn = false;
                    _waiting = true;
                }
            }
            
        }
        else
        {
            ToggleWeapons(false);
        }
    }

    public void UpdatePlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); ;
        _players = new GameObject[players.Length];
        _players = players;
    }

    public GameObject GetNearestPlayer()
    {
        GameObject target = _players[0];

        foreach (GameObject player in _players)
        {
            if (Vector2.Distance(target.transform.position, transform.position) > Vector2.Distance(player.transform.position, transform.position))
            {
                target = player;
            }
        }

        return target;
    }

    public void AttackPlayer(GameObject target)
    {
        int weaponIndex = Random.Range(0, 3);
        if (weaponIndex == 2) weaponIndex = 1;
        if (_weapon != null)
        {
            _weapon.SetAI(weaponIndex, target.transform);
            _weapon.AIAttack();
        }
        else Debug.Log("Cant shoot");
    }

    public void ToggleWeapons(bool toggle)
    {
        transform.GetChild(0).gameObject.SetActive(toggle);
    }
}
