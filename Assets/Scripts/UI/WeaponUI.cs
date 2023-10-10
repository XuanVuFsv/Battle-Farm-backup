using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public TMP_Text weaponName;
    public RectTransform panel;
    
    // Update is called once per frame
    void Update()
    {
        //Make weapon UI on the ground can be seen by the player in every direction
        transform.LookAt(Camera.main.transform.position);
    }
}
