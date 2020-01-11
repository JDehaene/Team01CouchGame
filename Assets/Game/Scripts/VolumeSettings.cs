using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _mixer;

    public void SetLevel(float sliderValue)
    {
        //Log10: Converts value to decibels
        _mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
