using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //Pause variables
    public GameObject pauseUI;
    public GameObject endUI;
    [SerializeField] bool canPause;
    private GameObject[] _players;

    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.P))
                Pause();
        }
    }

    public void Pause()
    {
        //Pause game and unlock/show cursor
        Time.timeScale = 0;
        foreach (GameObject player in _players)
        {
            player.GetComponent<Player>().Pause(true);
        }

        pauseUI.SetActive(true);
    }

    public void Resume()
    {
        //Resume game and hide/lock cursor
        Time.timeScale = 1;
        foreach (GameObject player in _players)
        {
            player.GetComponent<Player>().Pause(false);
        }

        pauseUI.SetActive(false);
    }

    public void EndScreen()
    {
        pauseUI.SetActive(false);
        endUI.SetActive(true);
    }

    //Load the scene from the given name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Quit the app
    public void QuitApp()
    {
        Application.Quit();
    }
}
