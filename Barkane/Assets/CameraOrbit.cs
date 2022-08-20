using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 4.0f;
    [SerializeField] private float ScrollSenstivity = 0.2f;
    [SerializeField] private float orbitDampen = 10.0f;
    [SerializeField] private float scrollDampen = 6.0f;
    [SerializeField] private float minCameraDistance = 1.5f;
    [SerializeField] private float maxCameraDistance = 10.0f;

    [SerializeField] private bool cameraDisabled = true;

    private Transform cameraTransform;
    private Transform cameraParent;
    private Vector3 localRoatation;
    private float cameraDistance = 5.0f;
    private Vector2 prevMousePosition;

    void Start()
    {
        cameraTransform = this.transform;
        cameraParent = this.transform.parent;
    }

    //We want the camera to move after everything else
    private void LateUpdate() 
    {
        if(!cameraDisabled)
        {
            Vector2 diff = prevMousePosition - Mouse.current.position.ReadValue();
            localRoatation.x += diff.x * mouseSensitivity * -1; //for some reason the x axis is inverted but the y axis is not
            localRoatation.y += diff.y * mouseSensitivity;
            localRoatation.y = Mathf.Clamp(localRoatation.y, 20f, 80f);
        }

        float scrollAmount = Mouse.current.scroll.ReadValue().y * ScrollSenstivity * 0.01f * cameraDistance;
        cameraDistance -= scrollAmount;
        cameraDistance = Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance);

        Quaternion quaternion = Quaternion.Euler(localRoatation.y, localRoatation.x, 0);
        cameraParent.rotation = Quaternion.Lerp(cameraParent.rotation, quaternion, Time.deltaTime * orbitDampen);
        if(cameraTransform.localPosition.z != cameraDistance * -1)
            cameraTransform.localPosition = new Vector3(0, 0, Mathf.Lerp(cameraTransform.localPosition.z, cameraDistance * -1, Time.deltaTime * scrollDampen));
        
        prevMousePosition = Mouse.current.position.ReadValue();
    }

    private void OnClick(InputValue value)
    {
        Debug.Log("click");
        ToggleCamera(value.isPressed);
    }

    private void ToggleCamera(bool value)
    {
        cameraDisabled = !value;
        if(!cameraDisabled)
            prevMousePosition = Mouse.current.position.ReadValue();
    }
}