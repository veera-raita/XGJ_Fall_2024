using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] InputReader inputReader;
    public bool tutorialRunning { get; set; } = true;
    public bool gameOver { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Oopsie woopsie we made a fucky wucky and now there's two GameManagers!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGame()
    {
        spawner.overrideSpawner = true;
        spawner.spawning = false;
        inputReader.EnableDialogue();
        gameOver = true;

        GetComponent<UIManager>().GameOver();
        //activate a panel here with some game over message, slowly fade it
        //into a gruesome pic / description of what happened to our guy
    }
}
