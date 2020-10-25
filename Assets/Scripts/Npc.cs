using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Pathfinding;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Npc : MonoBehaviour, ICharacter
{
    
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    public float Health { get; set; }
    public float maxHealth = 100;
    bool alive;

    private int timer = 3;
    private float time = 0;
    
    public delegate void NpcDelegate();
    public static event NpcDelegate NpcEaten;
    public int nutritionValue;

    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    //The point to move to
    public Transform target;
    private Animator _animator;
    private Seeker _seeker;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
    private static readonly int TakingDamage = Animator.StringToHash("TakingDamage");
    //The calculated path
    public Path path;

    public GameObject NPC;
    //The AI's speed per second
    public float speed = 2;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards
    private int _currentWaypoint = 0;

    public void Start ()
    {
        target = GameObject.Find("Lara").transform;
        _animator = NPC.GetComponent<Animator>();
        _seeker = GetComponent<Seeker>();
        Health = maxHealth;
        alive = true;

        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        
        
        //_seeker.StartPath( transform.position, target.position, OnPathComplete );
    }

    public void OnPathComplete ( Path p )
    {
        Debug.Log( "Yay, we got a path back. Did it have an error? " + p.error );
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            _currentWaypoint = 0;
        }
    }

    public void GetBitten(float damage)
    {
        if (alive)
        {
            TakeDamage(damage);
        }
        else getEaten();
    }

    public void TakeDamage(float damage)
    {
        if (alive)
        {
            StartCoroutine(playDamageEffects());
            Health -= damage;
            if (Health <= 0) Die();
        }
    }

    void getEaten()
    {
        Debug.Log("this dude got eaten");
        NpcEaten();
        Destroy(gameObject);
    }

    IEnumerator playDamageEffects()
    {
        _animator.SetBool(TakingDamage, true);
        //playParticleEffectBlood
        yield return new WaitForSeconds(0.2f);
        _animator.SetBool(TakingDamage, false);

    }

    void Die()
    {
        _animator.SetTrigger("Dead");
        alive = false;
        //GetComponent<Collider2D>().enabled = false;
    }

    public void FixedUpdate ()
    {
        if (!alive) return;
        
        time += Time.fixedDeltaTime;

        if (time > timer)
        {
            if (IsMovementRandom)
            {
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 1f);
            }

            time = 0;
        }


        var vector = new Vector2(x, y);
        Debug.Log(vector);

        if (x != 0 || y != 0)
            _animator.SetBool(IsWalking, true);
        else
            _animator.SetBool(IsWalking, false);

        if (x < 0)
            NPC.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (x > 0)
            NPC.transform.rotation = Quaternion.Euler(0, 0, 0);

        vector *= speed * Time.fixedDeltaTime;

        transform.Translate(vector);

        var targetDist = (target.position - transform.position).normalized;

        if (math.abs(targetDist.x) <= 0.3f && math.abs(targetDist.y) <= 0.3f)
        {
            IsMovementRandom = false;
            x = targetDist.x;
            y = targetDist.y;
            speed = 6;
        }
        // if (path == null)
        // {
        //     //We have no path to move after yet
        //     return;
        // }
        //
        // if (_currentWaypoint >= path.vectorPath.Count)
        // {
        //     _seeker.StartPath( transform.position, target.position, OnPathComplete );
        // }
        //
        // //Direction to the next waypoint
        // var dir = ( path.vectorPath[ _currentWaypoint ] - transform.position ).normalized;
        //
        // var x = dir.x;
        // var y = dir.y;
        //
        // if (x != 0 || y != 0)
        //     _animator.SetBool(IsWalking, true);
        // else
        //     _animator.SetBool(IsWalking, false);
        //
        // if (x<0)
        //     NPC.transform.rotation = Quaternion.Euler(0, 180, 0);
        // else if (x>0)
        //     NPC.transform.rotation = Quaternion.Euler(0, 0, 0);
        //
        //
        // dir *= speed * Time.fixedDeltaTime;
        //
        // transform.Translate( dir );
        //
        // //If we are, proceed to follow the next waypoint
        // if (Vector3.Distance( transform.position, path.vectorPath[ _currentWaypoint ] ) < nextWaypointDistance)
        // {
        //     _currentWaypoint++;
        // }
    }

    private float x, y;
    private bool IsMovementRandom = true;
}
