using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    [SerializeField]
    private float ParallexEffectCoeficient;
    private Transform cameraTransform;
    private Vector3 lastCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        cameraTransform.position += deltaMovement * ParallexEffectCoeficient;
        lastCameraPos = cameraTransform.position;
    }
}
