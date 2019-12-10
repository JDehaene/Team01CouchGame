using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private ParticleSystem _statsChangeParticle;
    [SerializeField] private ParticleSystem _teleportParticle;

    private Vector3 _offset = Vector3.up;

    public void DeathParticleEffect(Vector3 pos)
    {
        ParticleSystem pSystem = Instantiate(_deathParticle, pos + _offset, Quaternion.identity);
        _deathParticle.Play();
        Destroy(pSystem, _deathParticle.duration);
    }
    public void TeleportParticleEffect(Vector3 pos)
    {
        ParticleSystem pSystem = Instantiate(_teleportParticle, pos + _offset, Quaternion.identity);
        _teleportParticle.Play();
        Destroy(pSystem, _teleportParticle.duration);
    }
    public void StatsParticleEffect(Vector3 pos)
    {
        ParticleSystem pSystem = Instantiate(_statsChangeParticle, pos + _offset, Quaternion.identity);
        _statsChangeParticle.Play();
        Destroy(pSystem, _statsChangeParticle.duration);
    }
}
