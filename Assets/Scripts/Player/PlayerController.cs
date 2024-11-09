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

    [Header("Engine References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject flashlightHolder;
    [SerializeField] private SpriteMask flashlightMask;

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

    // Update is called once per frame
    void Update()
    {
        MoveAction();
    }

    private void MoveAction()
    {
        if (moving) rb.velocity = new Vector2(speed, rb.velocity.y);
        else rb.velocity = Vector2.zero;
    }

    private void HandleMove()
    {
        moving = true;
    }

    private void HandleMoveCanceled()
    {
        moving = false;
    }

    private void LookBig()
    {
        lookingBig = true;
    }

    private void LookBigCanceled()
    {
        lookingBig = false;
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

    private IEnumerator LerpLight()
    {
        float takenTime = 0f;
        float targetYRotation = 0;
        float startYRotation = 180f;
        Vector3 currentRotation = flashlightHolder.transform.eulerAngles;

        while (takenTime < lightLerpDuration)
        {
            takenTime += Time.deltaTime;
            currentRotation.y = Mathf.Lerp(startYRotation, targetYRotation, takenTime / lightLerpDuration);
            flashlightHolder.transform.eulerAngles = currentRotation;
            if (takenTime > lightLerpDuration / 2) turned = false;
            yield return null;
        }
        lightControlDisabled = false;
    }
}
