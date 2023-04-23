using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager
{
    public enum Sound
    {
        Score, Lose, BirdJump, ButtonOver, ButtonClick
    }

    public static void PlaySound(Sound sound)
    {
        AudioSource audioSource = new GameObject("Sound", typeof(AudioSource)).GetComponent<AudioSource>();
        
        audioSource.PlayOneShot(GetAudioClip(sound));

        Object.Destroy(audioSource.gameObject, 5f);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (AssetsHandler.SoundAudioClip soundAudioClip in AssetsHandler.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        
        return null;
    }
}
