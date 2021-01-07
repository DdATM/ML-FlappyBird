using MLAgents;
using UnityEngine;

public class BirdAgent : Agent
{
    private Animator _animator;
    private bool _canOperate;
    private ResetParameters _resetParameters;
    private Rigidbody2D _rigidbody;
    private int _tubeNum = 1;


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
        var localPosition = transform.localPosition;
        AddVectorObs(localPosition.y);
        Debug.Log("寻找的"+_tubeNum);
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
        var movement = (int) vectorAction[0];
        if (movement == 1)
        {
            _rigidbody.velocity = new Vector2(0, 5);
            _animator.SetTrigger("jump");
        }
        AddReward(0.01f);
    }

    public override void AgentReset()
    {
        
        Debug.Log("执行了特工重置");
        transform.position = new Vector3(-0.77f, 0.44f, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -14));
        _rigidbody.velocity = Vector2.zero;
        _animator.enabled = true;
        _tubeNum = 1;
        SetResetParameters();
    }

    public override float[] Heuristic()
    {
        if (Input.GetMouseButtonDown(0)) return new float[] {1};
        return new float[] {0};
    }


    private void OnTriggerExit2D(Collider2D col)
    {
        _tubeNum++;
        AddReward(1);
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        AddReward(-1);
        _animator.enabled = false;
        _rigidbody.velocity = new Vector2(0, 0);
       
       // BroadcastMessage("OnGameOver");
      
       tubeFactory.OnGameOver();
        Done();
    }
    public void SetResetParameters()
    {
       // 这里执行下 重置工厂的操作
       tubeFactory.Init();
    }
}