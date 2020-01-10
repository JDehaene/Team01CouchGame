using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayer : MonoBehaviour
{
    [SerializeField] private Image _healthBar,_staminaBar, _wizard, _maxHpImage, _powerImage,_speedImage;
    [SerializeField] private Text _speedStat, _powerStat, _maxHpStat;

    private float _currentHealth, _maxHealth, _currentStamina, _maxStamina;
    private bool _isReady = false;

    private void Update()
    {
        if(_isReady)
        {
            _healthBar.fillAmount = _currentHealth / _maxHealth;
            _staminaBar.fillAmount = _currentStamina / _maxStamina;
        }
    }

    public void StartStats(float currenthp, float maxhp, float currentstamina, float maxstamina)
    {
        _currentHealth = currenthp;
        _maxHealth = maxhp;
        _currentStamina = currentstamina;
        _maxStamina = maxstamina;
        _isReady = true;
    }

    public void TakesDamage(float damage)
    {
        _currentHealth -= damage;
    }

    public void StaminaMeter(float currentstamina, float maxstamina)
    {
        _currentStamina = currentstamina;
        _maxStamina = maxstamina;
    }

    public void ChangedStats(float currenthp ,float speed, float power, float maxhp)
    {
        _currentHealth = currenthp;
        _maxHealth = maxhp;
        _speedStat.text = speed.ToString("n1");
        _powerStat.text = power.ToString("n1");
        _maxHpStat.text = maxhp.ToString("f0");
    }

    public void NewPlayerColor(Color playercolor)
    {
        _wizard.color = playercolor;
        //_maxHpImage. color = playercolor;
        //_powerImage.color = playercolor;
        //_speedImage.color = playercolor;
    }

}
