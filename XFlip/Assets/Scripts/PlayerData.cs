using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int PlayerCurrentCoin;

    [Header("Required coin for per level upgrade")]
    public int CoinValuePerLevel;

    [Header("Player behaviour datas")]
    public int Speed;
    public float Accelaration;
    public float LeanSpeed;

    [Header("Current upgrade level")]
    public int SpeedLevel;
    public float AccelarationLevel;
    public float LeanSpeedLevel;

    [Header("Upgrade value for per one level")]
    public int SpeedUpgradeValue;
    public float AccelarationUpgradeValue;
    public float LeanSpeedUpgradeValue;

}
