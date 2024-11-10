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
    private int recordDist;
    public int currentDist { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //gameOver = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Oopsie woopsie we made a fucky wucky and now there's two GameManagers!");
        }
        inputReader.EnableDialogue();
        recordDist = PlayerPrefs.GetInt("recordDist", 0);
        UIManager.instance.UpdateRecordDist(recordDist);
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
        if (PlayerPrefs.GetInt("recordDist", recordDist) < currentDist)
        {
            PlayerPrefs.SetInt("recordDist", currentDist);
        }
    }
}
