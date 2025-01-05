using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatScript : MonoBehaviour
{
    [Header("Settings")]
    private float moveSpeed = 1.5f;
    private float attackRange = 2f;
    private int health = 100;
    private int damage = 10;
    private bool isDead = false;
    public float attackCooldown = 5f;
    private bool readyToAttack = true;

    [Header("Component")]
    public Transform player;
    public Animator enemyAnimator;
    private Rigidbody rb;
    private KeyInput playerScript;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip goatNoise;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.Find("Astronaut");
        playerScript = player.GetComponent<KeyInput>();
    }

    void Update()
    {
        if (isDead) return;

        MakeMaybeNoise();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (playerScript.isInGoatZone && distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer();
        }
        else if (playerScript.isInGoatZone && distanceToPlayer <= attackRange)
        {
            StopMoving();
            Attack();
        }
        else
        {
            StopMoving();
        }

    }

    // A 0.2% de chance de provoquer un son de chèvre
    void MakeMaybeNoise()
    {
        int randomInt = Random.Range(1, 501);
        if (randomInt == 1)
        {
            audioSource.PlayOneShot(goatNoise);
        }
    }

    bool IsPlayerInFront()
    {
        // Position de départ du raycast
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;

        // Direction de la chèvre
        Vector3 rayDirection = transform.forward;

        float rayLength = attackRange + 0.5f;
        int playerLayerMask = LayerMask.GetMask("Player");

        // Check si le rayon à touché le joueur
        if (Physics.Raycast(rayOrigin, rayDirection, rayLength, playerLayerMask))
        {
            return true;
        }
        return false;
    }


    void MoveTowardsPlayer()
    {
        if (!IsAnimationPlaying("Walk"))
        {
            enemyAnimator.SetTrigger("WalkTrigger");
        }
        // Calcule la direction vers le joueur
        Vector3 direction = (player.position - transform.position).normalized;

        // Calcule le vecteur de déplacement
        Vector3 movement = direction * moveSpeed * Time.deltaTime;

        // Bouge la chèvre
        Vector3 newPosition = new Vector3(rb.position.x + movement.x, rb.position.y, rb.position.z + movement.z);
        rb.MovePosition(newPosition);

        // Se tourne vers le joueur
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void Attack()
    {
        if (readyToAttack && !IsAnimationPlaying("Attack"))
        {
            readyToAttack = false;
            enemyAnimator.SetTrigger("AttackTrigger");

            if (IsPlayerInFront())
            {
                playerScript.TakeDamage(damage);
            }

            // Active un CoolDown pour le système d'attaque
            Invoke(nameof(resetAttackCooldown), attackCooldown);
        }
    }

    void resetAttackCooldown()
    {
        Debug.Log("Cooldown fini");
        readyToAttack = true;
    }

    void StopMoving()
    {
        if (!IsAnimationPlaying("Idle"))
        {
            enemyAnimator.SetTrigger("WaitTrigger");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        enemyAnimator.SetTrigger("HitTrigger");

        if (health <= 0)
        {
            Die();
        }

    }

    void Die()
    {
        isDead = true;
        enemyAnimator.SetTrigger("DieTrigger");
    }

    // Helper

    bool IsAnimationPlaying(string animationName)
    {
        return enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

}
