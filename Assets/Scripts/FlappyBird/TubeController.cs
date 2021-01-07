using System;
using UnityEngine;

public class TubeController : MonoBehaviour
{ public float finishPosX;
    private Rigidbody2D _rigidbody2D;

    public float speed;

  
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.x <= finishPosX)
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            TubeFactory.TubeSavePool.Enqueue(o);
        }

    }

    private void OnEnable()
    {
        _rigidbody2D.velocity = new Vector2(speed, 0);
    }

    private void OnDisable()
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
    }

    public  void OnGameOver()
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
        GameObject o;
        (o = gameObject).SetActive(false);
        TubeFactory.TubeSavePool.Enqueue(o);
        //Destroy(gameObject);
    }
}