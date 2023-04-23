using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    public class OnPipesPassedCountChangedEventArgs : EventArgs { public int pipesPassedCount; }
    public event EventHandler<OnPipesPassedCountChangedEventArgs> OnPipesPassedCountChanged;

    public enum Difficulty
    {
        Easy, Medium, Hard, Impossible
    }

    public enum State
    {
        WaitingToStart, Running, GameIsOver
    }

    private class Pipe
    {
        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool upsideDown;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool upsideDown)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
            this.upsideDown = upsideDown;
        }

        public void DestroySelf()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public bool IsUpsideDown()
        {
            return upsideDown;
        }
    }

    private const float CAMERA_VERTICAL_SIZE = 50f;
    private const float PIPE_BODY_WIDTH = 7.5f;
    private const float PIPE_HEAD_HEIGHT = 3.75f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float BIRD_X_POSITION = 0f;

    private State state;
    private List<Pipe> pipesInstantiated;
    private float pipeSpawnTimer;
    private float pipeSpawnTimerMax;
    private float gapSize;
    private int pipesSpawned = 0;
    private int pipesPassedCount = 0;

    private void Awake()
    {
        Instance = this;

        state = State.WaitingToStart;
        pipesInstantiated = new List<Pipe>();
        
        SetDifficulty(Difficulty.Easy);
    }

    private void Start()
    {
        BirdController.Instance.OnStartedPlaying += Bird_OnStartedPlaying;
        BirdController.Instance.OnDied += Bird_OnDied;
    }

    private void Update()
    {
        if (state == State.Running)
        {
            HandlePipesMovement();
            HandlePipesSpawning();
        }
    }

    private void Bird_OnStartedPlaying(object sender, EventArgs args)
    {
        state = State.Running;
    }

    private void Bird_OnDied(object sender, EventArgs args)
    {
        state = State.GameIsOver;
    }

    private void HandlePipesMovement()
    {
        for (int i = 0; i < pipesInstantiated.Count; i++)
        {
            Pipe pipe = pipesInstantiated[i];

            bool isOnTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            bool isOnTheLeftOfBird = pipe.GetXPosition() <= BIRD_X_POSITION;

            if (!pipe.IsUpsideDown() && isOnTheRightOfBird && isOnTheLeftOfBird)
            {
                pipesPassedCount += 1;

                SoundManager.PlaySound(SoundManager.Sound.Score);

                OnPipesPassedCountChanged?.Invoke(this, new OnPipesPassedCountChangedEventArgs
                {
                    pipesPassedCount = pipesPassedCount
                });
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.DestroySelf();

                pipesInstantiated.Remove(pipe);

                i -= 1;
            }
        }
    }

    private void HandlePipesSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;

        if (pipeSpawnTimer < 0f)
        {
            float heightEdgeLimit = 10f;
            float totalCameraVerticalSize = 2f * CAMERA_VERTICAL_SIZE;
            float minHeight = (gapSize / 2f) + heightEdgeLimit;
            float maxHeight = totalCameraVerticalSize - (gapSize / 2f) - heightEdgeLimit;
            float height = UnityEngine.Random.Range(minHeight, maxHeight);

            CreatePipePair(height, gapSize, PIPE_SPAWN_X_POSITION);

            pipeSpawnTimer += pipeSpawnTimerMax;
        }
    }

    private void CreatePipe(float height, float x, bool upsideDown = false)
    {
        float pipeHeadYPosition = height - (PIPE_HEAD_HEIGHT / 2f) - CAMERA_VERTICAL_SIZE;
        float pipeBodyYPosition = -CAMERA_VERTICAL_SIZE;

        if (upsideDown)
        {
            pipeHeadYPosition = CAMERA_VERTICAL_SIZE - height + (PIPE_HEAD_HEIGHT / 2f);
            pipeBodyYPosition = CAMERA_VERTICAL_SIZE;
        }

        Transform pipeHead = Instantiate(AssetsHandler.Instance.pipeHeadPrefab);

        pipeHead.position = new Vector3(x, pipeHeadYPosition);

        Transform pipeBody = Instantiate(AssetsHandler.Instance.pipeBodyPrefab);
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        BoxCollider2D pipeBodyBoxCollider2d = pipeBody.transform.GetComponent<BoxCollider2D>();

        if (upsideDown)
        {
            pipeBody.localScale = new Vector3(1f, -1f, 1f);
        }

        pipeBody.position = new Vector3(x, pipeBodyYPosition);
        pipeBodySpriteRenderer.size = new Vector2(PIPE_BODY_WIDTH, height);
        pipeBodyBoxCollider2d.size = new Vector2(PIPE_BODY_WIDTH, height);
        pipeBodyBoxCollider2d.offset = new Vector2(0f, height / 2f);

        pipesInstantiated.Add(new Pipe(pipeHead, pipeBody, upsideDown));
    }

    private void CreatePipePair(float gapYCenter, float gapSize, float x)
    {
        CreatePipe(gapYCenter - (gapSize / 2f), x);
        CreatePipe((2f * CAMERA_VERTICAL_SIZE) - gapYCenter - (gapSize / 2f), x, true);

        pipesSpawned += 1;

        SetDifficulty(GetDifficulty());
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                pipeSpawnTimerMax = 1.4f;
                gapSize = 60f;
                break;
            case Difficulty.Medium:
                pipeSpawnTimerMax = 1.2f;
                gapSize = 50f;
                break;
            case Difficulty.Hard:
                pipeSpawnTimerMax = 1.0f;
                gapSize = 40f;
                break;
            case Difficulty.Impossible:
                pipeSpawnTimerMax = 0.8f;
                gapSize = 30f;
                break;
        }
    }

    public State GetState()
    {
        return state;
    }

    public int GetPipesPassedCount()
    {
        return pipesPassedCount;
    }
}
