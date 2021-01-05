using UnityEngine;
using System.Collections;
using System;

public class BirdController : MonoBehaviour {
    public float speed;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private void Start()
    {
      _rigidbody2D=  GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            _rigidbody2D.velocity = new Vector2(0, speed);
            _animator.SetTrigger("jump");
        }
	}

    void OnTriggerExit2D(Collider2D col) {
        NotificationCenter.GetInstance().PostNotification("ScoreAdd");
    }


    void OnCollisionEnter2D(Collision2D col)
    {
       _rigidbody2D.velocity = new Vector2(0, 0);
        NotificationCenter.GetInstance().PostNotification("GameOver");
        _animator.enabled = false;
        this.enabled = false;
    }

}



