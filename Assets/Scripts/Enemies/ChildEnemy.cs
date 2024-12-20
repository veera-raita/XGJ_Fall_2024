using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildEnemy : MonoBehaviour, IAttack
{
    private PlayerController playerController;
    private PlayerCheck playerCheck;
    [SerializeField] private float reactTime = 2f;
    private const float dashTime = 0.3f;
    public bool facingRight = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite ready;
    [SerializeField] private Sprite attack;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCheck = GetComponent<PlayerCheck>();
        playerCheck.Setup(PlayerCheck.CheckBehavior.LookBig, PlayerCheck.LoseBehavior.Fade, reactTime, facingRight);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        spriteRenderer.sprite = attack;
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        float takenTime = 0f;

        float moveAt = playerController.transform.position.x;
        float moveFrom = transform.position.x;
        Vector2 newPos = transform.position;

        while (takenTime < dashTime)
        {
            takenTime += Time.deltaTime;

            newPos.x = Mathf.Lerp(moveFrom, moveAt, takenTime / dashTime);
            transform.position = newPos;
            yield return null;
        }


        GameManager.instance.EndGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteRenderer.sprite = ready;
        }
    }
}
