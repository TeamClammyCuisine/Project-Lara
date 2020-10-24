using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, ICharacter
{
    private Animator _animator;
    private Controllers _controls;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    public float Speed { get; set; }
    public float Health { get; set; }
    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    
    private void Awake()
    { 
        _controls = new Controllers();
        Speed = 1;
        _animator = GetComponent<Animator>();
    }

    private void Attack()
    {
        Debug.Log("Attack Performed");
        _animator.SetBool(IsAttacking,true);
    }

    private void Update()
    { 
        var direction = _controls.Player.Movement.ReadValue<Vector2>();
        var attacked = Convert.ToBoolean(_controls.Player.Atack.ReadValue<float>());
        _animator.SetBool(IsAttacking, attacked);
        
        var x = direction.x;
        var y = direction.y;


        if (attacked) return;
        
        if (x != 0 || y != 0)
            _animator.SetBool(IsWalking, true);
        else
            _animator.SetBool(IsWalking, false);
            
        if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }else if (x > 0)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
            
        x *= Time.deltaTime * Speed;
        y *= Time.deltaTime * Speed; 
                    
        transform.Translate(Math.Abs(x),y,0);


    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

 
}
