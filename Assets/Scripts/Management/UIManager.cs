using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private const float maxTypeTime = 1f;
    [SerializeField] private float typeSpeed = 16f;
    private bool isTyping = false;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject continuePrompt;
    private Queue<string> paragraphs = new Queue<string>();
    private string p;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        paragraphs.Enqueue("Hells...");
        paragraphs.Enqueue("Never should have agreed to this...");
        paragraphs.Enqueue("But what choice did I have? He double dog dared me!");
        paragraphs.Enqueue("All I can do now is move forward... [D]");
        paragraphs.Enqueue("But I could always look behind me if I feel like something's there [A]");
        paragraphs.Enqueue("Or try to look big if I feel like something might fear me more than I fear it [W]");
        paragraphs.Enqueue("Or maybe the dark might be to my advantage..? [Q]");
        DisplayNextParagraph();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        inputReader.NextDialogueEvent += DialogueHandle;
        inputReader.SkipDialogueEvent += SkipHandle;
        inputReader.RestartEvent += Restart;
        inputReader.QuitEvent += QuitGame;
    }

    public void GameOver()
    {
        tutorialPanel.SetActive(true);
        paragraphs.Enqueue("...");
        paragraphs.Enqueue("...Mikey?");
        paragraphs.Enqueue("I know I shouldn't have sent you out here alone! I'm sorry!");
        paragraphs.Enqueue("Mikey?! Answer me!");
        paragraphs.Enqueue("What is... Fuck! Mikey!");
        paragraphs.Enqueue("[Mikey's mangled corpse lies on the cave floor, covered in blood]");
        paragraphs.Enqueue("I have to get out of here, or I might be next!");
        paragraphs.Enqueue("Where the hell even is the exit?!");

        if (playerController.lightOn)
        {
            paragraphs.Enqueue("I should grab his flashlight or I'll neve- [SPLAT]");
        }
        else
        {
            paragraphs.Enqueue("His flashlight should still be somewh- [SPLAT]");
        }

        paragraphs.Enqueue("...");
        paragraphs.Enqueue("Game Over\nPress [R] to restart or [Esc] to quit");

        DisplayNextParagraph();
    }

    private void DialogueHandle()
    {
        DisplayNextParagraph();
    }

    private void SkipHandle()
    {
        paragraphs.Clear();
        if (GameManager.instance.gameOver)
        {
            paragraphs.Enqueue("Game Over\nPress [R] to restart or [Esc] to quit");
        }
        DisplayNextParagraph();
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void DisplayNextParagraph()
    {
        continuePrompt.SetActive(false);
        if (paragraphs.Count == 0)
        {
            if (GameManager.instance.tutorialRunning)
            {
                GameManager.instance.tutorialRunning = false;
                tutorialPanel.SetActive(false);
                inputReader.EnableMovement();
            }
            return;
        }

        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            StartCoroutine(TypeDialogueText(p));
        }

        displayText.text = p;
    }

    private IEnumerator TypeDialogueText(string _p)
    {
        isTyping = true;
        int maxVisibleChars = 0;

        displayText.text = _p;
        displayText.maxVisibleCharacters = maxVisibleChars;

        foreach (char c in _p.ToCharArray())

        {
            maxVisibleChars++;
            displayText.maxVisibleCharacters = maxVisibleChars;
            yield return new WaitForSeconds(maxTypeTime / typeSpeed);
        }
        isTyping = false;
        if (paragraphs.Count == 0 && GameManager.instance.gameOver)
        {
            Debug.Log("last dialogue");
            inputReader.SetGameOver();
        }
        else
        {
            continuePrompt.SetActive(true);
        }
    }
}
