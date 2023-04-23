using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public static BirdController Instance { get; private set; }

    public event EventHandler OnStartedPlaying;
    public event EventHandler OnDied;

    private enum State
    { 
        WaitingToStart, Running, Dead
    }

    private const float JUMP_VELOCITY = 100f;

    private State state;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        Instance = this;

        state = State.WaitingToStart;
        rb2d = GetComponent<Rigidbody2D>();

        rb2d.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    state = State.Running;

                    rb2d.bodyType = RigidbodyType2D.Dynamic;

                    Jump();

                    OnStartedPlaying?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.Running:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    Jump();
                }

                transform.eulerAngles = new Vector3(0f, 0f, 0.25f * rb2d.velocity.y);
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        rb2d.velocity = Vector2.up * JUMP_VELOCITY;

        SoundManager.PlaySound(SoundManager.Sound.BirdJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        state = State.Dead;

        rb2d.bodyType = RigidbodyType2D.Static;

        SoundManager.PlaySound(SoundManager.Sound.Lose);
        ScoreController.TrySetNewHighScore(LevelController.Instance.GetPipesPassedCount());

        OnDied?.Invoke(this, EventArgs.Empty);
    }
}
