using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TubeFactory : MonoBehaviour
{
    public static Queue<GameObject> TubeSavePool=new Queue<GameObject>();
    private Queue<GameObject>tubeActivePool=new Queue<GameObject>();
    private int _tubeNum;

    public float maxCreatePosY;
    public float minCreatePosY;

    public GameObject tube;
    public Dictionary<int, Vector3> tubeDir = new Dictionary<int, Vector3>();
    public float waitTime;
    
    public void Init()
    {
        Random.InitState(DateTime.Today.Millisecond);
        ExecuteCreate();
    }

    public void ExecuteCreate()
    {
        var posY = Random.Range(minCreatePosY, maxCreatePosY);
        var createPos = new Vector3(0, posY, transform.position.z);
        var go = Dequeue();
        go.transform.position = createPos;
        go.SetActive(true);
        _tubeNum++;
        tubeActivePool.Enqueue(go);
        tubeDir.Add(_tubeNum, go.transform.position);
        StartCoroutine(CreateTube());
    }

    private IEnumerator CreateTube()
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            var posY = Random.Range(minCreatePosY, maxCreatePosY);
            var createPos = new Vector3(0, posY, transform.position.z);
            var go = Dequeue();
            go.transform.position = createPos;
            go.SetActive(true);
            _tubeNum++;
            Debug.Log("加入字典的"+_tubeNum);
            tubeActivePool.Enqueue(go);
            tubeDir.Add(_tubeNum, go.transform.position);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void OnGameOver()
    {
        StopAllCoroutines();
        tubeDir.Clear();
       
        foreach (GameObject o in tubeActivePool)
        {
            o.GetComponent<TubeController>().OnGameOver();
        }
        //  TubePool.Clear();
        _tubeNum = 0;
    }

    public void Enqueue(GameObject tube)
    {
        tubeActivePool.Dequeue();
        TubeSavePool.Enqueue(tube);
    }

    private GameObject Dequeue()
    {
        if (TubeSavePool.Count > 0)
            return TubeSavePool.Dequeue();
        return Instantiate(tube, Vector3.zero, Quaternion.identity);
    }
}