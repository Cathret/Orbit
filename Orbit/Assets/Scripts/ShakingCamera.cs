using System.Collections;
using System.Collections.Generic;
using Orbit.Entity;
using UnityEngine;

public class ShakingCamera : MonoBehaviour {

    // How long the object should shake for.
    [SerializeField]
    private float shakeDuration = 0.3f;

    private float shakeTimer = 0.0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    [SerializeField]
    private float shakeAmount = 0.7f;
    [SerializeField]
    private float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Start()
    {
        AUnitController.DmgTakenEvent.AddListener(Shake);
    }

    void Update()
    {
        if (shakeTimer < shakeDuration)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeTimer += Time.deltaTime * decreaseFactor;
        }
    }

    public void Shake()
    {
        originalPos = transform.localPosition;
        shakeTimer = 0.0f;
    }
}
