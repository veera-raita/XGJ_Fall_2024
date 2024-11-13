using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Input Bools")]
    private bool lightControlDisabled = false;
    public bool lightOn { get; private set; } = true;
    public bool moving { get; private set; } = false;
    public bool lookingBig { get; private set; } = false;
    public bool turned { get; private set; } = false;

    [Header("Const Values")]
    private const float speed = 1.5f;
    private const float lightLerpDuration = 5.0f;
    private Vector2 flashlightPosDefault = new(-0.132f, 0.252f);
    private Vector2 flashlightPosLookBig = new(-0.22f, 0.156f);

    [Header("Engine References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject flashlightHolder;
    [SerializeField] private SpriteMask flashlightMask;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip walk;
    [SerializeField] private AnimationClip lookBig;

    enum LastAction
    {
        None,
        Turn,
        LookBig
    }


    // Start is called before the first frame update
    void Start()
    {
        inputReader.MoveEvent += HandleMove;
        inputReader.MoveCanceledEvent += HandleMoveCanceled;
        inputReader.LookBigEvent += LookBig;
        inputReader.LookBigCanceledEvent += LookBigCanceled;
        inputReader.ToggleLightEvent += ToggleLight;
        inputReader.TurnLightEvent += FlipLight;
    }

    private void OnDestroy()
    {
        inputReader.MoveEvent -= HandleMove;
        inputReader.MoveCanceledEvent -= HandleMoveCanceled;
        inputReader.LookBigEvent -= LookBig;
        inputReader.LookBigCanceledEvent -= LookBigCanceled;
        inputReader.ToggleLightEvent -= ToggleLight;
        inputReader.TurnLightEvent -= FlipLight;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAction();
        GameManager.instance.currentDist = (int)transform.position.x;
        UIManager.instance.UpdateCurrentDist((int)transform.position.x);
    }

    private void MoveAction()
    {
        if (moving && ! turned) rb.velocity = new Vector2(speed, rb.velocity.y);
        else rb.velocity = Vector2.zero;
    }

    private void HandleMove()
    {
        moving = true;
        lookingBig = false;
        if (!turned)
        animator.Play(walk.name);
        flashlightHolder.transform.localPosition = flashlightPosDefault;
    }

    private void HandleMoveCanceled()
    {
        if (moving)
        animator.Play(idle.name);
        moving = false;
    }

    private void LookBig()
    {
        lookingBig = true;
        moving = false;
        animator.Play(lookBig.name);
        flashlightHolder.transform.localPosition = flashlightPosLookBig;
    }

    private void LookBigCanceled()
    {
        if (!lookingBig) return;
        lookingBig = false;
        animator.Play(idle.name);
        flashlightHolder.transform.localPosition = flashlightPosDefault;
    }

    private void ToggleLight()
    {
        lightOn = !lightOn;
        if (lightOn)
        {
            flashlight.GetComponent<Light2D>().intensity = 1;
            flashlightMask.enabled = true;
        }
        else
        {
            flashlight.GetComponent<Light2D>().intensity = 0;
            flashlightMask.enabled = false;
        }
    }

    private void FlipLight()
    {
        if (!lightControlDisabled)
        {
            lightControlDisabled = true;
            turned = true;
            StartCoroutine(LerpLight());
        }
    }

    private void MoveLightUp()
    {
        flashlightHolder.transform.position = new Vector2(flashlightHolder.transform.position.x, flashlightHolder.transform.position.y + 0.04f);
    }

    private void MoveLightDown()
    {

        flashlightHolder.transform.position = new Vector2(flashlightHolder.transform.position.x, flashlightHolder.transform.position.y - 0.04f);
    }

    public void PlayerFlipper()
    {

        if (turned)
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

    private IEnumerator LerpLight()
    {
        PlayerFlipper();
        animator.Play(idle.name);
        float takenTime = 0f;
        float targetYRotation = 0;
        float startYRotation = 180f;
        Vector3 currentRotation = flashlightHolder.transform.eulerAngles;
        bool hasFlipped = false;

        while (takenTime < lightLerpDuration)
        {
            takenTime += Time.deltaTime;
            currentRotation.y = Mathf.Lerp(startYRotation, targetYRotation, takenTime / lightLerpDuration);
            flashlightHolder.transform.eulerAngles = currentRotation;
            if (takenTime > lightLerpDuration / 2)
            {
                turned = false;
                if (!hasFlipped)
                {
                    PlayerFlipper();
                    hasFlipped = true;
                    if (moving) animator.Play(walk.name);
                }
            }
            yield return null;
        }
        lightControlDisabled = false;
    }
}
