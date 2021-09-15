using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float acceleration;
    public float maxSpeed;
    public float maxSpeedIncreaseRate;
    public float jumpAcceleration;
    private bool isJumping, isGrounded;
    private Rigidbody2D rb;
    public float fallPositionY;
    public CameraMovement mainCamera;
    public GameObject gameOverPanel;

    [Header("Raycast Attributes")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;

    [Header("Reference")]
    private Animator animator;
    private CharacterSoundController soundController;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        soundController = GetComponent<CharacterSoundController>();
        lastPositionX = transform.position.x;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isGrounded)
            {
                isJumping = true;
                soundController.PlayJump();
            }
        }

        animator.SetBool("isGrounded", isGrounded);

        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if(scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if(transform.position.y < fallPositionY)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        score.FinishScoring();
        mainCamera.enabled = false;
        gameOverPanel.SetActive(true);
        this.enabled = false;
        rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        CheckIsGrounded();

        Vector2 velocity = rb.velocity;

        if (isJumping)
        {
            velocity.y += jumpAcceleration;
            isJumping = false;
        }

        velocity.x = Mathf.Clamp(velocity.x + acceleration * Time.deltaTime, 0f, maxSpeed);
        rb.velocity = velocity;
    }

    private void CheckIsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        if (hit)
        {
            if(!isGrounded && rb.velocity.y <= 0f)
                isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    //fungsi untuk menambah speed dari player
    public void IncreaseMaxSpeed()
    {
        maxSpeed += maxSpeedIncreaseRate;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Cactus" || collision.tag == "Bird")
        {
            GameOver();
        }
    }
}
