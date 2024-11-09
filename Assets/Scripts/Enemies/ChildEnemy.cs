using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChildEnemy : MonoBehaviour, IAttack
{
    private PlayerCheck playerCheck;
    private EnemyMove enemyMove;
    [SerializeField] float reactTime = 2f;
    public bool facingRight = false;
    private bool startFleeing = false;
    private bool fleeing = false;
    private Animator animator;
    private AnimationClip sobAnimation;
    private AnimationClip attackAnimation;
    private AnimationClip fleeAnimation;

    // Start is called before the first frame update
    void Start()
    {
        playerCheck = GetComponent<PlayerCheck>();
        enemyMove = GetComponent<EnemyMove>();
        playerCheck.Setup(PlayerCheck.CheckBehavior.DidTurn, PlayerCheck.LoseBehavior.Pop, enemyMove, reactTime, facingRight);
        animator.Play(sobAnimation.name);
    }

    public void Attack()
    {
        animator.Play(attackAnimation.name);
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(attackAnimation.length);
        GameManager.instance.EndGame();
    }

    private void Flee()
    {
        enemyMove.MoveSelf();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.fleeing)
        {
            startFleeing = true;
        }

        if (startFleeing)
        {
            animator.Play(fleeAnimation.name);
            startFleeing = false;
            fleeing = true;
            if (!facingRight)
            {
                enemyMove.EnemyFlipper(facingRight);
                facingRight = true;
            }
        }

        if (fleeing) Flee();
    }
}
