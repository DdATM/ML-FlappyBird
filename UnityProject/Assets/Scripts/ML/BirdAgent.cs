using MLAgents;
using UnityEngine;

public class BirdAgent : Agent
{
    private Animator _animator;
    private ResetParameters _resetParameters;

    private Rigidbody2D _rigidbody;

//追踪代号
    private int _tubeNum;
    public GameObject tube1;
    public GameObject tube2;

    public override void InitializeAgent()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        var academy = FindObjectOfType<Academy>();
        _resetParameters = academy.resetParameters;
    }

    public override void CollectObservations()
    {
        var localPosition = transform.localPosition;
        AddVectorObs(localPosition.y);
        if (_tubeNum == 1)
        {
            var position = tube1.transform.position;
            AddVectorObs(position.y);
            var xValue = position.x - localPosition.x;
            AddVectorObs(xValue);
            if (xValue < 0)
            {
                _tubeNum = 2;
                AddReward(1);
            }
        }
        else
        {
            var position = tube2.transform.position;
            AddVectorObs(position.y);
            var xValue = position.x - localPosition.x;
            AddVectorObs(xValue);
            if (xValue < 0)
            {
                _tubeNum = 1;
                AddReward(1);
            }
        }


        AddVectorObs(_rigidbody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction)
    {
        //输入为 离散一个 是否点击
        //判断为 是否撞到柱子，是 done Setreward -1；
        //否 通过柱子 +1 活着+0.1
     
        var movement = (int) vectorAction[0];
        if (movement == 1)
        {
            _rigidbody.velocity = new Vector2(0, 5f);
            _animator.SetTrigger("jump");
        }

        AddReward(0.01f);
    }

    public override void AgentReset()
    {
        transform.position = new Vector3(-0.77f, 0.44f, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -14));
        _rigidbody.velocity = Vector2.zero;
        _animator.enabled = true;
        _tubeNum = 1;
        SetResetParameters();
    }

    public override float[] Heuristic()
    {
        if (Input.GetKeyDown(KeyCode.Space)) return new float[] {1};
        return new float[] {0};
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        AddReward(-1);
        _animator.enabled = false;
        Done();
    }

    public void SetResetParameters()
    {
        tube1.GetComponent<TubeController>().Init();
        tube2.GetComponent<TubeController>().Init();
    }
}