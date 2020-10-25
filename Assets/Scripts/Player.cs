using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HealthBarScript))]
[RequireComponent(typeof(HungerBarScript))]
[RequireComponent(typeof(VenomBarScript))]
public class Player : MonoBehaviour, ICharacter
{
    private Animator _animator;
    private Animator biteAnimator;
    private Controllers _controls;
    private Transform _transform;

    private static readonly int MovingRight = Animator.StringToHash("MovingRight");
    private static readonly int MovingUp = Animator.StringToHash("MovingUp");
    private static readonly int MovingDown = Animator.StringToHash("MovingDown");
    private static readonly int TakingDamage = Animator.StringToHash("TakingDamage");
    private static readonly int Died = Animator.StringToHash("Died");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int BiteEffect = Animator.StringToHash("BiteEffect");

    public delegate void PlayerDelegate();
    public static event PlayerDelegate LaraDied;

    bool canSpit = false;

    public float Speed { get; set; }

    public float Health { get; set; }
    [SerializeField]
    public int maxHealth;

    public int Hunger { get; set; }
    [SerializeField]
    public int maxHunger;
    public int hungerSpeed;

    public int Venom { get; set; }
    [SerializeField]
    public int maxVenom;
    public GameObject venomProjectile;
    public GameObject spitPoint;
    string facing;
    bool spitting = false;

    bool alive;

    public float AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float attackRange = 0.5f;
    public GameObject BiteAttack;
    bool biting = false;
    public float biteSpeed = 0.4f;

    public LayerMask enemyLayers;

    //UI
    public HealthBarScript HealthBar;
    public HungerBarScript HungerBar;
    public VenomBarScript VenomBar;

    
    private void Awake()
    {
        alive = true;
        _controls = new Controllers();
        Speed = 1;

        _transform = GetComponent<Transform>();
        _animator = GetComponentInChildren<Animator>();
        biteAnimator = BiteAttack.GetComponent<Animator>();

        AttackDamage = 50;

        Health = maxHealth;
        HealthBar.SetMaxHealth(maxHealth);
        HealthBar.SetHealth(maxHealth);

        Hunger = maxHunger;
        HungerBar.SetMaxHunger(maxHunger);
        HungerBar.SetHunger(maxHunger);
        StartCoroutine("HungerGrowing");

        Venom = maxVenom;
        VenomBar.SetMaxVenom(maxVenom);
        VenomBar.SetVenom(maxVenom);
        facing = "down";

    }

    void Attack()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(BiteAttack.transform.position,attackRange,enemyLayers);
        StartCoroutine(PlayBiteEffect());
        foreach (Collider2D enemyHit in enemiesHit)
        {
            enemyHit.GetComponent<Npc>().GetBitten(AttackDamage);
        }
        biteAnimator.SetBool(BiteEffect, false);
    }

    IEnumerator PlayBiteEffect()
    {
        biting = true;
        BiteAttack.SetActive(true);
        _animator.SetBool(Attacking, true);
        biteAnimator.SetBool(BiteEffect, true);
        yield return new WaitForSeconds(0.3f);
        biteAnimator.SetBool(BiteEffect, false);
        BiteAttack.SetActive(false);
        yield return new WaitForSeconds(biteSpeed);
        _animator.SetBool(Attacking, false);
        biting = false;
    }

    IEnumerator SpitAttack()
    {
        if (Venom >= 1)
        {
            var projectile = Instantiate(venomProjectile, spitPoint.transform.position, Quaternion.identity);
            spitting = true;
            _animator.SetBool(Attacking, true);
            Venom--;
            VenomBar.SetVenom(Venom);
            projectile.GetComponent<ProjectileScript>().SetDirection(facing);
            yield return new WaitForSeconds(biteSpeed);
            _animator.SetBool(Attacking, false);
            spitting = false;
        }
    }

    //this is used to see where the bite attack will hit
    void OnDrawGizmosSelected()
    {
        if (BiteAttack == null) return;
        Gizmos.DrawWireSphere(BiteAttack.transform.position, attackRange);
    }

    private void Update()
    {
        if (alive)
        {
            var direction = _controls.Player.Movement.ReadValue<Vector2>();
            var attacked = Convert.ToBoolean(_controls.Player.Atack.ReadValue<float>());
            var spit = Convert.ToBoolean(_controls.Player.Spit.ReadValue<float>());

            if (spit && !spitting && canSpit) StartCoroutine(SpitAttack());
            if (attacked && !biting) Attack();

            var x = direction.x;
            var y = direction.y;


            if (attacked) return;

            if (x != 0)
            {
                _animator.SetBool(MovingRight, true);
            }
            else
            {
                _animator.SetBool(MovingRight, false);
            }

            if (y < 0)
            {
                _animator.SetBool(MovingDown, true);
                facing = "down";
                spitPoint.transform.position = _transform.position;
                BiteAttack.transform.position = _transform.position + new Vector3(0,-0.5f - attackRange);
            }
            else if (y > 0)
            {
                _animator.SetBool(MovingUp, true);
                facing = "up";
                spitPoint.transform.position = _transform.position;
                BiteAttack.transform.position = _transform.position + new Vector3(0,0.5f + attackRange);
            }
            else
            {
                _animator.SetBool(MovingUp, false);
                _animator.SetBool(MovingDown, false);

            }



            if (x < 0)
            {
                BiteAttack.transform.position = _transform.position + new Vector3(-0.3f, 0.273f,0);
                BiteAttack.transform.position = _transform.position + new Vector3(-0.4f - attackRange, 0); ;
                facing = "left";
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (x > 0)
            {
                spitPoint.transform.position = _transform.position + new Vector3(0.3f, 0.273f, 0);
                BiteAttack.transform.position = _transform.position + new Vector3(0.4f + attackRange, 0);
                facing = "right";
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            x *= Time.deltaTime * Speed;
            y *= Time.deltaTime * Speed;

            transform.Translate(Math.Abs(x), y, 0);
        }
        
    }

    private void OnEnable()
    {
        Npc.NpcEaten += onNpcEaten;
        GameManager.UnlockLaraVenom += onUnlockVenom;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
        GameManager.UnlockLaraVenom += onUnlockVenom;
        Npc.NpcEaten -= onNpcEaten;
    }

    void onUnlockVenom()
    {
        canSpit = true;
    }

    void onNpcEaten()
    {
        if (alive)
        {
            //_animator.SetBool(Eating, false);
            Hunger += 50;
            Venom = maxVenom;
            if (Hunger > maxHunger) Hunger = maxHunger;
            HungerBar.SetHunger(Hunger);
            VenomBar.SetVenom(Venom);
        }
    }

    public void Die()
    {
        _animator.SetBool(Died, true);
        alive = false;
        LaraDied();
    }

    public void TakeDamage(float damage)
    {
        if (alive)
        {
            _animator.SetBool(TakingDamage, false);
            Health -= damage;
            HealthBar.SetHealth(Health);
            if (Health <= 0) Die();
        }
    }

    IEnumerator HungerGrowing()
    {
        while (true)
        {
            if (Hunger >= 0)
            {
                Hunger -= hungerSpeed;
                HungerBar.SetHunger(Hunger);
            }
            else TakeDamage(hungerSpeed * 2);
            yield return new WaitForSeconds(1f);
        }
    }
}
 
