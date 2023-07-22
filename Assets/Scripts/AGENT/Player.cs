using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Player Variables
    [HideInInspector] public bool _isTurn = false;
    [SerializeField] private float _playerSpeed = 3f;
    private Rigidbody2D _rigidBody;

    //Timer ref
    private GameTimer _timerRef;

    //Pause ref
    private bool _isPaused = false;

    //Bullet Trajectory Ref


    // Start is called before the first frame update
    void Start()
    {
        _timerRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameTimer>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTurn && !_isPaused)
        {
            ToggleWeapons(true);

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                //convert user input into world movement
                float horizontalMovement = Input.GetAxisRaw("Horizontal");

                //assign movement to a vector3
                Vector3 movementDirection = new Vector3(horizontalMovement, 0, 0).normalized;

                //apply movement to player
                gameObject.transform.Translate(movementDirection * _playerSpeed * Time.deltaTime);

                //Update the facing direction of the player
                if (Input.GetAxisRaw("Horizontal") == 1)
                {
                    FaceDirection(Vector3.right);
                }
                else if (Input.GetAxisRaw("Horizontal") == -1)
                {
                    FaceDirection(-Vector3.right);
                }
            }
            //JUMP
            if (Input.GetKeyDown(KeyCode.W))
            {
                _rigidBody.AddForce(new Vector2(0, 300f));
            }
        }
        else
        {
            ToggleWeapons(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _timerRef.NextTurnTimer();
        }
    }

    //Update sprite face direction
    private void FaceDirection(Vector3 direction)
    {
        GetComponent<SpriteRenderer>().flipX = direction != Vector3.right;
    }

    public void ToggleWeapons(bool toggle)
    {
        transform.GetChild(0).gameObject.SetActive(toggle);
    }

    public void Pause(bool isPaused)
    {
        _isPaused = isPaused;
    }
}
