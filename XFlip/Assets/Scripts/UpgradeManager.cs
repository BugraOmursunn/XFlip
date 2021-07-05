using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerData _playerData;

    void Start()
    {
        ObserverManager.UpgradeSpeed.AddListener(UpgradePlayerSpeed);
        ObserverManager.UpgradeAccelaration.AddListener(UpgradePlayerAccelaration);
        ObserverManager.UpgradeLeanSpeed.AddListener(UpgradePlayerLeanSpeed);
    }
    public void UpgradePlayerSpeed()
    {
        if (_playerData.PlayerCurrentCoin >= _playerData.CoinValuePerLevel)
        {
            _playerData.Speed += _playerData.SpeedUpgradeValue;
            _playerData.SpeedLevel++;
            _playerData.PlayerCurrentCoin -= _playerData.CoinValuePerLevel;

            ObserverManager.RefreshTexts?.Invoke();
        }
    }
    public void UpgradePlayerAccelaration()
    {
        if (_playerData.PlayerCurrentCoin >= _playerData.CoinValuePerLevel)
        {
            _playerData.Accelaration += _playerData.AccelarationUpgradeValue;
            _playerData.AccelarationLevel++;
            _playerData.PlayerCurrentCoin -= _playerData.CoinValuePerLevel;

            ObserverManager.RefreshTexts?.Invoke();
        }
    }
    public void UpgradePlayerLeanSpeed()
    {
        if (_playerData.PlayerCurrentCoin >= _playerData.CoinValuePerLevel)
        {
            _playerData.LeanSpeed += _playerData.LeanSpeedUpgradeValue;
            _playerData.LeanSpeedLevel++;
            _playerData.PlayerCurrentCoin -= _playerData.CoinValuePerLevel;

            ObserverManager.RefreshTexts?.Invoke();
        }

    }


}
