using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentController : MonoBehaviour
{
    [SerializeField] private Transform[] groundSliceArray;
    [SerializeField] private Transform[] cloudsSliceArray;

    private const float ENVIROMENT_MOVE_SPEED = 30f;
    private const float GROUND_INITIAL_X_POSITION = 160f;
    private const float GROUND_FINAL_X_POSITION = -160f;
    private const float CLOUDS_INITIAL_X_POSITION = 130f;
    private const float CLOUDS_FINAL_X_POSITION = -130f;

    private void Update()
    {
        if (LevelController.Instance.GetState() == LevelController.State.Running)
        {
            HandleGroundMovement();
            HandleCloudsMovement();
        }
    }

    private void HandleGroundMovement()
    {
        foreach (Transform groundSlice in groundSliceArray)
        {
            groundSlice.position += new Vector3(-1f, 0f, 0f) * ENVIROMENT_MOVE_SPEED * Time.deltaTime;

            if (groundSlice.position.x <= GROUND_FINAL_X_POSITION)
            {
                groundSlice.position += new Vector3(2f, 0f, 0f) * GROUND_INITIAL_X_POSITION;
            }
        }
    }

    private void HandleCloudsMovement()
    {
        foreach (Transform cloudsSlice in cloudsSliceArray)
        {
            cloudsSlice.position += new Vector3(-1f, 0f, 0f) * ENVIROMENT_MOVE_SPEED * Time.deltaTime;

            if (cloudsSlice.position.x <= CLOUDS_FINAL_X_POSITION)
            {
                cloudsSlice.position += new Vector3(3f, 0f, 0f) * CLOUDS_INITIAL_X_POSITION;
            }
        }
    }
}
