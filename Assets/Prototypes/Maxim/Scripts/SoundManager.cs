using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _deathSounds;
    [SerializeField]
    private AudioClip _shootingSound;
    [SerializeField]
    private AudioClip _teleportSound;
    [SerializeField]
    private AudioClip[] _pickupSounds;
    [SerializeField]
    private AudioClip _backgroundMusic;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //_audioSource.PlayOneShot(_backgroundMusic);
        //_audioSource.loop = true;
    }

    public void DeathSound()
    {
        int randomIndex = Random.Range(0, _deathSounds.Length + 1);
        _audioSource.PlayOneShot(_deathSounds[randomIndex]);
    }
    public void ShootingSound()
    {
        _audioSource.PlayOneShot(_shootingSound);
    }
    public void TeleportSound()
    {
        _audioSource.PlayOneShot(_teleportSound);
    }
    public void PickupSound()
    {
        int randomIndex = Random.Range(0, _pickupSounds.Length + 1);
        _audioSource.PlayOneShot(_pickupSounds[randomIndex]);
    }
}
