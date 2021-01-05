using MLAgents;
using UnityEngine;

public class BirdAgent : Agent
{
    private ResetParameters _resetParameters;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private int _tubeNum=1;
    private bool _canOperate;
  

    public TubeFactory tubeFactory;

    public override void InitializeAgent()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _canOperate = true;
        var academy = FindObjectOfType<Academy>();
        _resetParameters = academy.resetParameters;
      
    }

    public override void CollectObservations()
    {
        Debug.Log(_tubeNum+"找寻的数字");
        var localPosition = transform.localPosition;
        AddVectorObs(localPosition.y);
       AddVectorObs(tubeFactory.tubeDir[_tubeNum].y);
       var distance = localPosition.x - tubeFactory.tubeDir[_tubeNum].x;
       AddVectorObs(distance);
        AddVectorObs(_rigidbody.velocity);
    }

    public override void AgentAction(float[] vectorAction)
    {
        //输入为 离散一个 是否点击
        //判断为 是否撞到柱子，是 done Setreward -1；
        //否 通过柱子 +1 活着+0.1
        var movement = (int)vectorAction[0];
        if (movement==1&&_canOperate)
        {
            _rigidbody.velocity = new Vector2(0, 5);
            _animator.SetTrigger("jump");
        }
        if (transform.localPosition.x - tubeFactory.tubeDir[_tubeNum].x>0)
        {
            AddReward(0.8f);
        }
        else
        {
            AddReward(0.02f);
        }

        if (!_canOperate)
        {
            Done();
            AddReward(-1f);
        }
      
    }

    public override void AgentReset()
    {
        transform.position=new Vector3(-0.77f,0.44f,0);
        transform.rotation=Quaternion.Euler(new Vector3(0,0,-14));
        _rigidbody.velocity=Vector2.zero;
        _animator.enabled = true;
        _canOperate = true;
        _tubeNum = 1;

    }

    public override float[] Heuristic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return new float[] { 1 };
        }
        return new float[] { 0 };
    }

   
    
    void OnTriggerExit2D(Collider2D col)
    {
        _tubeNum++;
        NotificationCenter.GetInstance().PostNotification("ScoreAdd");
    }


    void OnCollisionEnter2D(Collision2D col)
    {   
        _animator.enabled = false;
        _rigidbody.velocity = new Vector2(0, 0);
        NotificationCenter.GetInstance().PostNotification("GameOver");
        _canOperate = false;
      
        //  this.enabled = false;
    }
}