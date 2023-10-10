using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyQualitySetting : Singleton<MyQualitySetting>
{
    [SerializeField]
    TextMeshProUGUI vSyncText;

    // Start is called before the first frame update
    void Start()
    {
        vSyncText.text = QualitySettings.vSyncCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            if (QualitySettings.vSyncCount == 3) QualitySettings.vSyncCount = 0;
            else QualitySettings.vSyncCount++;
            vSyncText.text = QualitySettings.vSyncCount.ToString();
        }

        //if (Input.GetKeyDown(KeyCode.F)) Application.targetFrameRate = fps;

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Cursor.visible = false;
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    Cursor.visible = true;
        //    Cursor.lockState = CursorLockMode.None;
        //}
    }
}
