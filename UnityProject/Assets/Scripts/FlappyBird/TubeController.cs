using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TubeController : MonoBehaviour

{
    public float startX;
    private Rigidbody2D _rigidbody2D;


   
    public void Init()
    {
    
        Random.InitState(gameObject.GetInstanceID());
        _rigidbody2D = GetComponent<Rigidbody2D>();
        var posY = ChangeY();
        var createPos = new Vector3(startX, posY, transform.position.z); 
       transform.position = createPos;
        _rigidbody2D.velocity = new Vector2( -1.2f,0);
    }

    private void Update()
    {
       
        if (transform.position.x <= -4.8f)
        {  
            var posY = ChangeY();
            var createPos = new Vector3(0, posY, transform.position.z); 
          
            transform.position = createPos;
        }
    }

    private float ChangeY()
    {
        return Random.Range(-1.2f, 1.2f);
    }

    public void OnGameOver()
    {
        
    }
}