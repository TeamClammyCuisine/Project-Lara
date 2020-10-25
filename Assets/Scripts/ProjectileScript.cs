using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float velocity;
    public float damage;
    public int duration = 3;


    bool flying = true;

    Vector3 directionOfFlight;

    Transform _transform;
    Animator _animator;
    Rigidbody2D _rigidBody;


    // Start is called before the first frame update

    void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void SetDirection(string direction)
    {
        switch (direction)
        {
            case "up":
                directionOfFlight.x = 0;
                directionOfFlight.y = velocity;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                Debug.Log("up");
                break;
            case "down":
                directionOfFlight.x = 0;
                directionOfFlight.y = -velocity;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case "right":
                directionOfFlight.x = velocity;
                directionOfFlight.y = 0;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case "left":
                directionOfFlight.x = -velocity;
                directionOfFlight.y = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(TimeOut());
        if (flying)
        {
            _rigidBody.velocity = directionOfFlight;
        }
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            flying = false;
            _rigidBody.velocity = Vector3.zero;
            var NpcController = col.gameObject.GetComponent<Npc>();
            NpcController.TakeVenomDamage(damage);
            _animator.SetTrigger("Hit");
        }
        if (col.gameObject.tag == "Meltable")
        {
            flying = false;
            _rigidBody.velocity = Vector3.zero;
            _animator.SetTrigger("Hit");
            Destroy(col.gameObject);
        }
    }
}
