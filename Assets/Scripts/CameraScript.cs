using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private CustomKeys inputKeys;

    [SerializeField] private string mouseXInputname, mouseYInputName;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    private void Awake()
    {
        CursorLockState(CursorLockMode.Locked);
        xAxisClamp = 0.0f;
    }
    private void CursorLockState(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();

        UpdateLockState();
    }

    private void UpdateLockState()
    {
        if (Input.GetKeyDown(inputKeys.FindInput("menuKey")))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                CursorLockState(CursorLockMode.None);
            else if (Cursor.lockState == CursorLockMode.None)
                CursorLockState(CursorLockMode.Locked);
        }
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputname) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotaionToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotaionToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotaionToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
