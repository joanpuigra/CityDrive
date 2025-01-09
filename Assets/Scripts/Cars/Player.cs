using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Vector2 input;
    private float speed = 5f;
    public float health = 100f;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator PoliceWait(Police police)
    {
        police.enabled = false;
        yield return new WaitForSeconds(1f);
        police.enabled = true;
    }

    void Update()
    {
        if (health <= 0)
        {
            PlayerIsDead();
            return;
        }
        else
        {
            HandleInput();
            MovePlayer();
        }

        UpdateAnimations();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Police"))
        {
            collision.gameObject.GetComponent<Police>().health -= 35f;
            Debug.Log("Police health: " + collision.gameObject.GetComponent<Police>().health);
            collision.gameObject.GetComponent<Police>().transform.localPosition -= collision.gameObject.GetComponent<Police>().directionToPlayer * 0.5f;
            StartCoroutine(PoliceWait(collision.gameObject.GetComponent<Police>()));
        }
    }

    private void HandleInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(input.x, input.y, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void UpdateAnimations()
    {
        // Tolerance to avoid flickering between animations
        float threshold = 0.2f;

        animator.SetBool("isRight", input.x > threshold);
        animator.SetBool("isLeft", input.x < -threshold);
        animator.SetBool("isUp", input.y > threshold);
        animator.SetBool("isDown", input.y < -threshold);
    }

    private void PlayerIsDead()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetTrigger("isDead");
    }
}