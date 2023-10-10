using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Using Cinemachine Camera instead
public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    public bool lockFps = false;
    public int fps;
    public float speedRotateBody;

    // Start is called before the first frame update
    void Start()
    {
        //Set up cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(lockFps) Application.targetFrameRate = fps;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yAngle = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yAngle, 0), Time.deltaTime * speedRotateBody);
    }
}