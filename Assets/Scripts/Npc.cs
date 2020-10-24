using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Pathfinding;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Npc : MonoBehaviour, ICharacter
{
    
    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    public float Health { get; set; }
    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    //The point to move to
    public Transform target;
    private Animator _animator;
    private Seeker _seeker;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
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
        _animator = NPC.GetComponent<Animator>();
        _seeker = GetComponent<Seeker>();

        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        
        
        _seeker.StartPath( transform.position, target.position, OnPathComplete );
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

    public void FixedUpdate ()
    {
        
        if (path == null)
        {
            //We have no path to move after yet
            return;
        }

        if (_currentWaypoint >= path.vectorPath.Count)
        {
            _seeker.StartPath( transform.position, target.position, OnPathComplete );
        }

        //Direction to the next waypoint
        var dir = ( path.vectorPath[ _currentWaypoint ] - transform.position ).normalized;
        
        var x = dir.x;
        var y = dir.y;

        if (x != 0 || y != 0)
            _animator.SetBool(IsWalking, true);
        else
            _animator.SetBool(IsWalking, false);
        //
        if (x<0)
            NPC.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (x>0)
            NPC.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        
        Debug.Log(dir);
        dir *= speed * Time.fixedDeltaTime;
        
        transform.Translate( dir );
        
        // x *= Time.fixedDeltaTime * speed;
        // y *= Time.fixedDeltaTime * speed; 
        //             
        // transform.Translate(x,y,0);
      

        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance( transform.position, path.vectorPath[ _currentWaypoint ] ) < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }


}
