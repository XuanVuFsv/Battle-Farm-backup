using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSystemUI : MonoBehaviour
{
    private static WeaponSystemUI instance;
    [SerializeField]
    TextMeshProUGUI currentAmmoText;
    [SerializeField]
    TextMeshProUGUI weponNameText;
    [SerializeField]
    TextMeshProUGUI deltaTime;

    private float pollingTime = 1f;
    private float time = 1f;
    private int frameCount = 0;

    void MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;
    }


    public static WeaponSystemUI Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        MakeInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        //SubscribeAll();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;

        if(time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            deltaTime.text = "FPS: " + frameRate.ToString();

            time -= pollingTime;
            frameCount = 0;
        }
    }

    //public override void Execute(IGameEvent gEvent, DataGroup dataGroup)
    //{
    //    MyDebug.Instance.Log($"Update {dataGroup} ammo");
    //    UpdateAmmo(dataGroup.GetDataGroup());
    //}

    public void UpdateAmmo(int currentAmmo, int remainingAmmo)
    {
        currentAmmoText.text = currentAmmo + "/" + remainingAmmo;
    }    
}
