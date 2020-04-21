using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerController : MonoBehaviour
{
    [Header("行走速度")] public float walkSpeed;

    [Header("跑步速度")] public float runSpeed;

    [Header("打")] public KeyCode attackButton;
    [Header("跳")] public KeyCode jumpButton;
    [Header("格")] public KeyCode defenceButton;
    private bool _flipX;
    private bool _attack;
    private bool _run;
    private bool _runToLeft;
    private bool _runToRight;
    private float _keyDownInterval = 0.4f;
    private KeyCode _lastKeyDown;
    private float _lastKeyDownTime;
    private List<string> attackNameList = new List<string>() {"attackA", "attackB"};

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.flipX = _flipX;
        if (Input.GetKeyDown(attackButton))
            _attack = true;
        if (_attack)
        {
            if (_run)
                _animator.SetTrigger("runAttack");
            else
                _animator.SetTrigger(attackNameList[Random.Range(0, attackNameList.Count)]);

            _runToLeft = false;
            _runToRight = false;
            _attack = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _runToRight = false;
            if (_lastKeyDown.Equals(KeyCode.A) && Time.time - _lastKeyDownTime < _keyDownInterval)
            {
                _runToLeft = true;
            }
            else
            {
                _lastKeyDown = KeyCode.A;
                _lastKeyDownTime = Time.time;
                _runToLeft = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            _runToLeft = false;
            if (_lastKeyDown.Equals(KeyCode.D) && Time.time - _lastKeyDownTime < _keyDownInterval)
            {
                _runToRight = true;
            }
            else
            {
                _lastKeyDown = KeyCode.D;
                _lastKeyDownTime = Time.time;
                _runToRight = false;
            }
        }

        _run = _runToLeft || _runToRight;

        _animator.SetBool("run", _run);
    }

    private void FixedUpdate()
    {
        var _movement = Vector2.zero;
        _movement.x = Input.GetAxisRaw("Horizontal");
        if (_movement.x < 0)
            _flipX = true;

        if (_movement.x > 0)
            _flipX = false;
        _movement.y = Input.GetAxisRaw("Vertical");


        var speed = _run ? runSpeed : walkSpeed;
        transform.position = (Vector2) transform.position + _movement.normalized * (Time.fixedDeltaTime * speed);
        _animator.SetFloat("movementX", Mathf.Abs(_movement.x));
        _animator.SetFloat("movementY", Mathf.Abs(_movement.y));

        if (_movement.Equals(Vector2.zero))
        {
            _runToLeft = false;
            _runToRight = false;
        }
    }
}