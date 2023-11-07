using Frictionless;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform cameraTransform;
    private float shakeDuration = 0f;
    private float shakeIntensity = 0.1f;
    private Vector3 StartPosition;
    public Vector3 PortraitCameraOffset;
    
    void Awake()
    {
        StartPosition = transform.localPosition;
        ServiceFactory.RegisterSingleton(this);
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
    }

    void Update()
    {
        Vector3 startPosition = Vector3.zero;
        if (Screen.width < Screen.height)
        {
            startPosition = StartPosition + PortraitCameraOffset;
        }
        else
        {
            startPosition = StartPosition;
        }
        
        if (shakeDuration > 0)
        {
            cameraTransform.localPosition = startPosition + Random.insideUnitSphere * shakeIntensity;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            cameraTransform.localPosition = startPosition;
        }
    }

    // Call this method to start the camera shake with a specified duration and intensity.
    public void Shake(float duration, float intensity)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
    }
}