using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public enum CheckBehavior
    {
        IsMoving,
        LightOn,
        LookBig,
        DidTurn,
    }

    public enum LoseBehavior
    {
        Flee,
        Fade,
        Pop
    }

    [SerializeField] private PlayerController playerController;
    private EnemyMove enemyMove = null;

    private float reactTime = 1f;
    private float succeedTime = 0.3f;
    private const float fadePopTime = 1f;
    private bool checkingBehavior = false;
    private bool playerSucceed = false;
    private bool goodEnd = false;
    private bool facingRight = true;
    public bool attacking { get; private set; } = false;
    public bool fleeing { get; private set; } = false;
    private CheckBehavior checkFor;
    private LoseBehavior loseBehavior;

    public void Setup(CheckBehavior _checkBehavior, LoseBehavior _loseBehavior, EnemyMove _enemyMove, float _reactTime, bool _facingRight)
    {
        checkFor = _checkBehavior;
        loseBehavior = _loseBehavior;
        enemyMove = _enemyMove;
        reactTime = _reactTime;
        facingRight = _facingRight;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            checkingBehavior = true;
            StartCoroutine(InputTimer());
        }
    }

    private void CallAttack()
    {
        GetComponent<IAttack>().Attack();
    }

    private void Flee()
    {
        if (loseBehavior == LoseBehavior.Flee)
        {
            fleeing = true;
        }
        else if (loseBehavior == LoseBehavior.Fade)
        {
            StartCoroutine(FadePop());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator FadePop()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        float alpha;

        float takenTime = 0;

        while (takenTime < fadePopTime)
        {
            alpha = Mathf.Lerp(1f, 0f, takenTime / fadePopTime);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator InputTimer()
    {
        float takenTime = 0;
        float succeededTime = 0;
        bool playerWasStill = false;
        bool playerTotalFail = false;
        bool playerTotalSucceed = false;

        while (takenTime < reactTime)
        {
            takenTime += Time.deltaTime;

            switch (checkFor)
            {
                case CheckBehavior.IsMoving:
                    if (playerController.moving)
                    {
                        playerSucceed = false;
                    }
                    else if (!playerController.moving)
                    {
                        playerSucceed = true;
                        playerWasStill = true;
                    }
                    else if (playerController.moving && playerWasStill)
                    {
                        playerSucceed = false;
                        playerTotalFail = true;
                        break;
                    }
                    break;
                case CheckBehavior.LookBig:
                    if (playerController.lookingBig)
                    {
                        succeededTime += Time.deltaTime;
                    }
                    else
                    {
                        succeededTime -= Time.deltaTime / 2;
                    }
                    break;
                case CheckBehavior.LightOn:
                    if (!playerController.lightOn)
                    {
                        succeededTime += Time.deltaTime;
                    }
                    else
                    {
                        succeededTime -= Time.deltaTime / 2;
                    }
                    break;
                case CheckBehavior.DidTurn:
                    if (playerController.turned)
                    {
                        playerSucceed = true;
                        playerTotalSucceed = true;
                    }
                    break;
                default:
                    break;
            }

            if (succeededTime > succeedTime || playerSucceed == true || playerTotalSucceed)
            {
                goodEnd = true;
            }

            if (playerTotalSucceed || playerTotalFail || succeededTime > succeedTime) break;

            yield return null;
        }

        if (goodEnd) Flee();
        else GetComponent<IAttack>().Attack();
    }
}
