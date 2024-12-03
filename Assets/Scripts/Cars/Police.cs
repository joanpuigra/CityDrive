using System;
using UnityEngine;

public class Police : MonoBehaviour
{
    [Header("Player Interaction")]
    public Transform player;
    public LayerMask collisionLayer;

    [Header("Movement Parameters")]
    private float chaseRange = 10f;
    private float speed = 4f;

    [Header("Health Parameters")]
    public float health = 100f;
    private Animator animator;
    [NonSerialized] public Vector3 directionToPlayer;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        directionToPlayer = (player.position - transform.position).normalized;

        if (health <= 0)
        {
            PoliceIsDead();
            return;
        }

        if (Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            // Patrol();
        }

        UpdateAnimations();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().health -= 20f;
            Debug.Log("Player health: " + collision.gameObject.GetComponent<Player>().health);
        }
    }

    private void UpdateAnimations()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Tolerance to avoid flickering between animations
        float threshold = 0.2f;

        animator.SetBool("isRight", direction.x > threshold);
        animator.SetBool("isLeft", direction.x < -threshold);
        animator.SetBool("isUp", direction.y > threshold);
        animator.SetBool("isDown", direction.y < -threshold);
    }

    private void ChasePlayer()
    {
        transform.Translate(speed * Time.deltaTime * directionToPlayer);
    }

    private void PoliceIsDead()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("isDead");
    }
}
