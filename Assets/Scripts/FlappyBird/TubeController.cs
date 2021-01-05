using System;
using UnityEngine;

public class TubeController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    public float speed;

  
    private void Awake()
    {
        GetComponent<Observer>().AddEventHandler("GameOver", OnGameOver);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        _rigidbody2D.velocity = new Vector2(speed, 0);
    }

    private void OnDisable()
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        _rigidbody2D.velocity = new Vector2(0, 0);
        Debug.Log("done");
       // Destroy(gameObject);
    }
}