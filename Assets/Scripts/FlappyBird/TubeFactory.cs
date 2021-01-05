using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TubeFactory : MonoBehaviour
{
    public float finishPosX;
    public float maxCreatePosY;
    public float minCreatePosY;
  
    public GameObject tube;

    public Queue<GameObject> tubePool;
    public float waitTime;

    private int _tubeNum=0;
    public Dictionary<int,Vector3>tubeDir=new Dictionary<int, Vector3>();
    private void Start()
    {
        Random.InitState(DateTime.Today.Millisecond);
        tubePool = new Queue<GameObject>();
        GetComponent<Observer>().AddEventHandler("GameOver", OnGameOver);

        StartCoroutine(CreateTube());
    }

    private IEnumerator CreateTube()
    {
      

        var posY = Random.Range(minCreatePosY, maxCreatePosY);
        var createPos = new Vector3(0, posY, transform.position.z);
        var go = Dequeue();
        go.transform.position = createPos;
        go.SetActive(true);
        _tubeNum++;
        tubeDir.Add(_tubeNum,go.transform.position);
        Debug.Log(_tubeNum);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(CreateTube());

        while (go.transform.position.x >= finishPosX)
            yield return null;
      
        go.SetActive(false);
        Enqueue(go);
      
    }

    public void OnGameOver(object sender, EventArgs e)
    {
        StopAllCoroutines();
        tubeDir.Clear();
        _tubeNum=0;
    }

    private void Enqueue(GameObject tube)
    {
        tubePool.Enqueue(tube);
    }

    private GameObject Dequeue()
    {
        if (tubePool.Count > 0)
            return tubePool.Dequeue();
        return Instantiate(tube, Vector3.zero, Quaternion.identity) as GameObject;
    }
}