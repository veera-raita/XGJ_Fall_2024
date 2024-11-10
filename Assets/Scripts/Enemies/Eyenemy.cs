using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Eyenemy : MonoBehaviour, IAttack
{
    private PlayerCheck playerCheck;
    [SerializeField] private float reactTime = 2f;
    private const float dashTime = 0.3f;
    public bool facingRight = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Light2D eyeLight;
    [SerializeField] private Sprite ready;

    // Start is called before the first frame update
    void Start()
    {
        playerCheck = GetComponent<PlayerCheck>();
        playerCheck.Setup(PlayerCheck.CheckBehavior.LightOn, PlayerCheck.LoseBehavior.Fade, reactTime, facingRight);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        StartCoroutine(Glow());
    }

    private IEnumerator Glow()
    {
        float takenTime = 0f;
        float newIntensity = 15f;
        float startIntensity = 0f;

        while (takenTime < dashTime)
        {
            takenTime += Time.deltaTime;

            eyeLight.intensity = Mathf.Lerp(startIntensity, newIntensity, takenTime / dashTime);
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
