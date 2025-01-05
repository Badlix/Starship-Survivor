using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInput : MonoBehaviour
{
    public Animator characterAnimator;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    private bool readyToJump = true;

    [Header("Character Settings")]
    private int health = 100;
    private int damage = 50;
    private bool isDead = false;
    public float playerHeight;
    public Image healtBar;

    [Header("Character items")]
    public bool hasKey = false;
    public bool hasWeapon = false;
    public int numberOfFuelTank = 0;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode atkKey = KeyCode.Mouse0;
    public KeyCode jumpKeyGameController = KeyCode.JoystickButton1;
    public KeyCode atkKeyGameController = KeyCode.JoystickButton0;

    [Header("Ground Check")]
    public LayerMask ground;
    public LayerMask deathZone;
    private bool grounded = false;
    public Transform orientation;
    public Transform LaserEmitter;
    float horizontalInput;
    public float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private float rayLength = 0.55f;
    public GameObject endGame;
    public bool isInGoatZone = false;

    [Header("Particles")]
    public GameObject particlesSystem;

    [Header("Teleporter")]
    private Vector3 pos_teleporter_1 = new Vector3(-5, 0, 9);
    private Vector3 pos_teleporter_2 = new Vector3(-2, 11, 11);

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dieAudio;
    public AudioClip hurtAudio;
    public AudioClip lightSaberAudio;
    public AudioClip walkAudio;

    void Start()
    {
        characterAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isDead && animationIsNotPlaying("Die"))
        {
            Die();
        }
        else
        {
            if (rb.position.y < -10)
            {
                Die();
            }
        }

        if (isDead) return;

        grounded = Physics.Raycast(LaserEmitter.position, Vector3.down, rayLength, ground);

        IsInGoatZone();
        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Activation/Désactivation du système de particules
        if (animationIsPlaying("Walk"))
        {
            particlesSystem.SetActive(true);
        }
        else
        {
            particlesSystem.SetActive(false);
        }
    }

    void IsInGoatZone()
    {
        if (Physics.Raycast(LaserEmitter.position, Vector3.down, out RaycastHit hit, rayLength, ground))
        {
            if (hit.collider.CompareTag("GoatZone"))
            {
                isInGoatZone = true;
            }
            else
            {
                isInGoatZone = false;
            }

        }
    }

    void FixedUpdate()
    {

        if (isDead) return;

        MovePlayer();
        isDead = Physics.Raycast(LaserEmitter.position, Vector3.down, rayLength, deathZone);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) || Input.GetKey(jumpKeyGameController))
        {
            if (readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        if (Input.GetKey(atkKey) || Input.GetKey(atkKeyGameController))
        {
            if (grounded && hasWeapon && animationIsNotPlaying("Attack"))
            {
                playAnimation("AttackTrigger");
                audioSource.PlayOneShot(lightSaberAudio);
            }
        }
    }


    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (verticalInput == 0 && horizontalInput == 0 && grounded && animationIsNotPlaying("Waiting"))
        {
            playAnimation("WaitingTrigger");
        }
        else if (grounded && (verticalInput != 0 || horizontalInput != 0))
        {
            if (animationIsNotPlaying("Walk"))
            {
                playAnimation("WalkTrigger");
            }
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(walkAudio);
            }
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);

        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }
    }

    private void Jump()
    {
        playAnimation("JumpTrigger");
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Teleporter_1")
        {
            rb.transform.position = pos_teleporter_2;

        }
        else if (other.gameObject.tag == "Teleporter_2")
        {
            rb.transform.position = pos_teleporter_1;
        }
        else if (other.gameObject.CompareTag("Goat"))
        {
            InfligeDamage(other.gameObject);
        }
    }

    void Die()
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(dieAudio);
        }
        playAnimation("DieTrigger");
        GameStateManager endGameScript = endGame.GetComponent<GameStateManager>();
        endGameScript.isDead = true;
        endGameScript.gameEnd = true;
    }

    public void InfligeDamage(GameObject aGoat)
    {
        GoatScript goatScript = aGoat.GetComponent<GoatScript>();
        goatScript.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        audioSource.PlayOneShot(hurtAudio);
        health -= damage;
        healtBar.fillAmount = health / 100f;
        if (health <= 0)
        {
            Die();
        }

    }

    // Helper 

    void playAnimation(string triggerName)
    {
        characterAnimator.SetTrigger(triggerName);
    }

    bool animationIsPlaying(string animationName)
    {
        return this.characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    bool animationIsNotPlaying(string animationName)
    {
        return !this.characterAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    // Fonction qui sert à rien, l'animation d'attaque m'oblige à la mettre
    // et j'arrive pas à enlever le mode read-only des animations.
    public void Hit() { }
}

