using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Transform followTarget;
    public Camera mainCamera;
    //public Cinemachine.AxisState xAxis;
    //public Cinemachine.AxisState yAxis;
    //public Vector3 turnSpeed;
    //public float cameraSpeed = 1;
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
        //xAxis.Update(Time.fixedDeltaTime);
        //yAxis.Update(Time.fixedDeltaTime);

        //followTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0) * cameraSpeed;

        float yAngle = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yAngle, 0), Time.deltaTime * speedRotateBody);

        if (Input.GetKeyDown(KeyCode.F)) Application.targetFrameRate = fps;

        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}