using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public GameObject[] wallList;
    public int[] activeIndex = new int[3];
    public int destroyedWall = 0;
    private static WallSpawner instance;

    void MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else instance = this;
    }

    public static WallSpawner Instance
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
        wallList = new GameObject[transform.childCount];

        int i = 0;
        foreach (Transform wall in transform)
        {
            wallList[i] = wall.gameObject;
            wall.GetComponent<WallBehaviour>().index = i;
            i++;
        }

        Invoke("SpawnWall", 1);
    }

    public void SpawnWall()
    {
        //Debug.Log(activeIndex.Length);
        for (int i = 0; i < activeIndex.Length; i++)
        {
            //Debug.Log(i);
            activeIndex[i] = GetNewSpawnedWallIndex();
            wallList[activeIndex[i]].SetActive(true);
            wallList[activeIndex[i]].GetComponent<WallBehaviour>().index = i;
            //Debug.Log(" " + activeIndex[i]);
        }
    }

    public int GetNewSpawnedWallIndex()
    {
        int newIndexValue = Random.Range(0, wallList.Length -1);
        foreach (int _index in activeIndex)
        {
            if (newIndexValue == _index)
            {
                newIndexValue = GetNewSpawnedWallIndex();
            }
        }
        return newIndexValue;
    }

    public void DestroyWall(int wallActiveIndex)
    {
        destroyedWall++;

        wallList[activeIndex[wallActiveIndex]].GetComponent<WallBehaviour>().index = 10;
        wallList[activeIndex[wallActiveIndex]].SetActive(false);

        int newIndex = GetNewSpawnedWallIndex();
        activeIndex[wallActiveIndex] = newIndex;
        wallList[newIndex].SetActive(true);
        wallList[newIndex].GetComponent<WallBehaviour>().index = wallActiveIndex;
    }    
}
