using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyMove : MonoBehaviour
{
    //[SerializeField] private bool goLeft = true;
    [SerializeField] private float speed = 5f;
    [field: SerializeField] public float spawnY { get; private set; } = -2f;
    [field: SerializeField] public bool movingType { get; private set; } = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveSelf()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void EnemyFlipper(bool _facingRight)
    {

        if (_facingRight)
        {
            Vector3 newRotation = new(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
        }
        else
        {
            Vector3 newRotation = new(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}
