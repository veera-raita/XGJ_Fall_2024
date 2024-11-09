using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoinkerEnemy : MonoBehaviour, IAttack
{
    [SerializeField] private PlayerController playerController;
    private PlayerCheck playerCheck;
    private EnemyMove enemyMove;
    [SerializeField] private float reactTime = 1f;
    private const float offset = 0.5f;
    private const float endPosition = -0.9f;
    private const float yoinkTime = 0.7f;
    public bool facingRight = true;
    private const float rotateAmount = 45f;
    private bool attacking = false;
    [SerializeField] InputReader inputReader;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCheck = GetComponent<PlayerCheck>();
        enemyMove = GetComponent<EnemyMove>();
        playerCheck.Setup(PlayerCheck.CheckBehavior.DidTurn, PlayerCheck.LoseBehavior.Pop, enemyMove, reactTime, facingRight);
        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update()
    {
        //don't call MoveSelf() from EnemyMove since this is a more specialized enemy
        if (!attacking)
        transform.position = new Vector2(playerController.transform.position.x + offset, playerController.transform.position.y);
    }

    public void Attack()
    {
        attacking = true;
        StartCoroutine(AttackAnim());
        Debug.Log("attacked!");
        inputReader.DisableMovement();
    }

    private IEnumerator AttackAnim()
    {
        float takenTime = 0;
        Vector2 newPos = transform.position;
        float initialX = newPos.x;

        while (takenTime < yoinkTime)
        {
            takenTime += Time.deltaTime;
            newPos.x = Mathf.Lerp(initialX, endPosition, takenTime / yoinkTime);
            transform.position = newPos;
            yield return null;
        }
        GameManager.instance.EndGame();
    }

    private IEnumerator Animate()
    {
        float takenTime = 0f;
        Vector3 rotation = transform.eulerAngles;
        float initialZ = rotation.z;
        float finalZ = initialZ - rotateAmount;

        while (takenTime < reactTime)
        {
            takenTime += Time.deltaTime;

            //do stuff here (maybe rotate sprite?) for visuals
            rotation.z = Mathf.Lerp(initialZ, finalZ, takenTime / reactTime);
            transform.eulerAngles = rotation;

            yield return null;
        }
    }
}
