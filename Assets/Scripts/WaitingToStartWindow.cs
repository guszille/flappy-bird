using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingToStartWindow : MonoBehaviour
{
    private void Start()
    {
        BirdController.Instance.OnStartedPlaying += Bird_OnStartedPlaying;

        Show();
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs args)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
