using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsHandler : MonoBehaviour
{
    public static AssetsHandler Instance { get; private set; }

    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    public Transform pipeHeadPrefab;
    public Transform pipeBodyPrefab;

    public SoundAudioClip[] soundAudioClipArray;

    private void Awake()
    {
        Instance = this;
    }
}
