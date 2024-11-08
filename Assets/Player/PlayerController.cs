using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Input Bools")]
    private bool lightControlDisabled = false;
    private bool lightOn = true;
    public bool moving { get; private set; } = false;
    private bool lookingBig = false;

    [Header("Const Values")]
    private const float speed = 3.0f;
    private const float lightLerpDuration = 5.0f;

    [Header("Dynamic Values")]
    private Coroutine lerpLight;

    [Header("Engine References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject flashlight;


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
        if (lightOn) flashlight.GetComponent<Light2D>().intensity = 1;
        else flashlight.GetComponent<Light2D>().intensity = 0;
    }

    private void FlipLight()
    {
        if (!lightControlDisabled)
        {
            Debug.Log("flipping light");
            lightControlDisabled = true;
            lerpLight = StartCoroutine(LerpLight());
        }
    }

    private IEnumerator LerpLight()
    {
        float takenTime = 0f;
        float targetRotation = 0;
        float currentRotation = 180;
        flashlight.transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotation, transform.rotation.z);

        while (takenTime < lightLerpDuration)
        {
            takenTime += Time.deltaTime;
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, takenTime / lightLerpDuration);
            flashlight.transform.rotation = Quaternion.Euler(transform.rotation.x, currentRotation, transform.rotation.z);
            yield return null;
        }
        lightControlDisabled = false;
    }
}
