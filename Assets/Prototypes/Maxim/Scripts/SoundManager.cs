using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _deathSounds;
    [SerializeField]
    private AudioClip[] _shootingSounds;
    [SerializeField]
    private AudioClip _teleportSound;
    [SerializeField]
    private AudioClip[] _pickupSounds;

    [SerializeField]
    private AudioSource _audioSource;

    public void DeathSound()
    {
        int randomIndex = Random.Range(0, _deathSounds.Length);
        _audioSource.PlayOneShot(_deathSounds[randomIndex]);
    }
    public void ShootingSound()
    {
        _audioSource.PlayOneShot(_shootingSounds[0]);
    }
    public void TeleportSound()
    {
        _audioSource.PlayOneShot(_teleportSound);
    }
    public void PickupSound()
    {
        int randomIndex = Random.Range(0, _pickupSounds.Length);
        _audioSource.PlayOneShot(_pickupSounds[randomIndex]);
    }
    public void DarkSound()
    {
        _audioSource.PlayOneShot(_shootingSounds[1]);
    }
    public void EarthSound()
    {
        _audioSource.PlayOneShot(_shootingSounds[2]);
    }
    public void FlamesSound()
    {
        _audioSource.PlayOneShot(_shootingSounds[3]);
    }
    public void SnowSound()
    {
        _audioSource.PlayOneShot(_shootingSounds[4]);
    }
}
